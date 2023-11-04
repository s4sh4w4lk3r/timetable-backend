using Models.Entities.Timetables.Cells.CellMembers;
using System.Diagnostics.CodeAnalysis;

namespace Models.Entities.Timetables.Cells
{
    public class ActualTimetableCell : TimetableCell
    {
        public required DateOnly Date { get; init; }
        public required bool IsReplaced { get; set; }
        public required bool IsCanceled { get; set; }

        private ActualTimetableCell() : base() { }

        [SetsRequiredMembers]
        public ActualTimetableCell(int actualTimetableCellId, Teacher teacher, Subject subject, Cabinet cabinet, LessonTime lessonTime, DateOnly date, bool isReplaced = false, bool isCanceled = false)
            : base(actualTimetableCellId, teacher, subject, cabinet, lessonTime)
        {
            Date = date;
            IsReplaced = isReplaced;
            IsCanceled = isCanceled;
        }
    }
}
