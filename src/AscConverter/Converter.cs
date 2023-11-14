using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables.Cells.CellMembers;
using Repository;
using System.Xml.Serialization;

namespace AscConverter;
public class Converter
{
    private readonly AscXmlObjects.Timetable _ascTimetable;
    private readonly TimetableContext _dbContext;

    public Converter(string ascXmlPath, TimetableContext dbContext)
    {
        var serializer = new XmlSerializer(typeof(AscXmlObjects.Timetable));
        using var reader = new StreamReader(ascXmlPath);
        _ascTimetable = serializer.Deserialize(reader) as AscXmlObjects.Timetable ?? throw new InvalidCastException("Не получилось привести десериализованный объект к Timetable");
        _dbContext = dbContext;
    }

    public void /*List<StableTimetable>*/ Convert()
    {
        OOPTypes.Timetable oopTimetable = ConvertToOOP(_ascTimetable);
        FillDbContext(oopTimetable);
        ConvertToStableTimetable(oopTimetable);
        _dbContext.SaveChanges();
    }

    private static OOPTypes.Timetable ConvertToOOP(AscXmlObjects.Timetable ascTimetable)
    {
        var groups = ascTimetable.Classes.Class; // группы
        var podgroups = ascTimetable.Groups.Group; // группы
        var teachers = ascTimetable.Teachers.Teacher; // учителя
        var korpusa = ascTimetable.Buildings.Building; //корпусы
        var periods = ascTimetable.Periods.Period; // лессонтаймы
        var daysdef = ascTimetable.Daysdefs.Daysdef; // дни недели
        var classrooms = ascTimetable.Classrooms.Classroom; // кабинеты
        var subjects = ascTimetable.Subjects.Subject; // предметы
        var weeksdef = ascTimetable.Weeksdefs.Weeksdef; // недели
        var lessons = ascTimetable.Lessons.Lesson; // сборник объкетов
        var cards = ascTimetable.Cards.Card; // карточка которая рисуется в расписании


        var normgroups = groups.Select(e => new OOPTypes.Group() { Id = e.Id, Name = e.Name }).ToList();
        var normpodgroups = podgroups.Select(e => new OOPTypes.SubGroup { Id = e.Id, Name = e.Name }).ToList();
        var normteachers = teachers.Select(e => new OOPTypes.Teacher { Firstname = e.Firstname, Id = e.Id, Lastname = e.Lastname }).ToList();
        var normkorpusa = korpusa.Select(e => new OOPTypes.Building { Id = e.Id, Name = e.Name }).ToList();
        var normperiods = periods.Select(e => new OOPTypes.Period() { EndTime = TimeOnly.Parse(e.Endtime), Number = e._period, StartTime = TimeOnly.Parse(e.Starttime) }).ToList();
        var normdaysdef = daysdef.Select(e => new OOPTypes.Daydef() { DayCode = e.Days, Id = e.Id, Name = e.Name }).ToList();
        var normclassrooms = classrooms.Select(e => new OOPTypes.Cabinet() { Id = e.Id, Name = e.Name, ShortName = e.Short, Building = normkorpusa.Single(x => x.Id == e.Buildingid) }).ToList();
        var normsubjects = subjects.Select(e => new OOPTypes.Subject() { Id = e.Id, Name = e.Name, ShortName = e.Short }).ToList();
        var normweeksdef = weeksdef.Select(e => new OOPTypes.WeekDef() { Id = e.Id, ShortName = e.Short, WeekCode = e.Weeks, Name = e.Name }).ToList();

        var normPodgroupsDestincted = normpodgroups.Select(e => e.Name).Distinct().ToList();

        var normlessons = new List<OOPTypes.Lesson>();
        var normcards = new List<OOPTypes.Card>();

        foreach (var item in lessons)
        {
            normlessons.Add(new OOPTypes.Lesson()
            {
                Id = item.Id,
                Cabinet = normclassrooms.SingleOrDefault(e => e.Id == item.Classroomids),
                Group = normgroups.SingleOrDefault(e => e.Id == item.Classids),
                Subject = normsubjects.SingleOrDefault(e => e.Id == item.Subjectid),
                Teacher = normteachers.SingleOrDefault(e => e.Id == item.Teacherids),
                Daysdef = normdaysdef.SingleOrDefault(e => e.Id == item.Daysdefid),
                WeeksDef = normweeksdef.SingleOrDefault(e => e.Id == item.Weeksdefid),
                SubGroup = normpodgroups.SingleOrDefault(e => e.Id == item.Groupids),
                PeriodsPerCard = item.Periodspercard,
                PeriodsPerWeek = item.Periodsperweek
            });
        }

        foreach (var item in cards)
        {
            normcards.Add(new OOPTypes.Card
            {
                Daysdef = normdaysdef.Single(e => e.DayCode == item.Days),
                Cabinet = normclassrooms.Single(e => e.Id == item.Classroomids),
                Week = normweeksdef.Single(e => e.WeekCode == item.Weeks),
                Period = normperiods.Single(e => e.Number == item.Period),
                Lesson = normlessons.Single(e => e.Id == item.Lessonid),
                Id = null
            });
        }

        var oopTypesTimetable = new OOPTypes.Timetable()
        {
            Cards = normcards,
            Buildings = normkorpusa,
            Cabinets = normclassrooms,
            Daydefs = normdaysdef,
            Groups = normgroups,
            Lessons = normlessons,
            Periods = normperiods,
            SubGroups = normpodgroups,
            Subjects = normsubjects,
            Teachers = normteachers,
            WeekDefs = normweeksdef,
            SubGroupsDestincted = normPodgroupsDestincted
        };

        return oopTypesTimetable;
    }

