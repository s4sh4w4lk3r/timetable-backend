using HotChocolate.Data.Filters;
using Models.Entities.Timetables;

namespace WebApi.GraphQL.FilterTypes
{
    public class StableTimetableFilterType : FilterInputType<StableTimetable>
    {
        protected override void Configure(IFilterInputTypeDescriptor<StableTimetable> descriptor)
        {
            descriptor.BindFieldsImplicitly();
            descriptor.Field(e => e.StableTimetableCells).Type<StableTimetableCellFilterType>();
        }
    }
}
