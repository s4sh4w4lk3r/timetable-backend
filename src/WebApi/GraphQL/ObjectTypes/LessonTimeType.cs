using Core.Entities.Timetables.Cells.CellMembers;

namespace WebApi.GraphQL.ObjectTypes
{
    public class LessonTimeType : ObjectType<LessonTime>
    {
        protected override void Configure(IObjectTypeDescriptor<LessonTime> descriptor)
        {
            descriptor.BindFieldsImplicitly();

            descriptor.Ignore(e => e.StableTimetableCells);
            descriptor.Ignore(e => e.ActualTimetableCells);

            descriptor.Field(e=>e.EndsAt).Type<NonNullType<StringType>>();
            descriptor.Field(e=>e.StartsAt).Type<NonNullType<StringType>>();
        }
    }
}
