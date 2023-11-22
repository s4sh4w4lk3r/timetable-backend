using Microsoft.EntityFrameworkCore;
using Core.Entities.Timetables.Cells;
using Core.Entities.Timetables.Cells.CellMembers;
using Repository;
using Validation.IdValidators;
using WebApi.Services.Timetables.Interfaces;

namespace WebApi.Services.Timetables.Implementations
{
    public class ActualCellEditor : IActualCellEditor
    {
        private readonly TimetableContext _timetableContext;

        public ActualCellEditor(TimetableContext timetableContext)
        {
            _timetableContext = timetableContext;
        }

        public async Task<ServiceResult> Delete(int id, CancellationToken cancellationToken = default)
        {
#warning проверить
            var actualTimetableCell = await _timetableContext.Set<ActualTimetableCell>().SingleOrDefaultAsync(cancellationToken);
            if (actualTimetableCell is null)
            {
                return ServiceResult.Fail("Такой ячейки расписания не существует в актуальном расписании");
            }

            _timetableContext.Remove(actualTimetableCell);
            await _timetableContext.SaveChangesAsync(cancellationToken);
            return ServiceResult.Ok("Ячейка удалена из актуального расписания");
        }

        public async Task<ServiceResult<bool>> SwitchCellFlag(int actualTimetableCellId, IActualCellEditor.CellFlagToUpdate flag, CancellationToken cancellationToken = default)
        {
#warning проверить
            var actualTimetableCell = await _timetableContext.Set<ActualTimetableCell>().SingleOrDefaultAsync(e=>e.TimetableCellId == actualTimetableCellId, cancellationToken);
            if (actualTimetableCell is null)
            {
                return ServiceResult<bool>.Fail("Такой ячейки расписания не существует в актуальном расписании", false);
            }

            switch (flag)
            {
                case IActualCellEditor.CellFlagToUpdate.IsModified:
                    actualTimetableCell.IsModified = !actualTimetableCell.IsModified;
                    await _timetableContext.SaveChangesAsync(cancellationToken);
                    return ServiceResult<bool>.Ok("IsModifiedFlag обновлен.", actualTimetableCell.IsModified);

                case IActualCellEditor.CellFlagToUpdate.IsCanceled:
                    actualTimetableCell.IsCanceled = !actualTimetableCell.IsCanceled;
                    await _timetableContext.SaveChangesAsync(cancellationToken);
                    return ServiceResult<bool>.Ok("IsCanceledFlag обновлен.", actualTimetableCell.IsCanceled);

                case IActualCellEditor.CellFlagToUpdate.IsMoved:
                    actualTimetableCell.IsMoved = !actualTimetableCell.IsMoved;
                    await _timetableContext.SaveChangesAsync(cancellationToken);
                    return ServiceResult<bool>.Ok("IsMovedFlag обновлен.", actualTimetableCell.IsMoved);

                default:
                    return ServiceResult<bool>.Fail("Получен неизвестный флаг", false);
            }
        }

        public async Task<ServiceResult> InsertOrUpdate(ActualTimetableCell actualTimetableCell, CancellationToken cancellationToken = default)
        {
#warning проверить
            if (actualTimetableCell is null)
            {
                return ServiceResult.Fail("ActualTimetable is null.");
            }

            var validateIds = new TimetableCellIdValidator().Validate(actualTimetableCell);
            if (validateIds.IsValid is false)
            {
                return ServiceResult.Fail(validateIds.ToString());
            }

            if (actualTimetableCell.TimetableCellId == default)
            {
                bool teacherExist = await _timetableContext.Set<TeacherCM>().AnyAsync(e => e.TeacherId == actualTimetableCell.TeacherId, cancellationToken);
                if (teacherExist is false) return ServiceResult.Fail("Учителя с таким Id не существует.");

                bool subjectExist = await _timetableContext.Set<Subject>().AnyAsync(e => e.SubjectId == actualTimetableCell.SubjectId, cancellationToken);
                if (subjectExist is false) return ServiceResult.Fail("Предмета с таким Id не существует.");

                bool lessontimeExist = await _timetableContext.Set<LessonTime>().AnyAsync(e => e.LessonTimeId == actualTimetableCell.LessonTimeId, cancellationToken);
                if (lessontimeExist is false) return ServiceResult.Fail("Лессон тайма с таким Id не существует.");

                bool cabonetExist = await _timetableContext.Set<Cabinet>().AnyAsync(e => e.CabinetId == actualTimetableCell.CabinetId, cancellationToken);
                if (cabonetExist is false) return ServiceResult.Fail("Кабинета с таким Id не существует.");

                _timetableContext.Set<ActualTimetableCell>().Add(actualTimetableCell);
                await _timetableContext.SaveChangesAsync(cancellationToken);
                return ServiceResult.Ok("Ячейка добавлена.");
            }
            else
            {
                // Обнуляю нав. свойства, чтобы не было исключения из-за повторяющихся айдишников в локальном контексте. Обновлем данные по айдишникам из других таблиц.
                actualTimetableCell.Teacher = null;
                actualTimetableCell.Subject = null;
                actualTimetableCell.LessonTime = null;
                actualTimetableCell.Cabinet = null;

                _timetableContext.Set<ActualTimetableCell>().Update(actualTimetableCell);
                await _timetableContext.SaveChangesAsync(cancellationToken);
                return ServiceResult.Ok("Ячейка обновлена.");
            }
        }
    }
}
