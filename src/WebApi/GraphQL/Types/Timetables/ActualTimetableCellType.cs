using Models.Entities.Timetables.Cells;

namespace WebApi.GraphQL.Types.Timetables
{
    public class ActualTimetableCellType : ObjectType<ActualTimetableCell>
    {
        protected override void Configure(IObjectTypeDescriptor<ActualTimetableCell> descriptor)
        {
            descriptor.Field(e => e.TimetableCellId);

            descriptor.Field(e => e.IsCanceled);
            descriptor.Field(e => e.IsModified);
            descriptor.Field(e => e.IsMoved);

            descriptor.Field(e => e.Cabinet);
            descriptor.Field(e => e.CabinetId);

            descriptor.Field(e => e.Teacher);
            descriptor.Field(e => e.TeacherId);

            descriptor.Field(e => e.Subject);
            descriptor.Field(e => e.SubjectId);

            descriptor.Field(e => e.LessonTime);
            descriptor.Field(e => e.LessonTimeId);

            descriptor.Field(e => e.Date);
            descriptor.Field(e => e.SubGroup).Type<SubGroupType>();
        }
    }
}
