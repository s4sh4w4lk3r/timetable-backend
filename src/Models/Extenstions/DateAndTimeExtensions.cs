using System.Globalization;

namespace Models.Extenstions
{
    public static class DateAndTimeExtensions
    {
        /// <summary>
        /// Определяет, является ли неделя чётной по ISO 8601 (пункт 2.2.10).
        /// </summary>
        /// <param name="dateOnly"></param>
        /// <returns>True если неделя четная, иначе False.</returns>
        public static bool IsWeekEven(this DateOnly dateOnly)
        {
            DateTime dateTime = dateOnly.ToDateTime(TimeOnly.MinValue);
            int week = ISOWeek.GetWeekOfYear(dateTime);
            return week % 2 == 0;
        }

        /// <summary>
        /// Определяет, является ли неделя чётной по ISO 8601 (пункт 2.2.10).
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>True если неделя четная, иначе False.</returns>
        public static bool IsWeekEven(this DateTime dateTime)
        {
            int week = ISOWeek.GetWeekOfYear(dateTime);
            return week % 2 == 0;
        }

        /*/// <summary>
        /// Получить дату ближайшего дня недели, переданного в параметры метода.
        /// </summary>
        /// <param name="dateOnly"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static DateOnly GetDateOfNextDayOfWeek(this DateOnly dateOnly, DayOfWeek dayOfWeek)
        {
            for (int i = 0; i < 7; i++)
            {
                if (dateOnly.DayOfWeek == dayOfWeek)
                {
                    return dateOnly;
                }
                dateOnly = dateOnly.AddDays(1);
            }
            throw new ArgumentException("Получен несуществующий день недели.");
        }*/
    }
}
