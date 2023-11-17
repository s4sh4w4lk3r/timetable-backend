using Models.Entities.Timetables.Cells.CellMembers;

namespace WebApi.GraphQL.Types.Timetables
{
    public class SubGroupType : EnumType<SubGroup>
    {
        protected override void Configure(IEnumTypeDescriptor<SubGroup> descriptor)
        {
            descriptor.BindValuesImplicitly();
        }
    }
}
