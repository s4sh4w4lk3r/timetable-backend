namespace WebApi.GraphQL
{
    public class QueryType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Field("Teachers").Resolve("bebra");
            descriptor.Field("Subjects").Resolve("bebra");
            descriptor.Field("Groups").Resolve("bebra");
            descriptor.Field("Cabinets").Resolve("bebra");
            descriptor.Field("LessonTimes").Resolve("bebra");
            descriptor.Field("ActualTimetableCells").Resolve("bebra");
            descriptor.Field("ActualTimetables").Resolve("bebra");
            descriptor.Field("StableTimetableCells").Resolve("bebra");
            descriptor.Field("StableTimetables").Resolve("bebra");
            descriptor.Field("SubGroups").Resolve("bebra");
        }
    }
}