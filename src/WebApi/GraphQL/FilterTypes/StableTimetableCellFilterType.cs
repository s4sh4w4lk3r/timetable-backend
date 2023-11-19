using HotChocolate.Data.Filters;
using Models.Entities.Timetables.Cells;

namespace WebApi.GraphQL.FilterTypes
{
    public class StableTimetableCellFilterType : FilterInputType<StableTimetableCell>
    {
        protected override void Configure(IFilterInputTypeDescriptor<StableTimetableCell> descriptor)
        {
            descriptor.BindFieldsImplicitly();
        }
    }
}
