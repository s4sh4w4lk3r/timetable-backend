namespace OOPTypes
{
    internal class Group
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
    }

    internal class Teacher
    {
        public required string Id { get; set; }
        public required string Lastname { get; set; }
        public required string Firstname { get; set; }
    }

    internal class Building
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
    }

    internal class Period
    {
        public required string Number { get; set; }
        public required TimeOnly StartTime { get; set; }
        public required TimeOnly EndTime { get; set; }
    }

    internal class Daydef
    {
        public required string DayCode { get; set; }
        public required string Id { get; set; }
        public required string Name { get; set; }
    }

    internal class Cabinet
    {
        public required Building Building { get; set; }
        public required string CabinetId { get; set; }
        public required string Name { get; set; }
        public required string ShortName { get; set; }
    }

    internal class Subject
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string ShortName { get; set; }
    }

    internal class WeekDef
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string ShortName { get; set; }
        public required string WeekCode { get; set; }
    }

    internal class SubGroup
    {
        public required string Id { get; set; }
        public required string Name { get; set; }

    }

    internal class Lesson
    {
        public required string Id { get; set; }
        public required WeekDef WeeksDef { get; set; }
        public required Teacher Teacher { get; set; }
        public required Subject Subject { get; set; }
        public required Group Group { get; set; }
        public required SubGroup SubGroup { get; set; }
        public required Cabinet Cabinet { get; set; }
        public required Daydef Daysdef { get; set; }
        public required string PeriodsPerCard { get; set; }
        public required string PeriodsPerWeek { get; set; }
    }



    internal class Card
    {
        public string? Id { get; set; }
        public required Daydef Daysdef { get; set; }
        //public int Terms { get; set; }
        public required WeekDef Week { get; set; }
        public required Lesson Lesson { get; set; }
        public required Cabinet Cabinet { get; set; }
        public required Period Period { get; set; }
    }

    internal class Timetable
    {
        public required IList<Group> Groups { get; set; }
        public required IList<SubGroup> SubGroups { get; set; }
        public required IList<string> SubGroupsDestincted { get; set; }
        public required IList<Teacher> Teachers { get; set; }
        public required IList<Building> Buildings { get; set; }
        public required IList<Period> Periods { get; set; }
        public required IList<Daydef> Daydefs { get; set; }
        public required IList<Cabinet> Cabinets { get; set; }
        public required IList<Subject> Subjects { get; set; }
        public required IList<WeekDef> WeekDefs { get; set; }
        public required IList<Lesson> Lessons { get; set; }
        public required IList<Card> Cards { get; set; }
    }
}
