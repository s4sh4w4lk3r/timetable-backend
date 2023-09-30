using Core.LessonTimes;

namespace Core.Timetables.Cells
{
    /// <summary>
    /// Ячейка расписания
    /// </summary>
    public class TimetableCell
    {
        public int TimeTableCellPK { get; init; }
        public LessonTime? LessonTime { get; init; }
        public Cabinet? Cabinet { get; init; }
        public Teacher? Teacher { get; init; }
        public Subject? Subject { get; init; }
        public bool IsReplaced { get; private set; }

        private TimetableCell? _replacingTimeTableCell;
        /// <summary>
        /// Ссылка на пару, которую заменяют
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

        public TimetableCell(int timeTableCellPK, LessonTime lessonTime, Cabinet cabinet, Teacher teacher, Subject subject)
        {
            subject.ThrowIfNull();
            cabinet.ThrowIfNull();
            teacher.ThrowIfNull();
            lessonTime.ThrowIfNull();

            TimeTableCellPK = timeTableCellPK;
            Subject = subject;
            Cabinet = cabinet;
            Teacher = teacher;
            LessonTime = lessonTime;
        }
    }
}
