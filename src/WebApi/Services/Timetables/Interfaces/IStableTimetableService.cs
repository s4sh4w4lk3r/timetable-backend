namespace WebApi.Services.Timetables.Interfaces
{
    public interface IStableTimetableService
    {
        public Task<ServiceResult> ReadAndSaveAscXmlToRepoAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}
