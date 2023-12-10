using Core.Entities.Timetables;

namespace WebApi.GraphQL.ObjectTypes
{
    public class GroupType : ObjectType<Group>
    {
        protected override void Configure(IObjectTypeDescriptor<Group> descriptor)
        {
            descriptor.BindFieldsImplicitly();
            descriptor.Ignore(e => e.RegistrationEntities);
            descriptor.Ignore(e => e.Students);
        }
    }
}
