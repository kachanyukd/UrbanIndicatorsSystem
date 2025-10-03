using UrbanIndicatorsSystem.Models;
using UrbanIndicatorsSystem.Services;
using Xunit;

namespace UrbanIndicatorsSystem.Tests
{
    public class AreaServiceTests
    {
        [Fact]
        public void GetAreas_ReturnsListWithSixAreas()
        {
            // Arrange
            var service = new AreaService();

            // Act
            var result = service.GetAreas();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public void GetAreas_ReturnsCorrectAreaData()
        {
            // Arrange
            var service = new AreaService();

            // Act
            var result = service.GetAreas();

            // Assert
            Assert.Contains(result, a => a.Id == 1 && a.Name == "Pechersk");
            Assert.Contains(result, a => a.Id == 2 && a.Name == "Shevchenkivskyi");
            Assert.Contains(result, a => a.Id == 3 && a.Name == "Holosiivskyi");
            Assert.Contains(result, a => a.Id == 4 && a.Name == "Solomyanskiy");
            Assert.Contains(result, a => a.Id == 5 && a.Name == "Darnytskiy");
            Assert.Contains(result, a => a.Id == 6 && a.Name == "Obolonskiy");
        }

        [Fact]
        public void GetAreas_ReturnsAreasWithUniqueIds()
        {
            // Arrange
            var service = new AreaService();

            // Act
            var result = service.GetAreas();

            // Assert
            var ids = result.Select(a => a.Id).ToList();
            Assert.Equal(ids.Count, ids.Distinct().Count());
        }
    }
}