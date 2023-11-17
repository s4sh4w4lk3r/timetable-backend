using Models.Entities.Timetables;

namespace WebApi.GraphQL.Types.Timetables
{
    public class GroupType : ObjectType<Group>
    {
        protected override void Configure(IObjectTypeDescriptor<Group> descriptor)
        {
            descriptor.Field(e => e.GroupId);
            descriptor.Field(e => e.Name);
        }
    }
}
