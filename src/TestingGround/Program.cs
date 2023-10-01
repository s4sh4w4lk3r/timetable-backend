using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using System.Text.Json;

namespace TestingGround;

internal class Program
{

    static void Main(string[] args)
    {
        Random rand = new Random();

        var group = new Group(rand.Next(), "2ИП-2-20");

        var teacher1 = new Teacher(rand.Next(), "Дьяков", "Владимир", "Юрьевич");
        var teacher2 = new Teacher(rand.Next(), "Бурукин", "Миша", "Васильевич");
        var teacher3 = new Teacher(rand.Next(), "Бекасов", "Роман", "Анатольевич");

        var subject1 = new Subject(rand.Next(), "Английский");
        var subject2 = new Subject(rand.Next(), "Матеша");
        var subject3 = new Subject(rand.Next(), "Русский");

        var cabinet1 = new Cabinet(rand.Next(), "Москва", $"{rand.Next()}");
        var cabinet2 = new Cabinet(rand.Next(), "Москва", $"{rand.Next()}");
        var cabinet3 = new Cabinet(rand.Next(), "Москва", $"{rand.Next()}");

        var lessontime0 = new LessonTime(rand.Next(), 0, true, new TimeOnly(8, 10), new TimeOnly(8, 55));
        var lessontime1 = new LessonTime(rand.Next(), 1, true, new TimeOnly(9, 00), new TimeOnly(10, 30));
        var lessontime2 = new LessonTime(rand.Next(),2, true, new TimeOnly(10, 50), new TimeOnly(12, 20));
        var lessontime3 = new LessonTime(rand.Next(), 3, true, new TimeOnly(12, 40), new TimeOnly(14, 10));
        var lessontime4 = new LessonTime(rand.Next(), 4, true, new TimeOnly(14, 30), new TimeOnly(16, 00));
        var lessontime5 = new LessonTime(rand.Next(), 5, true, new TimeOnly(16, 10), new TimeOnly(17, 40));
        var lessontime0f = new LessonTime(rand.Next(), 0, false, new TimeOnly(8, 10), new TimeOnly(8, 55));
        var lessontime1f = new LessonTime(rand.Next(), 1, false, new TimeOnly(9, 00), new TimeOnly(10, 30));
        var lessontime2f = new LessonTime(rand.Next(), 2, false, new TimeOnly(10, 50), new TimeOnly(12, 20));
        var lessontime3f = new LessonTime(rand.Next(), 3, false, new TimeOnly(12, 40), new TimeOnly(14, 10));
        var lessontime4f = new LessonTime(rand.Next(), 4, false, new TimeOnly(14, 30), new TimeOnly(16, 00));
        var lessontime5f = new LessonTime(rand.Next(), 5, false, new TimeOnly(16, 10), new TimeOnly(17, 40));


        var tc0 = new TimetableCell(rand.Next(), lessontime0, cabinet1, teacher3, subject1);
        var tc1 = new TimetableCell(rand.Next(), lessontime1, cabinet1, teacher3, subject1);
        var tc2 = new TimetableCell(rand.Next(), lessontime2, cabinet3, teacher2, subject3);
        var tc3 = new TimetableCell(rand.Next(), lessontime3, cabinet2, teacher3, subject3);
        var tc4 = new TimetableCell(rand.Next(), lessontime4, cabinet1, teacher2, subject2);
        var tc5 = new TimetableCell(rand.Next(), lessontime5, cabinet3, teacher1, subject3);

        var tc7 = new TimetableCell(rand.Next(), lessontime0f, cabinet1, teacher3, subject1);
        var tc6 = new TimetableCell(rand.Next(), lessontime1f, cabinet3, teacher1, subject3);
        var tc8 = new TimetableCell(rand.Next(), lessontime2f, cabinet3, teacher2, subject3);
        var tc9 = new TimetableCell(rand.Next(), lessontime3f, cabinet2, teacher3, subject3);
        var tc10 = new TimetableCell(rand.Next(), lessontime4f, cabinet1, teacher2, subject2);
        var tc11 = new TimetableCell(rand.Next(), lessontime5f, cabinet3, teacher1, subject3);
        var tc12 = new TimetableCell(rand.Next(), lessontime5f, cabinet3, teacher1, subject3);
        tc12.ReplacingTimeTableCell = tc9;
        var tc13 = new TimetableCell(rand.Next(), lessontime0f, cabinet3, teacher1, subject3);



        var evenCells = new List<TimetableCell>()
        {
            tc0, tc1, tc2, tc3, tc4, tc5, tc6, tc7, tc8, tc9, tc10, tc11, tc12
        };

        var tt = new Timetable(group, evenCells);

        var op = new JsonSerializerOptions() { WriteIndented = true };
        string json = JsonSerializer.Serialize(tt, op);
        Console.WriteLine(json);
    }
}