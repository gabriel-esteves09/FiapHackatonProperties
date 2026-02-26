using Bogus;
using FHCK_Properties.Domain.Entity;

namespace FHCK_Properties.Test.Mocks.Entity;

public static class PropertyDataMock
{
    public static Property CreateValid(string? ownerId = null)
    {
        var faker = new Faker("pt_BR");

        return new Property
        {
            Id = Guid.NewGuid(),
            OwnerId = ownerId ?? Guid.NewGuid().ToString(),
            Name = faker.Company.CompanyName(),
            Address = faker.Address.FullAddress(),
            City = faker.Address.City(),
            TotalAreaHectares = faker.Random.Decimal(1, 5000),
            CreatedAt = DateTime.UtcNow,
            Plots = new List<Plot>()
        };
    }
}