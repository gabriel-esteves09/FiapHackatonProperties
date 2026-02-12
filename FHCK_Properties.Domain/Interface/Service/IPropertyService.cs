using FHCK_Properties.Domain.Entity;

namespace FHCK_Properties.Domain.Interface.Service
{
    public interface IPropertyService
    {
        Task<IEnumerable<Property>> GetAllAsync(string ownerId);
        Task<Property?> GetByIdAsync(Guid id);
        Task<Property> CreateAsync(Property property);
        Task<bool> UpdateAsync(Guid id, string ownerId, Property property);
        Task<bool> DeleteAsync(Guid id, string ownerId);
    }
}
    