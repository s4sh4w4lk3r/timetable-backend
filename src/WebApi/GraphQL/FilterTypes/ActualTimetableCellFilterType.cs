using HotChocolate.Data.Filters;
using Core.Entities.Timetables.Cells;

namespace WebApi.GraphQL.FilterTypes
{
    public class ActualTimetableCellFilterType : FilterInputType<ActualTimetableCell>
    {
        protected override void Configure(IFilterInputTypeDescriptor<ActualTimetableCell> descriptor)
        {
            descriptor.BindFieldsImplicitly();
        }
    }
}
