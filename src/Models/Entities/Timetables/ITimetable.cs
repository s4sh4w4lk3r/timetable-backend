namespace Models.Entities.Timetables
{
    /// <summary>
    /// Интерфейс расписания.
    /// </summary>
    public interface ITimetable
    {
        public int TimetableId { get; }
        public Group? Group { get; }
        public int GroupId { get; }


        /// <summary>
        /// Должен проверять наличие ячеек-дубликатов, которые ссылаются на одно и то же время занятий.
        /// </summary>
        /// <returns>True - если нет дубликатов, в противном случае False.</returns>
        public bool CheckNoDuplicates();
    }
}