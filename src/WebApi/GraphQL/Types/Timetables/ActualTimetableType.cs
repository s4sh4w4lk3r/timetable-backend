using Models.Entities.Timetables;

namespace WebApi.GraphQL.Types.Timetables
{


    public class ActualTimetableType : ObjectType<ActualTimetable>
    {
        protected override void Configure(IObjectTypeDescriptor<ActualTimetable> descriptor)
        {
            descriptor.Field(e => e.TimetableId);
            descriptor.Field(e => e.WeekNumber);
            descriptor.Field(e => e.ActualTimetableCells);
            descriptor.Field(e => e.Group);
            descriptor.Field(e => e.GroupId);
        }
    }
}
