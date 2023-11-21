using HotChocolate.Data.Filters;
using Core.Entities.Timetables;

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
