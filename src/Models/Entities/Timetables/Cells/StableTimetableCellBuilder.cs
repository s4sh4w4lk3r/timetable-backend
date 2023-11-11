using Models.Entities.Timetables.Cells.CellMembers;

namespace Models.Entities.Timetables.Cells
{
    public class StableTimetableCellBuilder
    {
        private readonly StableTimetableCell _stableTimetableCell = new();

        public StableTimetableCellBuilder(int stableTimetableCellId)
        {
            _stableTimetableCell.TimetableCellId = stableTimetableCellId;
        }

        public StableTimetableCellBuilder AddSubject(Subject subject)
        {
            subject.ThrowIfNull();
            _stableTimetableCell.Subject = subject;
            return this;
        }

        public StableTimetableCellBuilder AddCabinet(Cabinet cabinet)
        {
            cabinet.ThrowIfNull();
            _stableTimetableCell.Cabinet = cabinet; 
            return this;
        }

        public StableTimetableCellBuilder AddTeacher(TeacherCM teacher)
        {
            teacher.ThrowIfNull();
            _stableTimetableCell.Teacher = teacher;
            return this;
        }

        public StableTimetableCellBuilder AddLessonTime(LessonTime lessonTime)
        {
            lessonTime.ThrowIfNull();
            _stableTimetableCell.LessonTime = lessonTime;
            return this;
        }

        public StableTimetableCellBuilder AddIsWeekEven(bool isWeekEven)
        {
            _stableTimetableCell.IsWeekEven = isWeekEven;
            return this;
        }

        public StableTimetableCellBuilder AddDayOfWeek(DayOfWeek dayOfWeek)
        {
            _stableTimetableCell.DayOfWeek = dayOfWeek;
            return this;
        }

        public StableTimetableCell Build()
        {
            _stableTimetableCell.Subject.ThrowIfNull();
            _stableTimetableCell.Cabinet.ThrowIfNull();
            _stableTimetableCell.LessonTime.ThrowIfNull();
            _stableTimetableCell.Teacher.ThrowIfNull();

            return _stableTimetableCell;
        }
    }
}
