namespace WebApi.Services.Timetables.Interfaces
{
    public interface IActualTimetableService
    {
        public Task<ServiceResult> CreateOnlyOneActualTimetable(int stableTimetableId, IEnumerable<DateOnly> datesOnly, CancellationToken cancellationToken = default);
        public Task<ServiceResult> CreateActualTimetableForAll(IEnumerable<DateOnly> datesOnly, CancellationToken cancellationToken = default);
    }
}
