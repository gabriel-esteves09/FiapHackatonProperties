using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FHCK_Properties.Domain.Entity;

public class Property
{
    [Key]
    public Guid Id { get; set; }
    public string OwnerId { get; set; } = default!;
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = null!;
    [Required]
    [StringLength(400)]
    public string Address { get; set; } = null!;
    [Required]
    [StringLength(100)]
    public string City { get; set; } = null!;
    public decimal? TotalAreaHectares { get; set; }
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ICollection<Plot>? Plots { get; set; }

}
