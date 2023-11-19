using Models.Entities.Timetables.Cells;

namespace WebApi.GraphQL.ObjectTypes
{
    public class ActualTimetableCellType : ObjectType<ActualTimetableCell>
    {
        protected override void Configure(IObjectTypeDescriptor<ActualTimetableCell> descriptor)
        {
            descriptor.BindFieldsImplicitly();

            descriptor.Field(e => e.Cabinet).Type<NonNullType<CabinetType>>();
            descriptor.Field(e => e.Teacher).Type<NonNullType<TeacherType>>();
            descriptor.Field(e => e.Subject).Type<NonNullType<SubjectType>>();
            descriptor.Field(e => e.LessonTime).Type<NonNullType<LessonTimeType>>();
            //descriptor.Field(e => e.Date).Type<NonNullType<StringType>>();
        }
    }
}
