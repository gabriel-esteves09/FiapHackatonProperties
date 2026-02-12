using FHCK_Properties.Domain.Entity;

namespace FHCK_Properties.Domain.Interface.Service
{
    public interface IPlotService
    {
        Task<IEnumerable<Plot>> GetAllAsync();
        Task<Plot?> GetByIdAsync(Guid id);
        Task<Plot> CreateAsync(Plot plot);
        Task<bool> UpdateAsync(Guid id, Plot plot);
        Task<bool> DeleteAsync(Guid id);
    }
}
