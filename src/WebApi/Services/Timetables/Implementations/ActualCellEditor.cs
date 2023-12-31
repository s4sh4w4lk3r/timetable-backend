﻿using Microsoft.EntityFrameworkCore;
using Core.Entities.Timetables.Cells;
using Core.Entities.Timetables.Cells.CellMembers;
using Repository;
using Validation.IdValidators;
using WebApi.Services.Timetables.Interfaces;
using Core.Entities.Timetables;
using WebApi.Types;

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

        public async Task<ServiceResult> Update(ActualTimetableCell actualTimetableCell, CancellationToken cancellationToken = default)
        {
            if (actualTimetableCell is null)
            {
                return ServiceResult.Fail("ActualTimetableCell is null.");
            }

            var validateIds = new TimetableCellIdValidator().Validate(actualTimetableCell);
            if (validateIds.IsValid is false)
            {
                return ServiceResult.Fail(validateIds.ToString());
            }

            var actualTimetableCellFromRepo = await _timetableContext.Set<ActualTimetableCell>().SingleOrDefaultAsync(e => e.TimetableCellId == actualTimetableCell.TimetableCellId, cancellationToken: cancellationToken);
            if (actualTimetableCellFromRepo is null)
            {
                return ServiceResult.Fail(ResponseMessage.GetMessageIfNotFoundInDb("ячейка актуального расписания"));
            }

            bool teacherExist = await _timetableContext.Set<TeacherCM>().AnyAsync(e => e.TeacherId == actualTimetableCell.TeacherId, cancellationToken);
            if (teacherExist is false) return ServiceResult.Fail("Учителя с таким Id не существует.");

            bool subjectExist = await _timetableContext.Set<Subject>().AnyAsync(e => e.SubjectId == actualTimetableCell.SubjectId, cancellationToken);
            if (subjectExist is false) return ServiceResult.Fail("Предмета с таким Id не существует.");

            bool lessontimeExist = await _timetableContext.Set<LessonTime>().AnyAsync(e => e.LessonTimeId == actualTimetableCell.LessonTimeId, cancellationToken);
            if (lessontimeExist is false) return ServiceResult.Fail("Лессон тайма с таким Id не существует.");

            bool cabonetExist = await _timetableContext.Set<Cabinet>().AnyAsync(e => e.CabinetId == actualTimetableCell.CabinetId, cancellationToken);
            if (cabonetExist is false) return ServiceResult.Fail("Кабинета с таким Id не существует.");

            actualTimetableCellFromRepo.TeacherId = actualTimetableCell.TeacherId;
            actualTimetableCellFromRepo.SubjectId = actualTimetableCell.SubjectId;
            actualTimetableCellFromRepo.LessonTimeId = actualTimetableCell.LessonTimeId;
            actualTimetableCellFromRepo.CabinetId = actualTimetableCell.CabinetId;
            actualTimetableCellFromRepo.SubGroup = actualTimetableCell.SubGroup;

            _timetableContext.Set<ActualTimetableCell>().Update(actualTimetableCellFromRepo);
            await _timetableContext.SaveChangesAsync(cancellationToken);
            return ServiceResult.Ok("Ячейка обновлена.");
#warning сделать проверку чтобы нельзя было изменить лессонтайм на занятое место
        }

        public async Task<ServiceResult> Insert(int actualTimetableId, ActualTimetableCell actualTimetableCell, CancellationToken cancellationToken = default)
        {
#warning проверить
            if (actualTimetableCell is null)
            {
                return ServiceResult.Fail("ActualTimetableCell is null.");
            }

            var validateIds = new TimetableCellIdValidator().Validate(actualTimetableCell);
            if (validateIds.IsValid is false)
            {
                return ServiceResult.Fail(validateIds.ToString());
            }

            bool teacherExist = await _timetableContext.Set<TeacherCM>().AnyAsync(e => e.TeacherId == actualTimetableCell.TeacherId, cancellationToken);
            if (teacherExist is false) return ServiceResult.Fail("Учителя с таким Id не существует.");

            bool subjectExist = await _timetableContext.Set<Subject>().AnyAsync(e => e.SubjectId == actualTimetableCell.SubjectId, cancellationToken);
            if (subjectExist is false) return ServiceResult.Fail("Предмета с таким Id не существует.");

            bool lessontimeExist = await _timetableContext.Set<LessonTime>().AnyAsync(e => e.LessonTimeId == actualTimetableCell.LessonTimeId, cancellationToken);
            if (lessontimeExist is false) return ServiceResult.Fail("Лессон тайма с таким Id не существует.");

            bool cabonetExist = await _timetableContext.Set<Cabinet>().AnyAsync(e => e.CabinetId == actualTimetableCell.CabinetId, cancellationToken);
            if (cabonetExist is false) return ServiceResult.Fail("Кабинета с таким Id не существует.");

            var actualTimetable = await _timetableContext.Set<ActualTimetable>().Include(e => e.ActualTimetableCells)
                .SingleOrDefaultAsync(e => e.TimetableId == actualTimetableId, cancellationToken);
            if (actualTimetable is null) return ServiceResult.Fail(ResponseMessage.GetMessageIfNotFoundInDb("actualTimetable"));

            _timetableContext.Set<ActualTimetableCell>().Add(actualTimetableCell);
            actualTimetable.ActualTimetableCells.Add(actualTimetableCell);
            await _timetableContext.SaveChangesAsync(cancellationToken);
            return ServiceResult.Ok("Ячейка добавлена.");
        }
    }
}
