using HotChocolate.Authorization;
using Models.Entities.Identity;
using Models.Entities.Identity.Users;
using Repository;

namespace WebApi.GraphQL
{
    public class Queries
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        [Authorize(Roles = new string[] {"Student"})]
        public IQueryable<UserSession> GetTeachers([Service] TimetableContext dbContext)
        {
            return dbContext.Set<UserSession>().AsQueryable();
        }
    }
}
