using Models.Entities.Timetables;

namespace WebApi.GraphQL.ObjectTypes
{
    public class StableTimetableType : ObjectType<StableTimetable>
    {
        protected override void Configure(IObjectTypeDescriptor<StableTimetable> descriptor)
        {
            descriptor.BindFieldsImplicitly();
            descriptor.Ignore(e => e.CheckNoDuplicates());
        }
    }
}
