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

        /// <summary>
        /// Принимает в теле содержимое XML документа из ASC Timetables с расписанием, которое потом сохраняется как стабильное расписание в СУБД.
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("import-xml"), /*Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> ImportAscBase()
        {
            var result = await _stableTimetableService.ReadAndSaveAscXmlToRepoAsync(HttpContext.Request.Body);
            if (result.Success is false)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        
        /// <summary>
        /// Добавляет в бд актуальное расписание на указанные даты. 
        /// </summary>
        /// <remarks>
        /// Пример запроса: 
        /// 
        /// </remarks>
        /// <param name="groupIdAndDatesDto"></param>
        /// <returns></returns>
        [HttpPost, Route("convert-stable-to-actual-for-group"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConvertStableToActualForSpecifiedGroup(GroupIdAndDatesDto groupIdAndDatesDto)
        {
#warning проверить
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
                return BadRequest("Id группы, для которой надо добавить расписание, не указан.");
            }

            if (actualServiceResult.Success is false)
            {
                return BadRequest(actualServiceResult);
            }

            return Ok(actualServiceResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupIdAndDatesDto"></param>
        /// <returns></returns>
        [HttpPost, Route("convert-stable-to-actual-all-groups"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConvertStableToActualForAllGroups(GroupIdAndDatesDto groupIdAndDatesDto)
        {
#warning проверить
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

            ServiceResult actualServiceResult = await _actualTimetableService.CreateActualTimetableForAll(datesParsed);
            if (actualServiceResult.Success is false)
            {
                return BadRequest(actualServiceResult);
            }

            return Ok(actualServiceResult);
        }

        /// <summary>
        /// Дтошка для создания актуального расписания.
        /// </summary>
        public record class GroupIdAndDatesDto
        {
            /// <summary>
            /// Айди группы, для которой нужно добавить актуальное расписание. Является nullable типом.
            /// </summary>
            public int? StableGroupId { get; init; }

            /// <summary>
            /// Массив дат, на эти даты будет сделано актуальное расписание.
            /// </summary>
            public IEnumerable<string> Dates { get; init; }

            public GroupIdAndDatesDto(int? stableGroupId, IEnumerable<string> dates)
            {
                StableGroupId = stableGroupId;
                Dates = dates;
            }
        }
    }
}
