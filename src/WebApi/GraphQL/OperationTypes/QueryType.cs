using Models.Entities.Timetables;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables.Cells.CellMembers;
using Repository;
using WebApi.GraphQL.FilterTypes;
using WebApi.GraphQL.SortTypes;

namespace WebApi.GraphQL.OperationTypes
{
    public class Query
    {
        public IQueryable<Subject> GetSubjects([Service(ServiceKind.Synchronized)] TimetableContext context) => context.Set<Subject>();
        public IQueryable<TeacherCM> GetTeachers([Service(ServiceKind.Synchronized)] TimetableContext context) => context.Set<TeacherCM>();
        public IQueryable<Group> GetGroups([Service(ServiceKind.Synchronized)] TimetableContext context) => context.Set<Group>();
        public IQueryable<Cabinet> GetCabinets([Service(ServiceKind.Synchronized)] TimetableContext context) => context.Set<Cabinet>();
        public IQueryable<LessonTime> GetLessonTimes([Service(ServiceKind.Synchronized)] TimetableContext context) => context.Set<LessonTime>();
        public IQueryable<ActualTimetableCell> GetActualTimetableCells([Service(ServiceKind.Synchronized)] TimetableContext context) => context.Set<ActualTimetableCell>();
        public IQueryable<ActualTimetable> GetActualTimetables([Service(ServiceKind.Synchronized)] TimetableContext context) => context.Set<ActualTimetable>();
        public IQueryable<StableTimetableCell> GetStableTimetableCells([Service(ServiceKind.Synchronized)] TimetableContext context) => context.Set<StableTimetableCell>();
        public IQueryable<StableTimetable> GetStableTimetables([Service(ServiceKind.Synchronized)] TimetableContext context) => context.Set<StableTimetable>();
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

            descriptor.Field(e => e.GetActualTimetableCells(default!)).UseFiltering<ActualTimetableCellFilterType>().UseSorting<ActualTimetableCellSortType>();
            descriptor.Field(e => e.GetActualTimetables(default!)).UseFiltering<ActualTimetableFilterType>().UseSorting<ActualTimetableSortType>();

            descriptor.Field(e => e.GetStableTimetableCells(default!)).UseProjection().UseFiltering<StableTimetableCellFilterType>().UseSorting<StableTimetableCellSortType>();
            descriptor.Field(e => e.GetStableTimetables(default!)).UseProjection().UseFiltering<StableTimetableFilterType>().UseSorting<StableTimetableSortType>();
        }
    }
}