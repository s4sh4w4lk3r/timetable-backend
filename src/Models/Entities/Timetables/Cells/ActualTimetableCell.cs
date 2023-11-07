using Models.Entities.Timetables.Cells.CellMembers;
using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables.Cells
{
    public class ActualTimetableCell : TimetableCell
    {
        public required DateOnly Date { get; init; }
        public required bool IsModified { get; set; }
        public required bool IsCanceled { get; set; }
        public required bool IsMoved { get; set; }

        public ActualTimetableCell() : base() { }

        [SetsRequiredMembers]
        public ActualTimetableCell(int actualTimetableCellId, Teacher teacher, Subject subject, Cabinet cabinet, LessonTime lessonTime, DateOnly date, bool isModified = false, bool isCanceled = false, bool isMoved = false)
            : base(actualTimetableCellId, teacher, subject, cabinet, lessonTime)
        {
            Date = date;
            IsModified = isModified;
            IsCanceled = isCanceled;
            IsMoved = isMoved;
        }
    }
}
