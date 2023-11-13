using System.Xml.Serialization;
using AscXmlObjects;

namespace ConsoleApp1;
class Program
{

    static CorrectCLRTypes.Timetable ConvertToCLRTimetable(string ascExportXmlPath)
    {
        var serializer = new XmlSerializer(typeof(Timetable));
        using var reader = new StreamReader(ascExportXmlPath);

        AscXmlObjects.Timetable timetable = serializer.Deserialize(reader) as Timetable ?? throw new InvalidCastException("Не получилось привести десериализованный объект к Timetable");
        var groups = timetable.Classes.Class; // группы
        var podgroups = timetable.Groups.Group; // группы
        var teachers = timetable.Teachers.Teacher; // учителя
        var korpusa = timetable.Buildings.Building; //корпусы
        var periods = timetable.Periods.Period; // лессонтаймы
        var daysdef = timetable.Daysdefs.Daysdef; // дни недели
        var classrooms = timetable.Classrooms.Classroom; // кабинеты
        var subjects = timetable.Subjects.Subject; // предметы
        var weeksdef = timetable.Weeksdefs.Weeksdef; // недели
        var lessons = timetable.Lessons.Lesson; // сборник объкетов
        var cards = timetable.Cards.Card; // карточка которая рисуется в расписании


        var normgroups = groups.Select(e => new CorrectCLRTypes.Group() { Id = e.Id, Name = e.Name }).ToList();
        var normpodgroups = podgroups.Select(e => new CorrectCLRTypes.SubGroup { Id = e.Id, Name = e.Name }).ToList();
        var normteachers = teachers.Select(e => new CorrectCLRTypes.Teacher { Firstname = e.Firstname, Id = e.Id, Lastname = e.Lastname }).ToList();
        var normkorpusa = korpusa.Select(e => new CorrectCLRTypes.Building { Id = e.Id, Name = e.Name }).ToList();
        var normperiods = periods.Select(e => new CorrectCLRTypes.Period() { EndTime = TimeOnly.Parse(e.Endtime), Number = e._period, StartTime = TimeOnly.Parse(e.Starttime) }).ToList();
        var normdaysdef = daysdef.Select(e => new CorrectCLRTypes.Daydef() { DayCode = e.Days, Id = e.Id, Name = e.Name }).ToList();
        var normclassrooms = classrooms.Select(e => new CorrectCLRTypes.Cabinet() { CabinetId = e.Id, Name = e.Name, ShortName = e.Short, Building = normkorpusa.Single(x => x.Id == e.Buildingid) }).ToList();
        var normsubjects = subjects.Select(e => new CorrectCLRTypes.Subject() { Id = e.Id, Name = e.Name, ShortName = e.Short }).ToList();
        var normweeksdef = weeksdef.Select(e => new CorrectCLRTypes.WeekDef() { Id = e.Id, ShortName = e.Short, WeekCode = e.Weeks, Name = e.Name }).ToList();

        var normlessons = new List<CorrectCLRTypes.Lesson>();
        var normcards = new List<CorrectCLRTypes.Card>();

        foreach (var item in lessons)
        {
            normlessons.Add(new CorrectCLRTypes.Lesson()
            {
                Id = item.Id,
                Cabinet = normclassrooms.Single(e => e.CabinetId == item.Classroomids),
                Group = normgroups.Single(e => e.Id == item.Classids),
                Subject = normsubjects.Single(e => e.Id == item.Subjectid),
                Teacher = normteachers.Single(e => e.Id == item.Teacherids),
                Daysdef = normdaysdef.Single(e => e.Id == item.Daysdefid),
                WeeksDef = normweeksdef.Single(e => e.Id == item.Weeksdefid),
                PodGroup = normpodgroups.Single(e => e.Id == item.Groupids),
                PeriodsPerCard = item.Periodspercard,
                PeriodsPerWeek = item.Periodsperweek
            });
        }

        foreach (var item in cards)
        {
            normcards.Add(new CorrectCLRTypes.Card
            {
                Daysdef = normdaysdef.Single(e => e.DayCode == item.Days),
                Cabinet = normclassrooms.Single(e => e.CabinetId == item.Classroomids),
                Week = normweeksdef.Single(e => e.WeekCode == item.Weeks),
                Period = normperiods.Single(e => e.Number == item.Period),
                Lesson = normlessons.Single(e => e.Id == item.Lessonid),
                Id = null
            });
        }

        return new CorrectCLRTypes.Timetable()
        {
            Cards = normcards,
            Buildings = normkorpusa,
            Cabinets = normclassrooms,
            Daydefs = normdaysdef,
            Groups = normgroups,
            Lessons = normlessons,
            Periods = normperiods,
            PodGroups = normpodgroups,
            Subjects = normsubjects,
            Teachers = normteachers,
            WeekDefs = normweeksdef
        };
    }
}