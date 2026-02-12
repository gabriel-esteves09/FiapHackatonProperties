using System.Security.Claims;
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
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        private string GetOwnerId()
        {
            // você emite "sub" no JWT (recomendado)
            var ownerId = User.FindFirst("sub")?.Value
                       ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(ownerId))
                throw new UnauthorizedAccessException("Token sem claim de usuário (sub).");

            return ownerId;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ownerId = GetOwnerId();
            var properties = await _propertyService.GetAllAsync(ownerId);
            return Ok(properties);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ownerId = GetOwnerId();
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null) return NotFound();
            return Ok(property);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PropertyDTO dto)
        {
            var ownerId = GetOwnerId();

            var property = new Property
            {
                OwnerId = ownerId, // <-- aqui!
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                TotalAreaHectares = dto.TotalAreaHectares
            };

            var created = await _propertyService.CreateAsync(property);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PropertyDTO dto)
        {
            var ownerId = GetOwnerId();

            var property = new Property
            {
                Name = dto.Name,
                Address = dto.Address,
                City = dto.City,
                TotalAreaHectares = dto.TotalAreaHectares
            };

            var updated = await _propertyService.UpdateAsync(id, ownerId, property);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ownerId = GetOwnerId();
            var deleted = await _propertyService.DeleteAsync(id, ownerId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
