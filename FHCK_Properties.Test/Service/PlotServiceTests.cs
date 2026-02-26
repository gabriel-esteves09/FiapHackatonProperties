using FHCK_Properties.Application.Services;
using FHCK_Properties.Domain.Entity;
using FHCK_Properties.Infrastructure.Context;
using FHCK_Properties.Test.Mocks.Entity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FHCK_Properties.Test.Service;

[TestFixture]
public class PlotServiceTests
{
    #region Private Fields

    private AppDbContext _context = null!;
    private PlotService _service = null!;

    #endregion

    #region Setup / TearDown

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: $"FHCK_Properties_Plot_{Guid.NewGuid()}")
            .Options;

        _context = new AppDbContext(options);
        _service = new PlotService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    #endregion

    #region GetAllAsync

    [Test, Category("Success")]
    public async Task GetAllAsync_ShouldReturnPlots_WithPropertyIncluded()
    {
        // Arrange
        var property = PropertyDataMock.CreateValid("owner-a");
        _context.Property.Add(property);

        var plot1 = PlotDataMock.CreateValid(property.Id);
        var plot2 = PlotDataMock.CreateValid(property.Id);

        _context.Plot.AddRange(plot1, plot2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));

        // Include(p => p.Property)
        Assert.That(result.All(p => p.Property is not null), Is.True);
        Assert.That(result.All(p => p.Property!.Id == property.Id), Is.True);
    }

    #endregion

    #region GetByIdAsync

    [Test, Category("Success")]
    public async Task GetByIdAsync_ShouldReturnPlot_WhenExists_WithProperty()
    {
        // Arrange
        var property = PropertyDataMock.CreateValid("owner-a");
        _context.Property.Add(property);

        var plot = PlotDataMock.CreateValid(property.Id);
        _context.Plot.Add(plot);

        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetByIdAsync(plot.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(plot.Id));
        Assert.That(result.Property, Is.Not.Null);
        Assert.That(result.Property!.Id, Is.EqualTo(property.Id));
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
    public async Task CreateAsync_ShouldCreatePlot_WithNewId_AndCreatedAt()
    {
        // Arrange
        var property = PropertyDataMock.CreateValid("owner-a");
        _context.Property.Add(property);
        await _context.SaveChangesAsync();

        var plot = PlotDataMock.CreateValid(property.Id);
        plot.Id = Guid.Empty;
        plot.CreatedAt = default;

        // Act
        var created = await _service.CreateAsync(plot);

        // Assert
        Assert.That(created.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(created.CreatedAt, Is.Not.EqualTo(default(DateTime)));

        var fromDb = await _context.Plot.FindAsync(created.Id);
        Assert.That(fromDb, Is.Not.Null);
    }

    #endregion

    #region UpdateAsync

    [Test, Category("Success")]
    public async Task UpdateAsync_ShouldReturnTrue_WhenPlotExists()
    {
        // Arrange
        var property = PropertyDataMock.CreateValid("owner-a");
        _context.Property.Add(property);

        var existing = PlotDataMock.CreateValid(property.Id);
        _context.Plot.Add(existing);

        await _context.SaveChangesAsync();

        var update = new Plot
        {
            Name = "Talhão Atualizado",
            AreaHectares = 123.45m,
            CropType = "Soja",
            PropertyId = property.Id
        };

        // Act
        var ok = await _service.UpdateAsync(existing.Id, update);

        // Assert
        Assert.That(ok, Is.True);

        var fromDb = await _context.Plot.FirstAsync(p => p.Id == existing.Id);
        Assert.That(fromDb.Name, Is.EqualTo("Talhão Atualizado"));
        Assert.That(fromDb.AreaHectares, Is.EqualTo(123.45m));
        Assert.That(fromDb.CropType, Is.EqualTo("Soja"));
        Assert.That(fromDb.PropertyId, Is.EqualTo(property.Id));
    }

    [Test, Category("Success")]
    public async Task UpdateAsync_ShouldReturnFalse_WhenPlotDoesNotExist()
    {
        // Act
        var ok = await _service.UpdateAsync(Guid.NewGuid(), new Plot
        {
            Name = "X",
            AreaHectares = 1,
            CropType = "Milho",
            PropertyId = Guid.NewGuid()
        });

        // Assert
        Assert.That(ok, Is.False);
    }

    #endregion

    #region DeleteAsync

    [Test, Category("Success")]
    public async Task DeleteAsync_ShouldReturnTrue_WhenPlotExists()
    {
        // Arrange
        var property = PropertyDataMock.CreateValid("owner-a");
        _context.Property.Add(property);

        var existing = PlotDataMock.CreateValid(property.Id);
        _context.Plot.Add(existing);

        await _context.SaveChangesAsync();

        // Act
        var ok = await _service.DeleteAsync(existing.Id);

        // Assert
        Assert.That(ok, Is.True);

        var fromDb = await _context.Plot.FindAsync(existing.Id);
        Assert.That(fromDb, Is.Null);
    }

    [Test, Category("Success")]
    public async Task DeleteAsync_ShouldReturnFalse_WhenPlotDoesNotExist()
    {
        // Act
        var ok = await _service.DeleteAsync(Guid.NewGuid());

        // Assert
        Assert.That(ok, Is.False);
    }

    #endregion
}