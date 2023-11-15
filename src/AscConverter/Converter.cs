using Microsoft.EntityFrameworkCore;
using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables.Cells.CellMembers;
using Repository;
using System.Xml;
using System.Xml.Serialization;

namespace AscConverter;
public class Converter
{
    private AscXmlObjects.Timetable _ascTimetable = null!;
    private readonly TimetableContext _dbContext;
    private bool _isReaded;

    public Converter(TimetableContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Читает Stream, потом вызывает Dispose.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    /// <exception cref="InvalidCastException"></exception>
    public async Task ReadAsync(Stream stream)
    {
        var serializer = new XmlSerializer(typeof(AscXmlObjects.Timetable));
        var reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true });
        _isReaded = await reader.ReadAsync();
        _ascTimetable = serializer.Deserialize(reader) as AscXmlObjects.Timetable ?? throw new InvalidCastException("Не получилось привести десериализованный объект к Timetable");
        reader.Dispose();
        await stream.DisposeAsync();
    }

    public async Task SaveToDbAsync()
    {
        if (_isReaded is false) throw new IOException("Xml не прочитан.");

        OOPTypes.Timetable oopTimetable = ConvertToOOP(_ascTimetable);
        await FillDbContext(oopTimetable);
        await ConvertToStableTimetableAndSaveToContext(oopTimetable);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Конверит из XML классов в более нормальные классы расписания. которые хранят уже объекты в себе.
    /// </summary>
    /// <param name="ascTimetable"></param>
    /// <returns></returns>
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

                // В базе XML есть лессоны, где два кабинета и второго кабиента не существует.
                Cabinet = normclassrooms.SingleOrDefault(e => e.Id == item.Classroomids)!,
                Group = normgroups.Single(e => e.Id == item.Classids),
                Subject = normsubjects.Single(e => e.Id == item.Subjectid),
                Teacher = normteachers.Single(e => e.Id == item.Teacherids),
                Daysdef = normdaysdef.Single(e => e.Id == item.Daysdefid),
                WeeksDef = normweeksdef.Single(e => e.Id == item.Weeksdefid),
                SubGroup = normpodgroups.Single(e => e.Id == item.Groupids),
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

    /// <summary>
    /// Достает данные об объектах ячейки из дбконтекста и с помощью них создает ячейки, расписания и сохраняет обратно в контекст всё.
    /// </summary>
    /// <param name="oopTimetable"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private async Task ConvertToStableTimetableAndSaveToContext(OOPTypes.Timetable oopTimetable)
    {
        var cards = oopTimetable.Cards;
        List<StableTimetable> stableTimetables = new();

        foreach (var group in await _dbContext.Set<Group>().ToListAsync())
        {
            var cellsOfCurrentGroup = new List<StableTimetableCell>();
            var cardsOfCurrentGroup = oopTimetable.Cards.Where(e => e.Lesson.Group.Id == group.AscId).ToList();

            foreach (var card in cardsOfCurrentGroup)
            {
                WeekEvenness weekEvenness = DetermineWeekEvenness(card.Week.WeekCode);
                DayOfWeek dayOfWeek = DetermineDayOfWeek(card.Daysdef.DayCode);
                TeacherCM teacherCM = await _dbContext.Set<TeacherCM>().SingleAsync(e => card.Lesson.Teacher.Id == e.AscId);
                Subject subject = await _dbContext.Set<Subject>().SingleAsync(e => e.AscId == card.Lesson.Subject.Id);
                LessonTime lessonTime = await _dbContext.Set<LessonTime>().SingleAsync(e => e.Number == int.Parse(card.Period.Number));
                Cabinet cabinet = await _dbContext.Set<Cabinet>().SingleAsync(e => e.AscId == card.Cabinet.Id);
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
                        throw new ArgumentException("Четностb недели не определена.");
                }

            }
            var currentStableTimetable = new StableTimetable(default, group, cellsOfCurrentGroup);
            await _dbContext.AddAsync(currentStableTimetable);
        }
    }

    /// <summary>
    /// Заполяняет контекст инфой для ячеек и сейвит всё это в субд, чтобы получить айдишники.
    /// </summary>
    /// <param name="oopTimetable"></param>
    private async Task FillDbContext(OOPTypes.Timetable oopTimetable)
    {
        await _dbContext.AddRangeAsync(oopTimetable.Teachers.Select(e => new TeacherCM(default, e.Lastname, e.Firstname, string.Empty) { AscId = e.Id }));
        await _dbContext.AddRangeAsync(oopTimetable.Subjects.Select(e => new Subject(default, e.Name) { AscId = e.Id }));
        await _dbContext.AddRangeAsync(oopTimetable.Cards.Select(e => new LessonTime(default, int.Parse(e.Period.Number), e.Period.StartTime, e.Period.EndTime)).Distinct());
        await _dbContext.AddRangeAsync(oopTimetable.Cabinets.Select(e=> new Cabinet(default, e.Building.Name, e.Name) { AscId = e.Id }));
        await _dbContext.AddRangeAsync(oopTimetable.Groups.Select(e=> new Group(default, e.Name) { AscId = e.Id}));
        await _dbContext.SaveChangesAsync();
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