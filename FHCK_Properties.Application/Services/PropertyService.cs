using FHCK_Properties.Domain.Entity;
using FHCK_Properties.Domain.Interface.Service;
using FHCK_Properties.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FHCK_Properties.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly AppDbContext _context;

        public PropertyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Property>> GetAllAsync(string ownerId)
        {
            return await _context.Property
                .AsNoTracking()
                .Include(p => p.Plots)
                .Where(p => p.OwnerId == ownerId)
                .ToListAsync();
        }

        public async Task<Property?> GetByIdAsync(Guid id)
        {
            return await _context.Property
                .AsNoTracking()
                .Include(p => p.Plots)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Property> CreateAsync(Property property)
        {
            property.Id = Guid.NewGuid();
            property.CreatedAt = DateTime.UtcNow;

            _context.Property.Add(property);
            await _context.SaveChangesAsync();

            return property;
        }

        public async Task<bool> UpdateAsync(Guid id, string ownerId, Property property)
        {
            var existing = await _context.Property
                .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == ownerId);

            if (existing == null) return false;

            existing.Name = property.Name;
            existing.Address = property.Address;
            existing.City = property.City;
            existing.TotalAreaHectares = property.TotalAreaHectares;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, string ownerId)
        {
            var existing = await _context.Property
                .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == ownerId);

            if (existing == null) return false;

            _context.Property.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
