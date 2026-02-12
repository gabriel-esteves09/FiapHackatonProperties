using System.ComponentModel.DataAnnotations;

namespace FHCK_Properties.Domain.Entity;

public class Plot
{
    [Key]
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public decimal AreaHectares { get; set; }
    public string CropType { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Property? Property { get; set; }
}
