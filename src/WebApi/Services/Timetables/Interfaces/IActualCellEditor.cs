using Core.Entities.Timetables.Cells;

namespace WebApi.Services.Timetables.Interfaces
{
    public interface IActualCellEditor
    {
        /// <summary>
        /// Переключает одно из булевых значений ячейки и его же возврат.
        /// </summary>
        /// <param name="actualTimetableCellId"></param>
        /// <param name="flag"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Текущее состояние ячейки в T</returns>
        public Task<ServiceResult<bool>> SwitchCellFlag(int actualTimetableCellId, CellFlagToUpdate flag, CancellationToken cancellationToken = default);
        public Task<ServiceResult> Delete(int id, CancellationToken cancellationToken = default);
        public Task<ServiceResult> Update(ActualTimetableCell actualTimetableCell, CancellationToken cancellationToken = default);
        public Task<ServiceResult> Insert(int actualTimetableId, ActualTimetableCell actualTimetableCell, CancellationToken cancellationToken = default);


        public enum CellFlagToUpdate
        {
            IsModified = 0, IsCanceled = 1, IsMoved = 2
        }
    }
}
