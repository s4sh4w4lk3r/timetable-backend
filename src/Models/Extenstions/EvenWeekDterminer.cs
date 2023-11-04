using System.Globalization;

namespace Models.Extenstions
{
    public static class EvenWeekDterminer
    {
        /// <summary>
        /// Определяет, является ли неделя чётной по ISO 8601 (пункт 2.2.10).
        /// </summary>
        /// <param name="dateOnly"></param>
        /// <returns>True если неделя четная, иначе False.</returns>
        public static bool IsWeekEven(this DateOnly dateOnly)
        {
            DateTime dateTime = dateOnly.ToDateTime(TimeOnly.MinValue);
            Calendar cal = new CultureInfo("ru-RU").Calendar;
            int week = cal.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return week % 2 == 0;
        }

        /// <summary>
        /// Определяет, является ли неделя чётной по ISO 8601 (пункт 2.2.10).
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>True если неделя четная, иначе False.</returns>
        public static bool IsWeekEven(this DateTime dateTime)
        {
            Calendar cal = new CultureInfo("ru-RU").Calendar;
            int week = cal.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return week % 2 == 0;
        }
#warning написать юнит тесты к этому
    }
}
