using Bogus;
using FHCK_Properties.Application.DTO;


namespace FHCK_Properties.Test.Mocks.DTO;

public static class PropertyDtoMock
{
    public static PropertyDTO CreateValid()
    {
        var faker = new Faker("pt_BR");

        return new PropertyDTO
        {
            Name = faker.Company.CompanyName(),
            Address = faker.Address.FullAddress(),
            City = faker.Address.City(),
            TotalAreaHectares = faker.Random.Decimal(1, 5000)
        };
    }
}