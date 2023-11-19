using Microsoft.AspNetCore.Mvc;
using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables.Cells.CellMembers;
using Repository;
using WebApi.GraphQL.FilterTypes;

namespace WebApi.GraphQL.OperationTypes
{
    public class Query
    {
        public IQueryable<Subject> GetSubjects([FromServices] TimetableContext context) => context.Set<Subject>();
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
            descriptor.Field(e => e.GetTeachers(default!))
                .UseFiltering<TeacherCM>(x =>
                {
                    x.BindFieldsImplicitly(); x.Ignore(e => e.StableTimetableCells); x.Ignore(e => e.ActualTimetableCells);
                })
                .UseSorting<TeacherCM>(x =>
                {
                    x.BindFieldsImplicitly(); x.Ignore(e => e.StableTimetableCells); x.Ignore(e => e.ActualTimetableCells);
                });


            descriptor.Field(e => e.GetCabinets(default!))
                .UseFiltering<Cabinet>(x =>
                {
                    x.BindFieldsImplicitly(); x.Ignore(e => e.StableTimetableCells); x.Ignore(e => e.ActualTimetableCells);
                })
                .UseSorting<Cabinet>(x =>
                {
                    x.BindFieldsImplicitly(); x.Ignore(e => e.StableTimetableCells); x.Ignore(e => e.ActualTimetableCells);
                });


            descriptor.Field(e => e.GetLessonTimes(default!))
                .UseFiltering<LessonTime>(x =>
                {
                    x.BindFieldsImplicitly(); x.Ignore(e => e.StableTimetableCells); x.Ignore(e => e.ActualTimetableCells);
                    x.Field(e => e.EndsAt).Type<NonNullType<StringType>>(); x.Field(e => e.StartsAt).Type<NonNullType<StringType>>();
                })
                .UseSorting<LessonTime>(x =>
                {
                    x.BindFieldsImplicitly(); x.Ignore(e => e.StableTimetableCells); x.Ignore(e => e.ActualTimetableCells);
                    x.Field(e => e.EndsAt).Type<NonNullType<StringType>>(); x.Field(e => e.StartsAt).Type<NonNullType<StringType>>();
                });


            descriptor.Field(e => e.GetSubjects(default!))
                .UseFiltering<Subject>(x =>
                {
                    x.BindFieldsImplicitly(); x.Ignore(e => e.StableTimetableCells); x.Ignore(e => e.ActualTimetableCells);
                })
                .UseSorting<Subject>(x =>
                {
                    x.BindFieldsImplicitly(); x.Ignore(e => e.StableTimetableCells); x.Ignore(e => e.ActualTimetableCells);
                });


            descriptor.Field(e => e.GetGroups(default!))
                .UseFiltering<Group>(x =>
                {
                    x.BindFieldsImplicitly(); x.Ignore(e => e.RegistrationEntities); x.Ignore(e => e.Students);
                })
                .UseSorting<Group>(x =>
                {
                    x.BindFieldsImplicitly(); x.Ignore(e => e.RegistrationEntities); x.Ignore(e => e.Students);
                });


            descriptor.Field(e => e.GetActualTimetableCells(default!)).UseFiltering<ActualTimetableCellFilterType>();
            descriptor.Field(e => e.GetActualTimetables(default!)).UseFiltering<ActualTimetableFilterType>();

            descriptor.Field(e => e.GetStableTimetableCells(default!)).UseFiltering<StableTimetableCellFilterType>();
            descriptor.Field(e => e.GetStableTimetables(default!)).UseFiltering<StableTimetableFilterType>();
        }
    }
}