    private void /*List<StableTimetable>*/ ConvertToStableTimetable(OOPTypes.Timetable oopTimetable)
    {
        var cards = oopTimetable.Cards;
        List<StableTimetable> stableTimetables = new List<StableTimetable>();

        foreach (var group in oopTimetable.Groups)
        {
            var cellsOfCurrentGroup = new List<StableTimetableCell>();
            var cardsOfCurrentGroup = oopTimetable.Cards.Where(e => e.Lesson.Group.Id == group.Id).ToList();

            foreach (var card in cardsOfCurrentGroup)
            {
                WeekEvenness weekEvenness = DetermineWeekEvenness(card.Week.WeekCode);
                DayOfWeek dayOfWeek = DetermineDayOfWeek(card.Daysdef.DayCode);
                TeacherCM teacherCM = _dbContext.Set<TeacherCM>().Single(e => card.Lesson.Teacher.Id == e.AscId);
                Subject subject = _dbContext.Set<Subject>().Single(e => e.AscId == card.Lesson.Subject.Id);
                LessonTime lessonTime = _dbContext.Set<LessonTime>().Single(e => e.Number == int.Parse(card.Period.Number));
                Cabinet cabinet = _dbContext.Set<Cabinet>().Single(e => e.AscId == card.Cabinet.Id);
                SubGroup subGroup = DetermineSubgroup(card.Lesson.SubGroup.Name);

                switch (weekEvenness)
                {
                    case WeekEvenness.Both:
                        cellsOfCurrentGroup.Add(new StableTimetableCellBuilder(default).AddSubject(subject).AddLessonTime(lessonTime).AddTeacher(teacherCM)
                            .AddCabinet(cabinet).AddSubGroup(subGroup).AddDayOfWeek(dayOfWeek).AddIsWeekEven(true).Build());
                        cellsOfCurrentGroup.Add(new StableTimetableCellBuilder(default).AddSubject(subject).AddLessonTime(lessonTime).AddTeacher(teacherCM)
                            .AddCabinet(cabinet).AddSubGroup(subGroup).AddDayOfWeek(dayOfWeek).AddIsWeekEven(false).Build());
                        break;

                    case WeekEvenness.Even:
                        cellsOfCurrentGroup.Add(new StableTimetableCellBuilder(default).AddSubject(subject).AddLessonTime(lessonTime).AddTeacher(teacherCM)
                            .AddCabinet(cabinet).AddSubGroup(subGroup).AddDayOfWeek(dayOfWeek).AddIsWeekEven(true).Build());
                        break;

                    case WeekEvenness.Odd:
                        cellsOfCurrentGroup.Add(new StableTimetableCellBuilder(default).AddSubject(subject).AddLessonTime(lessonTime).AddTeacher(teacherCM)
                            .AddCabinet(cabinet).AddSubGroup(subGroup).AddDayOfWeek(dayOfWeek).AddIsWeekEven(false).Build());
                        break;

                    default:
                        throw new Exception("Четностb недели не определена.");
                }

            }
            var currentStableTimetable = new StableTimetable(default, new Group(default, group.Name), cellsOfCurrentGroup);
            _dbContext.Add(currentStableTimetable);
            //stableTimetables.Add(currentStableTimetable);
        }
        //return stableTimetables;
    }

