namespace WebApi.GraphQL.Types.Timetables
{
    public class DayOfWeekType : EnumType<DayOfWeek>
    {
        protected override void Configure(IEnumTypeDescriptor<DayOfWeek> descriptor)
        {
            descriptor.BindValuesImplicitly();
        }
    }
}
