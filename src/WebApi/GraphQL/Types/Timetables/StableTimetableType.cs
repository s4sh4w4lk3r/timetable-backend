using Models.Entities.Timetables;

namespace WebApi.GraphQL.Types.Timetables
{
    public class StableTimetableType : ObjectType<StableTimetable>
    {
        protected override void Configure(IObjectTypeDescriptor<StableTimetable> descriptor)
        {
            descriptor.Field(e => e.Group);
            descriptor.Field(e => e.TimetableId);
            descriptor.Field(e => e.GroupId);
            descriptor.Field(e => e.StableTimetableCells);
        }
    }
}
