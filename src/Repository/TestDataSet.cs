using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;

namespace Repository
{
    public class TestDataSet
    {
        public Timetable GetTimetable()
        {
            var group = GetGroups().First();
            var ttc = GetTimetableCells();
            var tt = new Timetable(1, group, ttc) { GroupId = group.GroupId };

            return tt;
        }

        private IList<TimetableCell> GetTimetableCells()
        {
            var teachers = GetTeachers();
            var subjects = GetSubjects();
            var lessonTimes = new Stack<LessonTime>(GetLessonsTimes());
            var cabinets = GetCabinets();
            var rand = new Random();

            var list = new List<TimetableCell>();

            while (lessonTimes.TryPeek(out _))
            {
                var teacher = teachers.OrderBy(x => rand.Next()).Take(1).First();
                var cabinet = cabinets.OrderBy(x => rand.Next()).Take(1).First();
                var subject = subjects.OrderBy(x => rand.Next()).Take(1).First();
                var lt = lessonTimes.Pop();

                var tc = new TimetableCell()
                {
                    Cabinet = cabinet,
                    CabinetId = cabinet.CabinetId,
                    LessonTime = lt,
                    LessonTimeId = lt.LessonTimeId,
                    Subject = subject,
                    SubjectId = subject.SubjectId,
                    Teacher = teacher,
                    TeacherId = teacher.TeacherId,
                    TimeTableCellId = rand.Next()
                };

                list.Add(tc);
            }
            return list;

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
            var mondayNotEven = new List<LessonTime>()
            {
                new LessonTime(110, 1, false, DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(120, 2, false, DayOfWeek.Monday,new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(130, 3, false, DayOfWeek.Monday,new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(140, 4, false, DayOfWeek.Monday,new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(150, 5, false, DayOfWeek.Monday,new TimeOnly(16, 10), new TimeOnly(17,40)),
            };


            var mondayEven = new List<LessonTime>()
            {
                new LessonTime(111, 1, true,DayOfWeek.Monday, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(121, 2, true,DayOfWeek.Monday, new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(131, 3, true, DayOfWeek.Monday,new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(141, 4, true, DayOfWeek.Monday,new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(151, 5, true, DayOfWeek.Monday,new TimeOnly(16, 10), new TimeOnly(17,40))
            };

            var tuesdayNotEven = new List<LessonTime>()
            {
                new LessonTime(210, 1, false, DayOfWeek.Tuesday, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(220, 2, false,DayOfWeek.Tuesday ,new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(230, 3, false, DayOfWeek.Tuesday,new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(240, 4, false,DayOfWeek.Tuesday,new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(250, 5, false, DayOfWeek.Tuesday,new TimeOnly(16, 10), new TimeOnly(17,40)),
            };


            var tuesdayEven = new List<LessonTime>()
            {
                new LessonTime(211, 1, true,DayOfWeek.Tuesday, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(221, 2, true,DayOfWeek.Tuesday, new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(231, 3, true, DayOfWeek.Tuesday,new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(241, 4, true, DayOfWeek.Tuesday,new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(251, 5, true, DayOfWeek.Tuesday,new TimeOnly(16, 10), new TimeOnly(17,40))
            };


            var wednesdayNotEven = new List<LessonTime>()
            {
                new LessonTime(310, 1, false, DayOfWeek.Wednesday, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(320, 2, false,DayOfWeek.Wednesday ,new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(330, 3, false, DayOfWeek.Wednesday,new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(340, 4, false,DayOfWeek.Wednesday,new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(350, 5, false, DayOfWeek.Wednesday,new TimeOnly(16, 10), new TimeOnly(17,40)),
            };


            var wednesdayEven = new List<LessonTime>()
            {
                new LessonTime(311, 1, true,DayOfWeek.Wednesday, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(321, 2, true,DayOfWeek.Wednesday, new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(331, 3, true, DayOfWeek.Wednesday,new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(341, 4, true, DayOfWeek.Wednesday,new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(351, 5, true, DayOfWeek.Wednesday,new TimeOnly(16, 10), new TimeOnly(17,40))
            };

            var thursdayNotEven = new List<LessonTime>()
            {
                new LessonTime(410, 1, false, DayOfWeek.Thursday, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(420, 2, false,DayOfWeek.Thursday,new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(430, 3, false, DayOfWeek.Thursday,new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(440, 4, false,DayOfWeek.Thursday,new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(450, 5, false, DayOfWeek.Thursday,new TimeOnly(16, 10), new TimeOnly(17,40)),
            };


            var thursdayEven = new List<LessonTime>()
            {
                new LessonTime(411, 1, true,DayOfWeek.Thursday, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(421, 2, true,DayOfWeek.Thursday, new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(431, 3, true, DayOfWeek.Thursday,new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(441, 4, true, DayOfWeek.Thursday,new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(451, 5, true, DayOfWeek.Thursday,new TimeOnly(16, 10), new TimeOnly(17,40))
            };

            var fridayNotEven = new List<LessonTime>()
            {
                new LessonTime(510, 1, false, DayOfWeek.Friday, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(520, 2, false,DayOfWeek.Friday,new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(530, 3, false, DayOfWeek.Friday,new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(540, 4, false,DayOfWeek.Friday,new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(550, 5, false, DayOfWeek.Friday,new TimeOnly(16, 10), new TimeOnly(17,40)),
            };


            var fridayEven = new List<LessonTime>()
            {
                new LessonTime(511, 1, true,DayOfWeek.Friday, new TimeOnly(9, 0), new TimeOnly(10,30)),
                new LessonTime(521, 2, true,DayOfWeek.Friday, new TimeOnly(10, 50), new TimeOnly(12,20)),
                new LessonTime(531, 3, true, DayOfWeek.Friday,new TimeOnly(12, 40), new TimeOnly(14,10)),
                new LessonTime(541, 4, true, DayOfWeek.Friday,new TimeOnly(14,30), new TimeOnly(16, 00)),
                new LessonTime(551, 5, true, DayOfWeek.Friday,new TimeOnly(16, 10), new TimeOnly(17,40))
            };

            var list = new List<LessonTime>();
            list.AddRange(mondayEven);
            list.AddRange(mondayNotEven);
            list.AddRange(tuesdayEven);
            list.AddRange(tuesdayNotEven);
            list.AddRange(wednesdayEven);
            list.AddRange(wednesdayNotEven);
            list.AddRange(thursdayEven);
            list.AddRange(thursdayNotEven);
            list.AddRange(fridayEven);
            list.AddRange(fridayNotEven);

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
