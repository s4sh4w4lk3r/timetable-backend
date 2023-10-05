using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Users;
using Repository.Implementations.EFCore;
using Repository.Interfaces;

namespace Repository.Implementations.MySql
{
    public class TestDataSet
    {
        public static void TestInsert(MySqlDbContext context)
        {
            Random rand = new();

            var groups = new List<Group>()
        {
            new Group(7, "2ИП-2-20"),
            new Group(1, "4ИП-2-21"),
            new Group(2, "5ИП-1-23"),
            new Group(3, "2ИП-2-24"),
            new Group(4, "1ИП-2-25"),
            new Group(5, "2ИП-2-22"),
            new Group(6, "4ИП-2-19"),
        };


            var teachers = new List<Teacher>()
        {
            new Teacher(5, "Дьяков", "Владимир", "Юрьевич"),
            new Teacher(1, "Бурукин", "Миша", "Васильевич"),
            new Teacher(2, "Бекасов", "Роман", "Анатольевич"),
            new Teacher(3, "Трегубов", "Роман", "Анатольевич"),
            new Teacher(4, "Пенин", "Роман", "Анатольевич"),
        };

            var subjects = new List<Subject>()
        {
            new Subject(6, "Английский"),
            new Subject(1, "Матеша"),
            new Subject(2, "Немекйий"),
            new Subject(3, "Биология"),
            new Subject(4, "Питончик"),
            new Subject(5, "Плюсик")
        };

            var cabinets = new List<Cabinet>()
        {
            new Cabinet(7, "Москва", $"{rand.Next()}"),
            new Cabinet(1, "Москва", $"{rand.Next()}"),
            new Cabinet(2, "Москва", $"{rand.Next()}"),
            new Cabinet(3, "Москва", $"{1234}"),
            new Cabinet(4, "Москва", $"{rand.Next()}"),
            new Cabinet(5, "Москва", $"{rand.Next()}"),
            new Cabinet(6, "Москва", $"{rand.Next()}")
        };



            var ltList = new List<LessonTime>()
        {
            new LessonTime(1, 0, true, new TimeOnly(8, 10), new TimeOnly(8, 55)),
            new LessonTime(2, 1, true, new TimeOnly(9, 00), new TimeOnly(10, 30)),
            new LessonTime(3, 2, true, new TimeOnly(10, 50), new TimeOnly(12, 20)),
            new LessonTime(4, 3, true, new TimeOnly(12, 40), new TimeOnly(14, 10)),
            new LessonTime(5, 4, true, new TimeOnly(14, 30), new TimeOnly(16, 00)),
            new LessonTime(6, 5, true, new TimeOnly(16, 10), new TimeOnly(17, 40)),
            new LessonTime(7, 0, false, new TimeOnly(8, 10), new TimeOnly(8, 55)),
            new LessonTime(8, 1, false, new TimeOnly(9, 00), new TimeOnly(10, 30)),
            new LessonTime(9, 2, false, new TimeOnly(10, 50), new TimeOnly(12, 20)),
            new LessonTime(10, 3, false, new TimeOnly(12, 40), new TimeOnly(14, 10)),
            new LessonTime(11, 4, false, new TimeOnly(14, 30), new TimeOnly(16, 00)),
            new LessonTime(12, 5, false, new TimeOnly(16, 10), new TimeOnly(17, 40)),
        };

            var users = new List<Administrator>()
        {
            new Administrator(1, "waxman98@gaumontleblanc.com", "fhsdhfi"),
            new Administrator(2, "wa1man98@gaumontleblanc.com", "fhsdhfi"),
            new Administrator(3, "wax4an98@gaumontleblanc.com", "fhsdhfi"),
            new Administrator(4, "waxfan98@gaumontleblanc.com", "fhsdhfi"),
            new Administrator(5, "waxvan98@gaumontleblanc.com", "fhsdhfi"),
            new Administrator(6, "waxfgn98@gaumontleblanc.com", "fhsdhfi")
        };

            context.LessonTimes.AddRange(ltList);
            context.Groups.AddRange(groups);
            context.Teachers.AddRange(teachers);
            context.Subjects.AddRange(subjects);
            context.Cabinets.AddRange(cabinets);
            context.Administrators.AddRange(users);
            context.SaveChanges();


            var lt1 = context.LessonTimes.Find(1);
            var cb1 = context.Cabinets.Find(1);
            var tch = context.Teachers.Find(1);
            var sbj = context.Subjects.Find(1);

            var cells = new List<TimetableCell>()
            {
                new TimetableCell() {Cabinet = cb1, LessonTime = lt1, Subject = sbj, Teacher = tch}
               
            };

            
            context.TimetableCells.AddRange(cells);

            context.SaveChanges();


        }
        public static void TestInsert1(MySqlDbContext context)
        {
            ITeacherRepository teacherRepository = new TeacherRepository(context);
            IAdministratorRepository administratorRepository = new AdministratorRepository(context);
            IGroupRepository groupRepository = new GroupRepository(context);
            ILessonTimeRepository lessonTimeRepository = new LessonTimeRepository(context);
            ISubjectRepository subjectRepository = new SubjectRepository(context);
            ICabinetRepository cabinetRepository = new CabinetRepository(context);
            ITimetableCellRepository timetableCellRepository = new TimetableCellRepository(context);
            ITimetableRepository timetableRepository = new TimetableRepository(context);
        }
    }
}
