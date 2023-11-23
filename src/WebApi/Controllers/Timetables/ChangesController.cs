using Core.Entities.Timetables.Cells;
using Core.Entities.Timetables.Cells.CellMembers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Timetables.Interfaces;
using WebApi.Types;

namespace WebApi.Controllers.Timetables
{
    [ApiController, Route("timetable/changes")]
    public class ChangesController(IActualCellEditor actualCellEditor) : ControllerBase
    {
        private readonly IActualCellEditor _actualCellEditor = actualCellEditor;


        [HttpPost, Route("switch-cell-flag"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> SwitchFlag(SwitchFlagDto switchFlagDto, CancellationToken cancellationToken)
        {
            if (switchFlagDto.ActualTimetableCellId == default)
            {
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("ActualCellId"));
            }

            if (Enum.IsDefined(switchFlagDto.Flag) is false)
            {
                return BadRequest("Получен неверный Enum код.");
            }

            var serviceResult = await _actualCellEditor.SwitchCellFlag(switchFlagDto.ActualTimetableCellId, switchFlagDto.Flag, cancellationToken);
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
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("actualCellId"));
            }

            var serviceResult = await _actualCellEditor.Delete(actualCellId, cancellationToken);
            if (serviceResult.Success is false)
            {
                return BadRequest(serviceResult);
            }

            return Ok(serviceResult);
        }

        [HttpPost, Route("insert-actual-cell"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Insert(InsertableActualCellDto insertableActualCellDto)
        {
#warning проверить ендпоинт

            if (insertableActualCellDto.SubjectId == default)
            {
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("SubjectId"));
            }

            if (insertableActualCellDto.ActualTimetableId == default)
            {
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("ActualTimetableId"));
            }

            if (insertableActualCellDto.TeacherId == default)
            {
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("TeacherId"));
            }

            if (insertableActualCellDto.LessonTimeId == default)
            {
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("LessonTimeId"));
            }

            if (insertableActualCellDto.CabinetId == default)
            {
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("CabinetId"));
            }

            bool dateParseOk = DateOnly.TryParse(insertableActualCellDto.Date, out DateOnly date);
            if (dateParseOk is false || date == default
                || date < new DateOnly(2023, 11, 11) || date > new DateOnly(2123, 11, 11))
            {
                return BadRequest("Некорректная дата указана.");
            }


            ActualTimetableCell actualTimetableCell = new(default, insertableActualCellDto.TeacherId,
                insertableActualCellDto.SubjectId, insertableActualCellDto.CabinetId, insertableActualCellDto.LessonTimeId, subGroup: insertableActualCellDto.SubGroup, date);

            var serviceResult = await _actualCellEditor.Insert(insertableActualCellDto.ActualTimetableId, actualTimetableCell);
            if (serviceResult.Success is false)
            {
                return BadRequest(serviceResult);
            }

            return Ok(serviceResult);
        }

        [HttpPost, Route("update-actual-cell"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdatebleActualCellDto updatebleActualCellDto)
        {
            if (updatebleActualCellDto.ActualTimetableCellId == default)
            {
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("ActualTimetableCellId"));
            }

            if (updatebleActualCellDto.SubjectId == default)
            {
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("SubjectId"));
            }

            if (updatebleActualCellDto.TeacherId == default)
            {
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("TeacherId"));
            }

            if (updatebleActualCellDto.LessonTimeId == default)
            {
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("LessonTimeId"));
            }

            if (updatebleActualCellDto.CabinetId == default)
            {
                return BadRequest(ResponseMessage.GetMessageIfDefaultValue("CabinetId"));
            }

            ActualTimetableCell actualTimetableCell = new(updatebleActualCellDto.ActualTimetableCellId, updatebleActualCellDto.TeacherId,
               updatebleActualCellDto.SubjectId, updatebleActualCellDto.CabinetId, updatebleActualCellDto.LessonTimeId, updatebleActualCellDto.SubGroup, default);

            var serviceResult = await _actualCellEditor.Update(actualTimetableCell);
            if (serviceResult.Success is false)
            {
                return BadRequest(serviceResult);
            }

            return Ok(serviceResult);
        }

        public record class InsertableActualCellDto(int ActualTimetableId, int SubjectId, int TeacherId, int LessonTimeId, int CabinetId, string Date, SubGroup SubGroup);
        public record class UpdatebleActualCellDto(int ActualTimetableCellId, int SubjectId, int TeacherId, int LessonTimeId, int CabinetId, SubGroup SubGroup);
        public record class SwitchFlagDto(int ActualTimetableCellId, IActualCellEditor.CellFlagToUpdate Flag);
    }
}
