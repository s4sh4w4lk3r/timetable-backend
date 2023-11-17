using Models.Entities.Timetables.Cells.CellMembers;

namespace WebApi.GraphQL.Types.Timetables
{
    public class SubjectType : ObjectType<Subject>
    {
        protected override void Configure(IObjectTypeDescriptor<Subject> descriptor)
        {
            descriptor.Field(e => e.SubjectId);
            descriptor.Field(e => e.Name);
        }
    }
}
