using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Services.Timetables.Interfaces;

namespace WebApi.Controllers.Timetables
{
    [ApiController, Route("timetable")]
    public class TimetableController : ControllerBase
    {
        private readonly IStableTimetableService _stableTimetableService;
        private readonly IActualTimetableService _actualTimetableService;

        public TimetableController(IStableTimetableService stableTimetableService, IActualTimetableService actualTimetableService)
        {
            _stableTimetableService = stableTimetableService;
            _actualTimetableService = actualTimetableService;
        }

        [HttpPost, Route("import-xml"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ImportAscBase()
        {
            var result = await _stableTimetableService.ReadAndSaveAscXmlToRepoAsync(HttpContext.Request.Body);
            if (result.Success is false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost, Route("convert-stable-to-actual"), /*Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> ConvertStableToActual(GroupIdAndDatesDto groupIdAndDatesDto)
        {
            if ((groupIdAndDatesDto.Dates is null) || (groupIdAndDatesDto.Dates.Any() is false))
            {
                return BadRequest("Даты не получены");
            }


            var datesParsed = new List<DateOnly>();
            foreach (var item in groupIdAndDatesDto.Dates)
            {
                if (DateOnly.TryParse(item, out DateOnly result) is false)
                {
                    return BadRequest("Неверный формат дат. Все даты должны быть получены в формате дд.мм.гггг.");
                }
                datesParsed.Add(result);
            }

            ServiceResult actualServiceResult;
            if (groupIdAndDatesDto.StableGroupId is int id && id > 0)
            {
                actualServiceResult = await _actualTimetableService.CreateOnlyOneActualTimetable(id, datesParsed);
            }
            else
            {
                actualServiceResult = await _actualTimetableService.CreateActualTimetableForAll(datesParsed);
            }


            if (actualServiceResult.Success is false)
            {
                return BadRequest(actualServiceResult);
            }

            return Ok(actualServiceResult);
        }

        public record class GroupIdAndDatesDto(int? StableGroupId, IEnumerable<string> Dates);
    }
}
