using Models.Entities.Timetables.Cells.CellMembers;

namespace WebApi.GraphQL.Types.Timetables
{
    public class CabinetType : ObjectType<Cabinet>
    {
        protected override void Configure(IObjectTypeDescriptor<Cabinet> descriptor)
        {
            descriptor.Field(e => e.CabinetId);
            descriptor.Field(e => e.Address);
            descriptor.Field(e => e.Number);
        }
    }
}
