using System;
using System.Threading.Tasks;
using FHCK_Properties.Application.DTO;
using FHCK_Properties.Domain.Entity;
using FHCK_Properties.Domain.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FHCK_Properties.API.Controllers
{
    [Route("api/[controller]")]         
    [ApiController]
    [Authorize]
    public class PlotController : ControllerBase
    {
        private readonly IPlotService _plotService;
        private readonly IPropertyService _propertyService;

        public PlotController(IPlotService plotService, IPropertyService propertyService)
        {
            _plotService = plotService;
            _propertyService = propertyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var plots = await _plotService.GetAllAsync();
            return Ok(plots);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var plot = await _plotService.GetByIdAsync(id);
            if (plot == null) return NotFound();
            return Ok(plot);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlotDTO dto)
        {
            // valida propriedade pai
            var prop = await _propertyService.GetByIdAsync(dto.PropertyId);
            if (prop == null) return BadRequest($"Property with id {dto.PropertyId} not found.");

            var plot = new Plot
            {
                PropertyId = dto.PropertyId,
                Name = dto.Name,
                AreaHectares = dto.AreaHectares,
                CropType = dto.CropType
            };

            var created = await _plotService.CreateAsync(plot);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PlotDTO dto)
        {
            var plot = new Plot
            {
                PropertyId = dto.PropertyId,
                Name = dto.Name,
                AreaHectares = dto.AreaHectares,
                CropType = dto.CropType
            };

            var updated = await _plotService.UpdateAsync(id, plot);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _plotService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
