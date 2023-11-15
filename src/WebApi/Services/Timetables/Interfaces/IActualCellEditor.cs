using Models.Entities.Timetables.Cells;

namespace WebApi.Services.Timetables.Interfaces
{
    public interface IActualCellEditor
    {
        public Task<ServiceResult<bool>> SwitchCellFlag(int actualTimetableCellId, CellFlagToUpdate flag, CancellationToken cancellationToken = default);
        public Task<ServiceResult> Delete(int id, CancellationToken cancellationToken = default);
        public Task<ServiceResult> InsertUpdate(ActualTimetableCell actualTimetableCell, CancellationToken cancellationToken = default);


        public enum CellFlagToUpdate
        {
            IsModified = 0, IsCanceled = 1, IsMoved = 2
        }
    }
}
