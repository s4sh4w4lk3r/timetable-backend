using Models.Entities.Timetables.Cells;

namespace WebApi.GraphQL.Types.Timetables
{
    public class StableTimetableCellType : ObjectType<StableTimetableCell>
    {
        protected override void Configure(IObjectTypeDescriptor<StableTimetableCell> descriptor)
        {
            descriptor.Field(e => e.SubGroup).Type<SubGroupType>();
            descriptor.Field(e => e.IsWeekEven);
            descriptor.Field(e => e.TimetableCellId);
            descriptor.Field(e => e.DayOfWeek).Type<DayOfWeekType>();

            descriptor.Field(e => e.Cabinet).Type<NonNullType<CabinetType>>();
            descriptor.Field(e => e.CabinetId);

            descriptor.Field(e => e.Teacher).Type<NonNullType<TeacherType>>();
            descriptor.Field(e => e.TeacherId);

            descriptor.Field(e => e.Subject).Type<NonNullType<SubjectType>>();
            descriptor.Field(e => e.SubjectId);

            descriptor.Field(e => e.LessonTime).Type<NonNullType<LessonTimeType>>();
            descriptor.Field(e => e.LessonTimeId);
        }
    }
}
