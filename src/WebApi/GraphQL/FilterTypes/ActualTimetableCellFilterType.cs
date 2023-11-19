using HotChocolate.Data.Filters;
using Models.Entities.Timetables.Cells;

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
