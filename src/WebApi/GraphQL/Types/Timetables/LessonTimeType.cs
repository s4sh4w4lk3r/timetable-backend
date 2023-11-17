using Models.Entities.Timetables.Cells.CellMembers;

namespace WebApi.GraphQL.Types.Timetables
{
    public class LessonTimeType : ObjectType<LessonTime>
    {
        protected override void Configure(IObjectTypeDescriptor<LessonTime> descriptor)
        {
            descriptor.Field(e => e.LessonTimeId);
            descriptor.Field(e => e.Number);
            descriptor.Field(e => e.StartsAt).Type<StringType>();
            descriptor.Field(e => e.EndsAt).Type<StringType>();
        }
    }
}
