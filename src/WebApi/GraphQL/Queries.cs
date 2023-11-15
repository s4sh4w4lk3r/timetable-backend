using HotChocolate.Authorization;
using Models.Entities.Timetables;
using Repository;

namespace WebApi.GraphQL
{
    public class Queries
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        [Authorize(Roles = new string[] { "Student" })]
        public IQueryable<ActualTimetable> GetActualTimetables([Service] TimetableContext dbContext)
        {
            return dbContext.Set<ActualTimetable>().AsQueryable();
        }
    }
}