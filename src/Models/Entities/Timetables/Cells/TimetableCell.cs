using Models.Entities.Timetables.Cells.CellMembers;

namespace Models.Entities.Timetables.Cells
{
    /// <summary>
    /// Ячейка расписания
    /// </summary>
    public class TimetableCell
    {
        public int TimeTableCellId { get; init; }

        public LessonTime? LessonTime { get; set; }
        public required int LessonTimeId { get; set; }

        public Cabinet? Cabinet { get; set; }
        public required int CabinetId { get; set; }

        public Teacher? Teacher { get; set; }
        public required int TeacherId { get; set; }

        public Subject? Subject { get; set; }
        public required int SubjectId { get; set; }

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
