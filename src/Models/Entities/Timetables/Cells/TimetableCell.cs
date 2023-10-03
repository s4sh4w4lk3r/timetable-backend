namespace Models.Entities.Timetables.Cells
{
    /// <summary>
    /// Ячейка расписания
    /// </summary>
    public class TimetableCell
    {
        public int TimeTableCellId { get; init; }
        public required LessonTime? LessonTime { get; init; }
        public required Cabinet? Cabinet { get; init; }
        public required Teacher? Teacher { get; init; }
        public required Subject? Subject { get; init; }
        public bool IsReplaced { get; private set; }

        private TimetableCell? _replacingTimeTableCell;
        /// <summary>
        /// Ссылка на пару, которую заменяют. Когда используется сеттер, то свойство IsReplaced становится true.
        /// </summary>
        public TimetableCell? ReplacingTimeTableCell
        {
            get => _replacingTimeTableCell;
            set
            {
                value.ThrowIfNull();
                _replacingTimeTableCell = value;
                IsReplaced = true;
            }
        }

        /// <summary>
        /// Убирает замену из свойства ReplacingTimeTableCell и IsReplaced становится обратно false.
        /// </summary>
        public void CancelReplace()
        {
            IsReplaced = false;
            ReplacingTimeTableCell = null;
        }
        public TimetableCell() { }
    }
}
