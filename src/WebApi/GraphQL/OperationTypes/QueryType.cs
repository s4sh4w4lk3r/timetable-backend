using Microsoft.AspNetCore.Mvc;
using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables.Cells.CellMembers;
using Repository;

namespace WebApi.GraphQL.OperationTypes
{
    public class Query
    {
        public IQueryable<TeacherCM> GetTeachers([FromServices] TimetableContext context) => context.Set<TeacherCM>();
        public IQueryable<Group> GetGroups([FromServices] TimetableContext context) => context.Set<Group>();
        public IQueryable<Cabinet> GetCabinets([FromServices] TimetableContext context) => context.Set<Cabinet>();
        public IQueryable<LessonTime> GetLessonTimes([FromServices] TimetableContext context) => context.Set<LessonTime>();
        public IQueryable<ActualTimetableCell> GetActualTimetableCells([FromServices] TimetableContext context) => context.Set<ActualTimetableCell>();
        public IQueryable<ActualTimetable> GetActualTimetables([FromServices] TimetableContext context) => context.Set<ActualTimetable>();
        public IQueryable<StableTimetableCell> GetStableTimetableCells([FromServices] TimetableContext context) => context.Set<StableTimetableCell>();
        public IQueryable<StableTimetable> GetStableTimetables([FromServices] TimetableContext context) => context.Set<StableTimetable>();
    }

    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(e => e.GetTeachers(default!)).UseFiltering();  
            descriptor.Field(e => e.GetGroups(default!)).UseFiltering();
            descriptor.Field(e => e.GetCabinets(default!)).UseFiltering();
            descriptor.Field(e => e.GetLessonTimes(default!)).UseFiltering();
            descriptor.Field(e => e.GetActualTimetableCells(default!)).UseFiltering();
            descriptor.Field(e => e.GetActualTimetables(default!)).UseFiltering();
            descriptor.Field(e => e.GetStableTimetableCells(default!)).UseFiltering();
            descriptor.Field(e => e.GetStableTimetables(default!)).UseFiltering();
        }
    }
}