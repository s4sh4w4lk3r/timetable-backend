using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using System.Net.NetworkInformation;

namespace Repository
{
    internal class TestDataSet
    {
        private IList<TimetableCell> GetTimetableCells()
        {
            var teachers = GetTeachers();
            var subjects = GetSubjects();
            var lessonTimes = GetLessonsTimes();
            var groups = GetGroups();
            var cabinets = GetCabinets();


        }
        private IList<Teacher> GetTeachers()
        { 
            var list = new List<Teacher>()
            {
                new Teacher(1, "Гаджиев", "Гаджи", "Дагирович"),
                new Teacher(2, "Бекасов", "Роман", "Анатольевич"),
                new Teacher(3, "Ахмерова", "Наталья", "Дмитриевна"),
                new Teacher(4, "Киселева", "Анастасия", "Владимировна"),
                new Teacher(5, "Алямкина", "Елена", "Семеновна"),
                new Teacher(6, "Чернова", "Людмила", "Львовна")
            };

            return list;
        }

        private IList<Subject> GetSubjects()
        {
            var list = new List<Subject>()
            {
                new Subject(1, "Математика"),
                new Subject(2, "Физкультура"),
                new Subject(3, "История"),
                new Subject(4, "Информационная безопасность"),
                new Subject(5, "МДК 01.02"),
            };

            return list;
        }

        private IList<LessonTime> GetLessonsTimes()
        {
            var list = new List<LessonTime>()
            {
                new LessonTime(1, 1, false, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(2, 2, false, new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(3, 3, false, new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(4, 4, false, new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(5, 5, false, new TimeOnly(16, 10), new TimeOnly(17,40)),

                new LessonTime(11, 1, true, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(12, 2, true, new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(13, 3, true, new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(14, 4, true, new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(15, 5, true, new TimeOnly(16, 10), new TimeOnly(17,40))
            };
            return list;
        }

        private IList<Group> GetGroups()
        {
            var list = new List<Group>()
            {
                new Group(1, "4ИП-2-20"),
                new Group(2, "2ГД-2-11-22"),
                new Group(3, "2СА-11-22"),
                new Group(4, "3СА-1-21"),
                new Group(5, "4ИП-1-20"),
                new Group(6, "1ИП-1-11-23"),
                new Group(7, "1ГД-2-23"),
                new Group(8, "1Р-2-23")
            };
            return list;
        }

        private IList<Cabinet> GetCabinets()
        {
            var list = new List<Cabinet>()
            {
                new Cabinet(1, "Миллионщикова", "210"),
                new Cabinet(2, "Миллионщикова", "231"),
                new Cabinet(3, "Миллионщикова", "316"),
                new Cabinet(4, "Миллионщикова", "419"),
                new Cabinet(5, "Миллионщикова", "308"),
                new Cabinet(6, "Миллионщикова", "116"),
                new Cabinet(7, "Миллионщикова", "311"),
            };
            return list;
        }
    }
}
