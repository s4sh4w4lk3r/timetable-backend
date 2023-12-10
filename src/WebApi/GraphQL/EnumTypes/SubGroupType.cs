using Core.Entities.Timetables.Cells.CellMembers;

namespace WebApi.GraphQL.EnumTypes
{
    public class SubGroupType : EnumType<SubGroup>
    {
        protected override void Configure(IEnumTypeDescriptor<SubGroup> descriptor)
        {
            descriptor.BindValuesImplicitly();
        }
    }
}
