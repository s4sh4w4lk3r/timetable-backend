using Core.Entities.Timetables.Cells.CellMembers;

namespace WebApi.GraphQL.ObjectTypes
{
    public class CabinetType : ObjectType<Cabinet>
    {
        protected override void Configure(IObjectTypeDescriptor<Cabinet> descriptor)
        {
            descriptor.BindFieldsImplicitly();

            descriptor.Ignore(e => e.StableTimetableCells);
            descriptor.Ignore(e => e.ActualTimetableCells);
        }
    }
}
