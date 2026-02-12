using FHCK_Properties.Domain.Entity;
using FHCK_Properties.Domain.Interface.Service;
using FHCK_Properties.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
    
namespace FHCK_Properties.Application.Services
{
    public class PlotService : IPlotService
    {
        private readonly AppDbContext _context;

        public PlotService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plot>> GetAllAsync()
        {
            return await _context.Plot
                .AsNoTracking()
                .Include(p => p.Property)
                .ToListAsync();
        }

        public async Task<Plot?> GetByIdAsync(Guid id)
        {
            return await _context.Plot
                .AsNoTracking()
                .Include(p => p.Property)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Plot> CreateAsync(Plot plot)
        {
            plot.Id = Guid.NewGuid();
            plot.CreatedAt = DateTime.UtcNow;

            _context.Plot.Add(plot);
            await _context.SaveChangesAsync();

            return plot;
        }

        public async Task<bool> UpdateAsync(Guid id, Plot plot)
        {
            var existing = await _context.Plot.FindAsync(id);
            if (existing == null) return false;

            existing.Name = plot.Name;
            existing.AreaHectares = plot.AreaHectares;
            existing.CropType = plot.CropType;
            existing.PropertyId = plot.PropertyId;

            _context.Plot.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Plot.FindAsync(id);
            if (existing == null) return false;

            _context.Plot.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
