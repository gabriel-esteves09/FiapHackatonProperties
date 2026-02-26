using FHCK_Properties.Application.Services;
using FHCK_Properties.Domain.Entity;
using FHCK_Properties.Infrastructure.Context;
using FHCK_Properties.Test.Mocks.Entity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FHCK_Properties.Test.Service;

[TestFixture]
public class PropertyServiceTests
{
    #region Private Fields

    private AppDbContext _context = null!;
    private PropertyService _service = null!;

    #endregion

    #region Setup / TearDown

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"FHCK_Properties_Property_{Guid.NewGuid()}")
            .Options;

        _context = new AppDbContext(options);
        _service = new PropertyService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    #endregion

    #region GetAllAsync

    [Test, Category("Success")]
    public async Task GetAllAsync_ShouldReturnOnlyOwnerProperties()
    {
        // Arrange
        var ownerA = "owner-a";
        var ownerB = "owner-b";

        var p1 = PropertyDataMock.CreateValid(ownerA);
        var p2 = PropertyDataMock.CreateValid(ownerA);
        var p3 = PropertyDataMock.CreateValid(ownerB);

        _context.Property.AddRange(p1, p2, p3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllAsync(ownerA);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.All(x => x.OwnerId == ownerA), Is.True);
    }

    #endregion

    #region GetByIdAsync

    [Test, Category("Success")]
    public async Task GetByIdAsync_ShouldReturnProperty_WhenExists()
    {
        // Arrange
        var property = PropertyDataMock.CreateValid("owner-a");
        _context.Property.Add(property);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetByIdAsync(property.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(property.Id));
    }

    [Test, Category("Success")]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var result = await _service.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.Null);
    }

    #endregion

    #region CreateAsync

    [Test, Category("Success")]
    public async Task CreateAsync_ShouldCreateProperty_WithNewId_AndCreatedAt()
    {
        // Arrange
        var property = PropertyDataMock.CreateValid("owner-a");
        property.Id = Guid.Empty;
        property.CreatedAt = default;

        // Act
        var created = await _service.CreateAsync(property);

        // Assert
        Assert.That(created.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(created.CreatedAt, Is.Not.EqualTo(default(DateTime)));

        var fromDb = await _context.Property.FindAsync(created.Id);
        Assert.That(fromDb, Is.Not.Null);
    }

    #endregion

    #region UpdateAsync

    [Test, Category("Success")]
    public async Task UpdateAsync_ShouldReturnTrue_WhenOwnerMatches_AndPropertyExists()
    {
        // Arrange
        var ownerId = "owner-a";
        var existing = PropertyDataMock.CreateValid(ownerId);
        _context.Property.Add(existing);
        await _context.SaveChangesAsync();

        var updated = new Property
        {
            Name = "Fazenda Atualizada",
            Address = "Rua Nova, 123",
            City = "Cidade Nova",
            TotalAreaHectares = 999
        };

        // Act
        var ok = await _service.UpdateAsync(existing.Id, ownerId, updated);

        // Assert
        Assert.That(ok, Is.True);

        var fromDb = await _context.Property.FirstAsync(p => p.Id == existing.Id);
        Assert.That(fromDb.Name, Is.EqualTo("Fazenda Atualizada"));
        Assert.That(fromDb.Address, Is.EqualTo("Rua Nova, 123"));
        Assert.That(fromDb.City, Is.EqualTo("Cidade Nova"));
        Assert.That(fromDb.TotalAreaHectares, Is.EqualTo(999));
    }

    [Test, Category("Success")]
    public async Task UpdateAsync_ShouldReturnFalse_WhenOwnerDoesNotMatch()
    {
        // Arrange
        var existing = PropertyDataMock.CreateValid("owner-a");
        _context.Property.Add(existing);
        await _context.SaveChangesAsync();

        // Act
        var ok = await _service.UpdateAsync(existing.Id, "owner-b", new Property
        {
            Name = "X",
            Address = "Y",
            City = "Z",
            TotalAreaHectares = 1
        });

        // Assert
        Assert.That(ok, Is.False);
    }

    [Test, Category("Success")]
    public async Task UpdateAsync_ShouldReturnFalse_WhenPropertyDoesNotExist()
    {
        // Act
        var ok = await _service.UpdateAsync(Guid.NewGuid(), "owner-a", new Property
        {
            Name = "X",
            Address = "Y",
            City = "Z",
            TotalAreaHectares = 1
        });

        // Assert
        Assert.That(ok, Is.False);
    }

    #endregion

    #region DeleteAsync

    [Test, Category("Success")]
    public async Task DeleteAsync_ShouldReturnTrue_WhenOwnerMatches_AndPropertyExists()
    {
        // Arrange
        var ownerId = "owner-a";
        var existing = PropertyDataMock.CreateValid(ownerId);
        _context.Property.Add(existing);
        await _context.SaveChangesAsync();

        // Act
        var ok = await _service.DeleteAsync(existing.Id, ownerId);

        // Assert
        Assert.That(ok, Is.True);

        var fromDb = await _context.Property.FindAsync(existing.Id);
        Assert.That(fromDb, Is.Null);
    }

    [Test, Category("Success")]
    public async Task DeleteAsync_ShouldReturnFalse_WhenOwnerDoesNotMatch()
    {
        // Arrange
        var existing = PropertyDataMock.CreateValid("owner-a");
        _context.Property.Add(existing);
        await _context.SaveChangesAsync();

        // Act
        var ok = await _service.DeleteAsync(existing.Id, "owner-b");

        // Assert
        Assert.That(ok, Is.False);
    }

    [Test, Category("Success")]
    public async Task DeleteAsync_ShouldReturnFalse_WhenPropertyDoesNotExist()
    {
        // Act
        var ok = await _service.DeleteAsync(Guid.NewGuid(), "owner-a");

        // Assert
        Assert.That(ok, Is.False);
    }

    #endregion
}