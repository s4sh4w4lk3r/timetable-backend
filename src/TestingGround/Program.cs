using Core;
using Core.LessonTimes;
using Core.Timetables;
using Core.Timetables.Cells;
using System.Text.Json;
using System.Text.Json.Serialization;
using Throw;

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

        var lessontime0 = new LessonTime(0, new TimeOnly(8, 10), new TimeOnly(8, 55));
        var lessontime1 = new LessonTime(1, new TimeOnly(9, 00), new TimeOnly(10, 30));
        var lessontime2 = new LessonTime(2, new TimeOnly(10, 50), new TimeOnly(12, 20));
        var lessontime3 = new LessonTime(3, new TimeOnly(12, 40), new TimeOnly(14, 10));
        var lessontime4 = new LessonTime(4, new TimeOnly(14, 30), new TimeOnly(16, 00));
        var lessontime5 = new LessonTime(5, new TimeOnly(16, 10), new TimeOnly(17, 40));

        var tc1 = new TimetableCell(lessontime0, cabinet1, teacher3, subject1);
        var tc2 = new TimetableCell(lessontime1, cabinet3, teacher2, subject3);
        var tc3 = new TimetableCell(lessontime4, cabinet2, teacher3, subject3);
        var tc4 = new TimetableCell(lessontime2, cabinet1, teacher2, subject2);
        var tc5 = new TimetableCell(lessontime5, cabinet3, teacher1, subject3);


        var oddCells = new List<TimetableCell>()
        {
            tc1, tc2, tc3
        };

        var evenCells = new List<TimetableCell>()
        {
            tc1, tc2, tc3, tc4, tc5
        };

        var tt = new Timetable(group, evenCells, oddCells);

        var json = JsonSerializer.Serialize(tt, new JsonSerializerOptions { WriteIndented = true }); ;
        Console.WriteLine(json);
    }
}