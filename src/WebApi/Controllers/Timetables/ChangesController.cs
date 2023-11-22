using Core.Entities.Timetables.Cells;
using Core.Entities.Timetables.Cells.CellMembers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Timetables.Interfaces;
using WebApi.Types;
using static WebApi.Controllers.Timetables.ChangesController;

namespace WebApi.Controllers.Timetables
{
    [ApiController, Route("timetable/changes")]
    public class ChangesController(IActualCellEditor actualCellEditor) : ControllerBase
    {
        private readonly IActualCellEditor _actualCellEditor = actualCellEditor;


        [HttpPost, Route("switch-cell-flag"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> SwitchFlag(SwitchFlagDto switchFlagDto, CancellationToken cancellationToken)
        {
#warning проверить ендпоинт
            if (switchFlagDto.ActualCellId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("ActualCellId"));
            }

            if (Enum.IsDefined(switchFlagDto.Flag) is false)
            {
                return BadRequest("Получен неверный Enum код.");
            }

            var serviceResult = await _actualCellEditor.SwitchCellFlag(switchFlagDto.ActualCellId, switchFlagDto.Flag, cancellationToken);
            if (serviceResult.Success is false)
            {
                return BadRequest(serviceResult);
            }

            return Ok(serviceResult);
        }

        [HttpPost, Route("delete-actual-cell"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCell(int actualCellId, CancellationToken cancellationToken)
        {
#warning проверить ендпоинт
            if (actualCellId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("actualCellId"));
            }

            var serviceResult = await _actualCellEditor.Delete(actualCellId, cancellationToken);
            if (serviceResult.Success is false)
            {
                return BadRequest(serviceResult);
            }

            return Ok(serviceResult);
        }

        public async Task<IActionResult> InsertCell(InsertableActualCellDto insertableActualCellDto)
        {
#warning проверить ендпоинт

            if (insertableActualCellDto.SubjectId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("SubjectId"));
            }

            if (insertableActualCellDto.ActualTimetableId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("ActualTimetableId"));
            }

            if (insertableActualCellDto.TeacherId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("TeacherId"));
            }

            if (insertableActualCellDto.LessonTimeId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("LessonTimeId"));
            }

            if (insertableActualCellDto.CabinetId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("CabinetId"));
            }

            bool dateParseOk = DateOnly.TryParse(insertableActualCellDto.Date, out DateOnly date);
            if (dateParseOk is false || date == default
                || date < new DateOnly(2023, 11, 11) || date > new DateOnly(2123, 11, 11))
            {
                return BadRequest("Некорректная дата указана.");
            }


            ActualTimetableCell actualTimetableCell = new(default, insertableActualCellDto.TeacherId,
                insertableActualCellDto.SubjectId, insertableActualCellDto.CabinetId, insertableActualCellDto.LessonTimeId, date);

            var serviceResult = await _actualCellEditor.Insert(insertableActualCellDto.ActualTimetableId, actualTimetableCell);
            if (serviceResult.Success is false)
            {
                return BadRequest(serviceResult);
            }

            return Ok(serviceResult);
        }

        public async Task<IActionResult> UpdateCell(UpdatebleActualCellDto updatebleActualCellDto)
        {
#warning возможно надо тут переделатьь
            if (updatebleActualCellDto.ActualTimetableCellId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("SubjectId"));
            }

            if (updatebleActualCellDto.SubjectId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("SubjectId"));
            }

            if (updatebleActualCellDto.ActualTimetableId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("ActualTimetableId"));
            }

            if (updatebleActualCellDto.TeacherId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("TeacherId"));
            }

            if (updatebleActualCellDto.LessonTimeId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("LessonTimeId"));
            }

            if (updatebleActualCellDto.CabinetId == default)
            {
                return BadRequest(ResponseMessage.GetMessageForDefaultValue("CabinetId"));
            }

            bool dateParseOk = DateOnly.TryParse(updatebleActualCellDto.Date, out DateOnly date);
            if (dateParseOk is false || date == default
                || date < new DateOnly(2023, 11, 11) || date > new DateOnly(2123, 11, 11))
            {
                return BadRequest("Некорректная дата указана.");
            }

            ActualTimetableCell actualTimetableCell = new(updatebleActualCellDto., updatebleActualCellDto.TeacherId,
               updatebleActualCellDto.SubjectId, updatebleActualCellDto.CabinetId, updatebleActualCellDto.LessonTimeId, date);

            var serviceResult = _actualCellEditor.Update(ac)
        }

        public record class InsertableActualCellDto(int ActualTimetableId, int SubjectId, int TeacherId, int LessonTimeId, int CabinetId, string Date, SubGroup SubGroup);
        public record class UpdatebleActualCellDto(int ActualTimetableCellId, int ActualTimetableId, int SubjectId, int TeacherId, int LessonTimeId, int CabinetId, string Date, SubGroup SubGroup);
        public record class SwitchFlagDto(int ActualCellId, IActualCellEditor.CellFlagToUpdate Flag);
    }
}
