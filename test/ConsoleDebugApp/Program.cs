using AscConverter;

namespace ConsoleDebugApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var a = new AscConverter.Converter(@"C:\Users\sanchous\Desktop\projects\timetable-backend\данные\база.xml").Convert();
            var my = a.Where(e => e.Group.Name == "4ИП-2-20").ToList();
        }
    }
}