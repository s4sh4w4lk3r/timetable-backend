using HotChocolate.Data.Filters;
using Core.Entities.Timetables.Cells;

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
