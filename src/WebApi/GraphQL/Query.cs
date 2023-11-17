using HotChocolate.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Timetables;
using Repository;

namespace WebApi.GraphQL
{
    public class Query
    {
        [UseProjection]
        [UseFiltering]
        //[UseSorting]
        [Authorize(Roles = new string[] { "Student", "Admin", "Teacher" })]
        public IQueryable<ActualTimetable> GetTimetables([FromServices] TimetableContext timetableContext)
        {
            /*return dbContext.Set<ActualTimetable>()
                .Include(e => e.Group)
                .Include(e => e.ActualTimetableCells!).ThenInclude(e => e.Teacher)
                .Include(e => e.ActualTimetableCells!).ThenInclude(e => e.Subject)
                .Include(e => e.ActualTimetableCells!).ThenInclude(e => e.LessonTime)
                .Include(e => e.ActualTimetableCells!).ThenInclude(e => e.Cabinet)
                .ToList();*/
            return timetableContext.Set<ActualTimetable>().AsQueryable();
        }
    }

    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.BindFieldsImplicitly();
        }
    }
}