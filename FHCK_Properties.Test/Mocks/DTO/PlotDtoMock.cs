using Bogus;
using FHCK_Properties.Application.DTO;

namespace FHCK_Properties.Test.Mocks.DTO;

public static class PlotDtoMock
{
    public static PlotDTO CreateValid(Guid propertyId)
    {
        var faker = new Faker("pt_BR");

        return new PlotDTO
        {
            PropertyId = propertyId,
            Name = $"Talhão {faker.Random.Number(1, 999)}",
            AreaHectares = faker.Random.Decimal(0.5m, 300m),
            CropType = faker.PickRandom(new[] { "Soja", "Milho", "Cana", "Algodão" })
        };
    }
}