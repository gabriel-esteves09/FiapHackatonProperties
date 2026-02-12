namespace FHCK_Properties.Application.DTO;

public class PropertyDTO
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public decimal? TotalAreaHectares { get; set; }

}
