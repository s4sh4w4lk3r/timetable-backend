using Models.Entities.Timetables.Cells.CellMembers;

namespace WebApi.GraphQL.ObjectTypes
{
    public class SubjectType : ObjectType<Subject>
    {
        protected override void Configure(IObjectTypeDescriptor<Subject> descriptor)
        {
            descriptor.BindFieldsImplicitly();

            descriptor.Ignore(e => e.StableTimetableCells);
            descriptor.Ignore(e => e.ActualTimetableCells);
        }
    }
}
