namespace FHCK_Properties.Application.DTO;

public class PlotDTO
{
    public Guid PropertyId { get; set; }
    public string Name { get; set; } = null!;
    public decimal AreaHectares { get; set; }
    public string CropType { get; set; } = null!;
}
