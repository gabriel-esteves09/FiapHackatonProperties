using Bogus;
using FHCK_Properties.Domain.Entity;

namespace FHCK_Properties.Test.Mocks.Entity;

public static class PlotDataMock
{
    public static Plot CreateValid(Guid propertyId)
    {
        var faker = new Faker("pt_BR");

        return new Plot
        {
            Id = Guid.NewGuid(),
            PropertyId = propertyId,
            Name = $"Talhão {faker.Random.Number(1, 999)}",
            AreaHectares = faker.Random.Decimal(0.5m, 300m),
            CropType = faker.PickRandom(new[] { "Soja", "Milho", "Cana", "Algodão" }),
            CreatedAt = DateTime.UtcNow
        };
    }
}