    private void FillDbContext(OOPTypes.Timetable oopTimetable)
    {
        _dbContext.AddRange(oopTimetable.Teachers.Select(e => new TeacherCM(default, e.Lastname, e.Firstname, string.Empty) { AscId = e.Id }));
        _dbContext.AddRange(oopTimetable.Subjects.Select(e => new Subject(default, e.Name) { AscId = e.Id }));
        _dbContext.AddRange(oopTimetable.Cards.Select(e => new LessonTime(default, int.Parse(e.Period.Number), e.Period.StartTime, e.Period.EndTime)).Distinct());
        _dbContext.AddRange(oopTimetable.Cabinets.Select(e=> new Cabinet(default, e.Building.Name, e.Name) { AscId = e.Id }));
        _dbContext.AddRange(oopTimetable.Groups.Select(e=> new Group(default, e.Name) { AscId = e.Id}));
        _dbContext.SaveChanges();
    }




    private static DayOfWeek DetermineDayOfWeek(string dayOfWeekCode)
    {
        const string MONDAY_CODE = "10000";
        const string TUESDAY_CODE = "01000";
        const string WEDNESDAY_CODE = "00100";
        const string THURSDAY_CODE = "00010";
        const string FRIDAY_CODE = "00001";

        return dayOfWeekCode switch
        {
            MONDAY_CODE => DayOfWeek.Monday,
            TUESDAY_CODE => DayOfWeek.Tuesday,
            WEDNESDAY_CODE => DayOfWeek.Wednesday,
            THURSDAY_CODE => DayOfWeek.Thursday,
            FRIDAY_CODE => DayOfWeek.Friday,
            _ => throw new ArgumentException("День недели не определен.")
        };
    }

    private static WeekEvenness DetermineWeekEvenness(string weekCode)
    {
        const string ANY_WEEK_CODE = "11";
        const string EVEN_WEEK_CODE = "10";
        const string ODD_WEEK_CODE = "01";

        return weekCode switch
        {
            ANY_WEEK_CODE => WeekEvenness.Both,
            EVEN_WEEK_CODE => WeekEvenness.Even,
            ODD_WEEK_CODE => WeekEvenness.Odd,
            _ => throw new ArgumentException("Тип недели не определен.")
        };
    }

    private static SubGroup DetermineSubgroup(string subgroupCode)
    {
        return subgroupCode switch
        {
            "Весь класс" => SubGroup.All,
            "1 группа" => SubGroup.FirstGroup,
            "2 группа" => SubGroup.SecondGroup,
            "Мальчики" => SubGroup.Males,
            "Девочки" => SubGroup.Females,
            _ => throw new ArgumentException("Подгруппа не определена.")
        };
    }
    private enum WeekEvenness { Both = 0, Even = 1, Odd = 2 }
}