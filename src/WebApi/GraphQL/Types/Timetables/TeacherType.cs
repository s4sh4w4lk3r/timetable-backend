using Models.Entities.Timetables.Cells.CellMembers;

namespace WebApi.GraphQL.Types.Timetables
{
    public class TeacherType : ObjectType<TeacherCM>
    {
        protected override void Configure(IObjectTypeDescriptor<TeacherCM> descriptor)
        {
            descriptor.Field(e => e.TeacherId);
            descriptor.Field(e => e.Lastname);
            descriptor.Field(e => e.Firstname);
            descriptor.Field(e => e.Middlename);
        }
    }
}
