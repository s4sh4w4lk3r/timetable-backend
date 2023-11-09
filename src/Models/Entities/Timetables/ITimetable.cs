namespace Models.Entities.Timetables
{
    /// <summary>
    /// Абстрактный класс расписания.
    /// </summary>
    public interface ITimetable
    {
        public int TimetableId { get; set; }
        public Group? Group { get; init; }


        /// <summary>
        /// Должен проверять наличие ячеек-дубликатов, которые ссылаются на одно и то же время занятий.
        /// </summary>
        /// <returns></returns>
        public bool CheckNoDuplicates();
    }
}