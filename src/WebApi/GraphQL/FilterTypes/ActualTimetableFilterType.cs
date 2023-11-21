using HotChocolate.Data.Filters;
using Core.Entities.Timetables;

namespace WebApi.GraphQL.FilterTypes
{
    public class ActualTimetableFilterType : FilterInputType<ActualTimetable>
    {
        protected override void Configure(IFilterInputTypeDescriptor<ActualTimetable> descriptor)
        {
            descriptor.BindFieldsImplicitly();
            descriptor.Field(e => e.ActualTimetableCells).Type<ActualTimetableCellFilterType>();
        }
    }
}
