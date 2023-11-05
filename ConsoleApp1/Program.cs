using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables.Cells.CellMembers;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var datesOnly = new List<DateOnly>()
            {
                DateOnly.Parse("06.11.2023"),
                DateOnly.Parse("07.11.2023"),
                DateOnly.Parse("08.11.2023"),
                DateOnly.Parse("09.11.2023"),
                DateOnly.Parse("10.11.2023"),
            };

            var atf = new ActualTimetableFactory(new StableTimetable(1, new Group(1, "4ИП2202"), GetListCells()));
            var tt = atf.Create(datesOnly);

            var a = tt.ActualTimetableCells.Where(e => e.Date == DateOnly.Parse("06.11.2023")).ToList();
        }

        public static List<StableTimetableCell> GetListCells()
        {
            var mdk1101 = new Subject(1, "МДК 11.01 Технология разработки и защиты баз данных");
            var mdk0701 = new Subject(2, "МДК 07.01 Управление и автоматизация баз данных");
            var mdk1102 = new Subject(3, "МДК 11.02 Программные решения для бизнеса");
            var mdk0201 = new Subject(4, "МДК 02.01 Технология разработки программного обеспечения");
            var mdk0702 = new Subject(5, "МДК.07.02 Сертификация информационных систем");
            var angl = new Subject(6, "Иностранный язык в профессиональной деятельности");
            var cert = new Subject(7, "Стандартизация, сертификация и техническое докуметоведение");
            var fizra = new Subject(8, "Физическая культура");
            var pravo = new Subject(9, "Правовое обеспечение профессиональной деятельности");

            var cab213 = new Cabinet(1, "Миллионщикова", "213");
            var cab215 = new Cabinet(2, "Миллионщикова", "215");
            var cab210 = new Cabinet(3, "Миллионщикова", "210");
            var cab324 = new Cabinet(4, "Миллионщикова", "324");
            var cab308 = new Cabinet(5, "Миллионщикова", "308");
            var cab102 = new Cabinet(6, "Миллионщикова", "102");
            var cab301 = new Cabinet(7, "Миллионщикова", "301");
            var cabtren = new Cabinet(8, "Миллионщикова", "Трен");

            var artsybasheva = new Teacher(1, "Арцыбашева", "d", "d");
            var petrenko = new Teacher(2, "Петренко", "das", "da");
            var ahmerova = new Teacher(3, "Ахмерова", "d", "d");
            var prohor = new Teacher(4, "Прохоренкова", "d", "d");
            var barsuk = new Teacher(5, "Барускова", "d", "d");
            var chern = new Teacher(6, "Чернова", "d", "d");
            var ippo = new Teacher(7, "Ипполитова", "d", "d");
            var tanchenko = new Teacher(8, "Танченко", "d", "d");

            var para1 = new LessonTime(1, 1, TimeOnly.Parse("9:00"), TimeOnly.Parse("10:30"));
            var para2 = new LessonTime(1, 2, TimeOnly.Parse("10:50"), TimeOnly.Parse("12:20"));
            var para3 = new LessonTime(1, 3, TimeOnly.Parse("12:40"), TimeOnly.Parse("14:10"));
            var para4 = new LessonTime(1, 4, TimeOnly.Parse("14:30"), TimeOnly.Parse("16:00"));
            var para5 = new LessonTime(1, 5, TimeOnly.Parse("16:10"), TimeOnly.Parse("17:40"));

            var evenList = new List<StableTimetableCell>()
            {
                new StableTimetableCell(1, prohor, mdk1101, cab213, para2, true, DayOfWeek.Monday),
                new StableTimetableCell(2, prohor, mdk0701, cab213, para3, true, DayOfWeek.Monday),
                new StableTimetableCell(3, ahmerova, mdk1102, cab215, para4, true, DayOfWeek.Monday),
                new StableTimetableCell(4, artsybasheva, mdk0201, cab210, para5, true, DayOfWeek.Monday),

                new StableTimetableCell(5, artsybasheva, mdk0201, cab210, para3, true, DayOfWeek.Tuesday),
                new StableTimetableCell(6, petrenko, mdk0702, cab324, para4, true, DayOfWeek.Tuesday),
                new StableTimetableCell(7, tanchenko, angl, cab301, para5, true, DayOfWeek.Tuesday),

                new StableTimetableCell(8, petrenko, mdk0702, cab324, para2, true, DayOfWeek.Wednesday),
                new StableTimetableCell(9, chern, fizra, cabtren, para3, true, DayOfWeek.Wednesday),
                new StableTimetableCell(10, prohor, mdk1101, cab213, para4, true, DayOfWeek.Wednesday),
                new StableTimetableCell(11, barsuk, pravo, cab308, para5, true, DayOfWeek.Wednesday),

                new StableTimetableCell(12, ippo, mdk1101, cab213, para1, true, DayOfWeek.Thursday),
                new StableTimetableCell(13, prohor, mdk1101, cab213, para2, true, DayOfWeek.Thursday),
                new StableTimetableCell(14, prohor, mdk0701, cab213, para3, true, DayOfWeek.Thursday),
                new StableTimetableCell(15, ippo, cert, cab102, para4, true, DayOfWeek.Thursday),

                new StableTimetableCell(16, ippo, cert, cab102, para2, true, DayOfWeek.Friday),
                new StableTimetableCell(17, petrenko, mdk0702, cab324, para3, true, DayOfWeek.Friday),
                new StableTimetableCell(18, artsybasheva, mdk0201, cab210, para4, true, DayOfWeek.Friday),
                new StableTimetableCell(19, barsuk, pravo, cab213, para5, true, DayOfWeek.Friday)
            };

            var oddList = new List<StableTimetableCell>()
            {
                new StableTimetableCell(31, prohor, mdk1101, cab213, para2, false, DayOfWeek.Monday),
                new StableTimetableCell(32, prohor, mdk0701, cab213, para3, false, DayOfWeek.Monday),
                new StableTimetableCell(33, ahmerova, mdk0201, cab215, para4, false, DayOfWeek.Monday),

                new StableTimetableCell(35, artsybasheva, mdk1101, cab210, para3, false, DayOfWeek.Tuesday),
                new StableTimetableCell(36, petrenko, mdk0702, cab324, para4, false, DayOfWeek.Tuesday),
                new StableTimetableCell(37, tanchenko, angl, cab308, para5, false, DayOfWeek.Tuesday),

                new StableTimetableCell(338, petrenko, mdk0702, cab324, para2, false, DayOfWeek.Wednesday),
                new StableTimetableCell(39, chern, fizra, cabtren, para3, false, DayOfWeek.Wednesday),
                new StableTimetableCell(310, prohor, mdk1101, cab213, para4, false, DayOfWeek.Wednesday),
                new StableTimetableCell(311, barsuk, pravo, cab213, para5, false, DayOfWeek.Wednesday),

                new StableTimetableCell(312, ippo, mdk1101, cab102, para1, false, DayOfWeek.Thursday),
                new StableTimetableCell(313, prohor, mdk1101, cab213, para2, false, DayOfWeek.Thursday),
                new StableTimetableCell(314, prohor, mdk1101, cab213, para3, false, DayOfWeek.Thursday),
                new StableTimetableCell(315, ippo, mdk1101, cab102, para4, false, DayOfWeek.Thursday),

                new StableTimetableCell(317, petrenko, mdk0702, cab324, para3, false, DayOfWeek.Friday),
                new StableTimetableCell(318, artsybasheva, mdk1101, cab210, para4, false, DayOfWeek.Friday),
                new StableTimetableCell(319, barsuk, pravo, cab213, para5, false, DayOfWeek.Friday)
            };

            oddList.AddRange(evenList);
            return oddList;
        }
    }

    /*public class ActualTimetableFactory
    {
        private readonly List<StableTimetableCell> _stableTimetableCells;
        private readonly List<ActualTimetableCell> _actualTimetableCells;
        private readonly Group _group;

        public ActualTimetableFactory(StableTimetable stableTimetable)
        {
            _group = stableTimetable.Group!;
            _stableTimetableCells = stableTimetable.StableTimetableCells!.ToList();
            _actualTimetableCells = new();
        }

        public ActualTimetable Create()
        {
            CreateForDayOfWeek(DayOfWeek.Monday);
            CreateForDayOfWeek(DayOfWeek.Tuesday);
            CreateForDayOfWeek(DayOfWeek.Wednesday);
            CreateForDayOfWeek(DayOfWeek.Thursday);
            CreateForDayOfWeek(DayOfWeek.Friday);

#warning подумать над этой строчкой.
            return new ActualTimetable(5, _group, _actualTimetableCells, ISOWeek.GetWeekOfYear(DateTime.Now));
        }

        private void CreateForDayOfWeek(DayOfWeek dayOfWeek)
        {
            var date = DateOnly.FromDateTime(DateTime.Now).GetDateOfNextDayOfWeek(dayOfWeek);
            bool isEven = date.IsWeekEven();

#warning надо как следует продебажить все по всем датам.
            isEven = true;

            Random random = new();

            var stableListForThisDay = _stableTimetableCells.Where(e => e.DayOfWeek == dayOfWeek && e.IsWeekEven == isEven).ToList();

            foreach (var item in stableListForThisDay)
            {
                var a = item.CastToActualCell(date);
                _actualTimetableCells.Add(a);

                *//*if (item.Teacher.Surname == "Танченко")
                {
                    _actualTimetableCells.Add(a);
                }*//*
            }
        }
    }*/
}