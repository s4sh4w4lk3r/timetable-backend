using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables
{
    /// <summary>
    /// Абстрактный класс расписания.
    /// </summary>
    public abstract class Timetable
    {
        public int TimetableId { get; init; }
        public required Group? Group { get; init; }

        protected Timetable() { }


        /// <summary>
        /// Используется наследниками для создания объекта класса. Конструктор при создании проверяет 
        /// наличие дубликатов ячеек на одно и то же время в расписании реализацией абстрактного метода CheckNoDuplicates(), который при их наличии выкидывает исключение.
        /// </summary>
        /// <param name="timetableId"></param>
        /// <param name="group"></param>
        /// <exception cref="InvalidOperationException"></exception>
        [SetsRequiredMembers]
        protected Timetable(int timetableId, Group group)
        {
            group.ThrowIfNull();

            TimetableId = timetableId;
            Group = group;
        }

        /// <summary>
        /// Должен проверять наличие ячеек-дубликатов, которые ссылаются на одно и то же время занятий.
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckNoDuplicates();
    }
}