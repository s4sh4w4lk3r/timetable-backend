using System.Xml.Serialization;

#pragma warning disable 8618
namespace AscXmlObjects
{
    [XmlRoot(ElementName = "period")]
    public class Period
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "short")]
        public string Short { get; set; }
        [XmlAttribute(AttributeName = "period")]
        public string _period { get; set; }
        [XmlAttribute(AttributeName = "starttime")]
        public string Starttime { get; set; }
        [XmlAttribute(AttributeName = "endtime")]
        public string Endtime { get; set; }
    }

    [XmlRoot(ElementName = "periods")]
    public class Periods
    {
        [XmlElement(ElementName = "period")]
        public List<Period> Period { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "daysdef")]
    public class Daysdef
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "short")]
        public string Short { get; set; }
        [XmlAttribute(AttributeName = "days")]
        public string Days { get; set; }
    }

    [XmlRoot(ElementName = "daysdefs")]
    public class Daysdefs
    {
        [XmlElement(ElementName = "daysdef")]
        public List<Daysdef> Daysdef { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "weeksdef")]
    public class Weeksdef
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "short")]
        public string Short { get; set; }
        [XmlAttribute(AttributeName = "weeks")]
        public string Weeks { get; set; }
    }

    [XmlRoot(ElementName = "weeksdefs")]
    public class Weeksdefs
    {
        [XmlElement(ElementName = "weeksdef")]
        public List<Weeksdef> Weeksdef { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "termsdef")]
    public class Termsdef
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "short")]
        public string Short { get; set; }
        [XmlAttribute(AttributeName = "terms")]
        public string Terms { get; set; }
    }

    [XmlRoot(ElementName = "termsdefs")]
    public class Termsdefs
    {
        [XmlElement(ElementName = "termsdef")]
        public Termsdef Termsdef { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "subject")]
    public class Subject
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "short")]
        public string Short { get; set; }
        [XmlAttribute(AttributeName = "partner_id")]
        public string Partner_id { get; set; }
    }

    [XmlRoot(ElementName = "subjects")]
    public class Subjects
    {
        [XmlElement(ElementName = "subject")]
        public List<Subject> Subject { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "teacher")]
    public class Teacher
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "firstname")]
        public string Firstname { get; set; }
        [XmlAttribute(AttributeName = "lastname")]
        public string Lastname { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "short")]
        public string Short { get; set; }
        [XmlAttribute(AttributeName = "gender")]
        public string Gender { get; set; }
        [XmlAttribute(AttributeName = "color")]
        public string Color { get; set; }
        [XmlAttribute(AttributeName = "email")]
        public string Email { get; set; }
        [XmlAttribute(AttributeName = "mobile")]
        public string Mobile { get; set; }
        [XmlAttribute(AttributeName = "partner_id")]
        public string Partner_id { get; set; }
    }

    [XmlRoot(ElementName = "teachers")]
    public class Teachers
    {
        [XmlElement(ElementName = "teacher")]
        public List<Teacher> Teacher { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "building")]
    public class Building
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "partner_id")]
        public string Partner_id { get; set; }
    }

    [XmlRoot(ElementName = "buildings")]
    public class Buildings
    {
        [XmlElement(ElementName = "building")]
        public List<Building> Building { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "classroom")]
    public class Classroom
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "short")]
        public string Short { get; set; }
       [XmlAttribute(AttributeName = "capacity")]
        public string Capacity { get; set; }
        [XmlAttribute(AttributeName = "buildingid")]
        public string Buildingid { get; set; }
        [XmlAttribute(AttributeName = "partner_id")]
        public string Partner_id { get; set; }
    }

    [XmlRoot(ElementName = "classrooms")]
    public class Classrooms
    {
        [XmlElement(ElementName = "classroom")]
        public List<Classroom> Classroom { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "grade")]
    public class Grade
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "short")]
        public string Short { get; set; }
        [XmlAttribute(AttributeName = "grade")]
        public string _grade { get; set; }
    }

    [XmlRoot(ElementName = "grades")]
    public class Grades
    {
        [XmlElement(ElementName = "grade")]
        public List<Grade> Grade { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "class")]
    public class Class
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "short")]
        public string Short { get; set; }
        [XmlAttribute(AttributeName = "teacherid")]
        public string Teacherid { get; set; }
        [XmlAttribute(AttributeName = "classroomids")]
        public string Classroomids { get; set; }
        [XmlAttribute(AttributeName = "grade")]
        public string Grade { get; set; }
        [XmlAttribute(AttributeName = "partner_id")]
        public string Partner_id { get; set; }
    }

    [XmlRoot(ElementName = "classes")]
    public class Classes
    {
        [XmlElement(ElementName = "class")]
        public List<Class> Class { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "group")]
    public class Group
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "classid")]
        public string Classid { get; set; }
        [XmlAttribute(AttributeName = "studentids")]
        public string Studentids { get; set; }
        [XmlAttribute(AttributeName = "entireclass")]
        public string Entireclass { get; set; }
        [XmlAttribute(AttributeName = "divisiontag")]
        public string Divisiontag { get; set; }
        [XmlAttribute(AttributeName = "studentcount")]
        public string Studentcount { get; set; }
    }

    [XmlRoot(ElementName = "groups")]
    public class Groups
    {
        [XmlElement(ElementName = "group")]
        public List<Group> Group { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "students")]
    public class Students
    {
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "studentsubjects")]
    public class Studentsubjects
    {
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "lesson")]
    public class Lesson
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "classids")]
        public string Classids { get; set; }
        [XmlAttribute(AttributeName = "subjectid")]
        public string Subjectid { get; set; }
        [XmlAttribute(AttributeName = "periodspercard")]
        public string Periodspercard { get; set; }
        [XmlAttribute(AttributeName = "periodsperweek")]
        public string Periodsperweek { get; set; }
        [XmlAttribute(AttributeName = "teacherids")]
        public string Teacherids { get; set; }
        [XmlAttribute(AttributeName = "classroomids")]
        public string Classroomids { get; set; }
        [XmlAttribute(AttributeName = "groupids")]
        public string Groupids { get; set; }
        [XmlAttribute(AttributeName = "capacity")]
        public string Capacity { get; set; }
        [XmlAttribute(AttributeName = "seminargroup")]
        public string Seminargroup { get; set; }
        [XmlAttribute(AttributeName = "termsdefid")]
        public string Termsdefid { get; set; }
        [XmlAttribute(AttributeName = "weeksdefid")]
        public string Weeksdefid { get; set; }
        [XmlAttribute(AttributeName = "daysdefid")]
        public string Daysdefid { get; set; }
        [XmlAttribute(AttributeName = "partner_id")]
        public string Partner_id { get; set; }
    }

    [XmlRoot(ElementName = "lessons")]
    public class Lessons
    {
        [XmlElement(ElementName = "lesson")]
        public List<Lesson> Lesson { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "card")]
    public class Card
    {
        [XmlAttribute(AttributeName = "lessonid")]
        public string Lessonid { get; set; }
        [XmlAttribute(AttributeName = "classroomids")]
        public string Classroomids { get; set; }
        [XmlAttribute(AttributeName = "period")]
        public string Period { get; set; }
        [XmlAttribute(AttributeName = "weeks")]
        public string Weeks { get; set; }
        [XmlAttribute(AttributeName = "terms")]
        public string Terms { get; set; }
        [XmlAttribute(AttributeName = "days")]
        public string Days { get; set; }
    }

    [XmlRoot(ElementName = "cards")]
    public class Cards
    {
        [XmlElement(ElementName = "card")]
        public List<Card> Card { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "columns")]
        public string Columns { get; set; }
    }

    [XmlRoot(ElementName = "timetable")]
    public class Timetable
    {
        [XmlElement(ElementName = "periods")]
        public Periods Periods { get; set; }
        [XmlElement(ElementName = "daysdefs")]
        public Daysdefs Daysdefs { get; set; }
        [XmlElement(ElementName = "weeksdefs")]
        public Weeksdefs Weeksdefs { get; set; }
        [XmlElement(ElementName = "termsdefs")]
        public Termsdefs Termsdefs { get; set; }
        [XmlElement(ElementName = "subjects")]
        public Subjects Subjects { get; set; }
        [XmlElement(ElementName = "teachers")]
        public Teachers Teachers { get; set; }
        [XmlElement(ElementName = "buildings")]
        public Buildings Buildings { get; set; }
        [XmlElement(ElementName = "classrooms")]
        public Classrooms Classrooms { get; set; }
        [XmlElement(ElementName = "grades")]
        public Grades Grades { get; set; }
        [XmlElement(ElementName = "classes")]
        public Classes Classes { get; set; }
        [XmlElement(ElementName = "groups")]
        public Groups Groups { get; set; }
        [XmlElement(ElementName = "students")]
        public Students Students { get; set; }
        [XmlElement(ElementName = "studentsubjects")]
        public Studentsubjects Studentsubjects { get; set; }
        [XmlElement(ElementName = "lessons")]
        public Lessons Lessons { get; set; }
        [XmlElement(ElementName = "cards")]
        public Cards Cards { get; set; }
        [XmlAttribute(AttributeName = "ascttversion")]
        public string Ascttversion { get; set; }
        [XmlAttribute(AttributeName = "importtype")]
        public string Importtype { get; set; }
        [XmlAttribute(AttributeName = "options")]
        public string Options { get; set; }
        [XmlAttribute(AttributeName = "defaultexport")]
        public string Defaultexport { get; set; }
        [XmlAttribute(AttributeName = "displayname")]
        public string Displayname { get; set; }
        [XmlAttribute(AttributeName = "displaycountries")]
        public string Displaycountries { get; set; }
    }

}