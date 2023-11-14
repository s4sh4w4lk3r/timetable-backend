using Repository;
using System.Xml;
using WebApi.Services.Timetables.Interfaces;

namespace WebApi.Services.Timetables
{
    public class StableTimetableService : IStableTimetableService
    {
        private readonly TimetableContext _timetableContext;

        public StableTimetableService(TimetableContext timetableContext)
        {
            _timetableContext = timetableContext;
        }

        public async Task<ServiceResult> ReadAndSaveAscXmlToRepoAsync(Stream stream)
        {
            try
            {
                var converter = new AscConverter.Converter(_timetableContext);
                await converter.ReadAsync(stream);
                await converter.SaveToDbAsync();
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail(ex.Message);
            }

            return ServiceResult.Ok("База загружена.");
        }
    }
}
