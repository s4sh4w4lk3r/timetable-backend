using Microsoft.EntityFrameworkCore;
using Models.Entities.Timetables.Cells;
using Models.Entities.Timetables;
using Repository;
using Models.Entities.Timetables.Cells.CellMembers;

namespace WebApi.Services.Timetables
{
    public class ChangesService
    {
        private readonly TimetableContext _dbContext;

        public ChangesService(TimetableContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResult> MarkAsCanceled(int timetableCellId, CancellationToken cancellationToken = default)
        {
#warning проверить
            if (timetableCellId == default)
            {
                return ServiceResult<ActualTimetable?>.Fail("Введен timetableCellId равный нулю.", null);
            }

            int rowsUpdated = await _dbContext.Set<ActualTimetableCell>().Where(e => e.TimetableCellId == timetableCellId).ExecuteUpdateAsync(e => e.SetProperty(e => e.IsCanceled, true), cancellationToken);
            if (rowsUpdated == 0)
            {
                return ServiceResult.Fail("Изменения в бд не прошли, возможно ячейки расписания с таким Id нет или она уже отменена.");
            }

            return ServiceResult.Ok("Ячейка занятия помечена как отмененная.");
        }

        public async Task<ServiceResult> MarkAsUncanceled(int timetableCellId, CancellationToken cancellationToken = default)
        {
#warning проверить
            if (timetableCellId == default)
            {
                return ServiceResult.Fail("Введен timetableCellId равный нулю.");
            }

            int rowsUpdated = await _dbContext.Set<ActualTimetableCell>().Where(e => e.TimetableCellId == timetableCellId).ExecuteUpdateAsync(e => e.SetProperty(e => e.IsCanceled, false), cancellationToken);
            if (rowsUpdated == 0)
            {
                return ServiceResult.Fail("Изменения в бд не прошли, возможно ячейки расписания с таким Id нет или уже помечена как НЕотмеченная.");
            }

            return ServiceResult.Ok("Ячейка занятия помечена как НЕотмененная.");
        }

        public async Task<ServiceResult> UpadateCellInfo(int timetableCellId, int newTeacherId, int newCabinetId, bool markAsReplaced = true, CancellationToken cancellationToken = default)
        {
#warning проверить.
            if (timetableCellId == default)
            {
                return ServiceResult.Fail("Введен timetableCellId равный нулю.");
            }
            if (newTeacherId == default)
            {
                return ServiceResult.Fail("Введен newTeacherId равный нулю.");
            }
            if (newCabinetId == default)
            {
                return ServiceResult.Fail("Введен newCabinetId равный нулю.");
            }

            var getTimetableCellTask = _dbContext.Set<ActualTimetableCell>().SingleOrDefaultAsync(e => e.TimetableCellId == timetableCellId, cancellationToken);
            var teacherCheckTask = _dbContext.Set<Teacher>().AnyAsync(e => e.TeacherId == newTeacherId, cancellationToken);
            var cabinetCheckTask = _dbContext.Set<Cabinet>().AnyAsync(e => e.CabinetId == newCabinetId, cancellationToken);

            var timetableCell = await getTimetableCellTask;
            bool teacherExists = await teacherCheckTask;
            bool cabinetExists = await cabinetCheckTask;

            if (teacherExists is false)
            {
                return ServiceResult.Fail("Учителя с таким newTeacherId не существует.");
            }

            if (cabinetExists is false)
            {
                return ServiceResult.Fail("Учителя с таким newCabinetId не существует.");
            }

            if (timetableCell is null)
            {
                return ServiceResult.Fail("Ячейки актульного расписания с таким timetableCellId не существует.");
            }

            timetableCell.CabinetId = newCabinetId;
            timetableCell.TeacherId = newTeacherId;

            if (markAsReplaced is true)
            {
                timetableCell.IsModified = true;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ServiceResult.Ok("Ячейка актульного расписания изменена.");
        }

        public async Task<ServiceResult> MoveCell(int timetableCellId, int newLessonTimeId, bool markAsMoved = true, CancellationToken cancellationToken = default)
        {
#warning проверить
            if (timetableCellId == default)
            {
                return ServiceResult.Fail("Введен timetableCellId равный нулю.");
            }

            if (newLessonTimeId == default)
            {
                return ServiceResult.Fail("Введен newLessonTimeId равный нулю.");
            }

            var lessonTimeExists = await _dbContext.Set<LessonTime>().AnyAsync(e => e.LessonTimeId == newLessonTimeId, cancellationToken);
            if (lessonTimeExists is false)
            {
                return ServiceResult.Fail("Такого lessonTime нет в бд.");
            }

            var timetableCell = await _dbContext.Set<ActualTimetableCell>().SingleOrDefaultAsync(e => e.TimetableCellId == timetableCellId, cancellationToken);
            if (timetableCell == null)
            {
                return ServiceResult.Fail("Ячейки расписания с таким Id нет в бд.");
            }

            bool isLessonTimeIsOccupied = await _dbContext.Set<ActualTimetableCell>().AnyAsync(e => e.Date == timetableCell.Date && e.LessonTimeId == newLessonTimeId, cancellationToken);
            if (isLessonTimeIsOccupied is true)
            {
                return ServiceResult.Fail("На этот день на это место занятия уже поставлена другая ячейка.");
            }

            if (markAsMoved is true)
            {
                timetableCell.IsMoved = true;
            }
            timetableCell.LessonTimeId = newLessonTimeId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ServiceResult.Ok("Ячейка расписания перемещена.");
        }
    }
}
