using HotChocolate.Data.Sorting;
using Core.Entities.Timetables.Cells;

namespace WebApi.GraphQL.SortTypes
{
    public class StableTimetableCellSortType : SortInputType<StableTimetableCell>
    {
        protected override void Configure(ISortInputTypeDescriptor<StableTimetableCell> descriptor)
        {
            descriptor.BindFieldsImplicitly();
        }
    }
}
