using HotChocolate.Data.Sorting;
using Models.Entities.Timetables.Cells;

namespace WebApi.GraphQL.SortTypes
{
    public class ActualTimetableCellSortType : SortInputType<ActualTimetableCell>
    {
        protected override void Configure(ISortInputTypeDescriptor<ActualTimetableCell> descriptor)
        {
            descriptor.BindFieldsImplicitly();
        }
    }
}
