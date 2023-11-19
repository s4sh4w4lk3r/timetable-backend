using Models.Entities.Timetables.Cells.CellMembers;

namespace WebApi.GraphQL.ObjectTypes
{
    public class TeacherType : ObjectType<TeacherCM>
    {
        protected override void Configure(IObjectTypeDescriptor<TeacherCM> descriptor)
        {
            descriptor.BindFieldsImplicitly();

            descriptor.Ignore(e => e.StableTimetableCells);
            descriptor.Ignore(e => e.ActualTimetableCells);
        }
    }
}
