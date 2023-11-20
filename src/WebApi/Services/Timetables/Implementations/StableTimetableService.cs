using Repository;
using WebApi.Services.Timetables.Interfaces;

namespace WebApi.Services.Timetables.Implementations
{
    public class StableTimetableService : IStableTimetableService
    {
        private readonly TimetableContext _timetableContext;

        public StableTimetableService(TimetableContext timetableContext)
        {
            _timetableContext = timetableContext;
        }

        public async Task<ServiceResult> ReadAndSaveAscXmlToRepoAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            try
            {
                var converter = new AscConverter.Converter(_timetableContext);
                await converter.ReadAsync(stream);
                await converter.SaveToDbAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                //return ServiceResult.Fail(ex.Message);
                throw;
            }

            return ServiceResult.Ok("База загружена.");
        }
    }
}
