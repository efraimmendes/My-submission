using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossSolar.Controllers;
using CrossSolar.Domain;
using CrossSolar.Models;
using CrossSolar.Repository;
using CrossSolar.Tests.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CrossSolar.Tests.Controller
{
    public class AnalyticsControllerTests
    {
        public AnalyticsControllerTests()
        {  }

        [Fact]
        public async Task Register_ShouldGetPanel()
        {
            Mock<DbSet<Panel>> mockPanel = MockPanelRepository();

            Mock<DbSet<OneHourElectricity>> mockOneHourElectricity = MockAnaliticsRepository();

            var mockContext = new Mock<CrossSolarDbContext>();
            mockContext.Setup(c => c.Set<Panel>()).Returns(mockPanel.Object);
            mockContext.Setup(c => c.Set<OneHourElectricity>()).Returns(mockOneHourElectricity.Object);

            var entityRepository = new PanelRepository(mockContext.Object);
            var analyticsRepository = new AnalyticsRepository(mockContext.Object);

            var _analyticsController = new AnalyticsController(analyticsRepository, entityRepository);

            // Act
            var result = await _analyticsController.Get("AAAA1111BBBB2222");

            // Assert
            Assert.NotNull(result);
            var createdResult = result as OkObjectResult;
            Assert.NotNull(createdResult);
            Assert.Equal(200, createdResult.StatusCode);
        }

        [Fact]
        public async Task Register_ShouldGetDayResult()
        {
            Mock<DbSet<Panel>> mockPanel = MockPanelRepository();
            Mock<DbSet<OneHourElectricity>> mockOneHourElectricity = MockDayResult();

            var mockContext = new Mock<CrossSolarDbContext>();
            mockContext.Setup(c => c.Set<Panel>()).Returns(mockPanel.Object);
            mockContext.Setup(c => c.Set<OneHourElectricity>()).Returns(mockOneHourElectricity.Object);

            var entityRepository = new PanelRepository(mockContext.Object);
            var analyticsRepository = new AnalyticsRepository(mockContext.Object);

            var _analyticsController = new AnalyticsController(analyticsRepository, entityRepository);

            // Act
            var result = await _analyticsController.DayResults("AAAA1111BBBB2222");

            // Assert
            Assert.NotNull(result);
            var createdResult = result as OkObjectResult;
            Assert.NotNull(createdResult);
            Assert.Equal(200, createdResult.StatusCode);

        }

        [Fact]
        public async Task Register_ShouldInsertAnalytics()
        {
            var oneHourElectricityContent = new OneHourElectricityModel
            {
                Id = 1,
                DateTime = new DateTime(2018, 1, 1),
                KiloWatt = 1500
            };

           var _panelRepositoryMock = new Mock<IPanelRepository>();
            var _analiticsRepositoryMock = new Mock<IAnalyticsRepository>();

            var _analiticsController = new AnalyticsController(_analiticsRepositoryMock.Object, _panelRepositoryMock.Object);

            // Arrange

            // Act
            var result = await _analiticsController.Post("AAAA1111BBBB2222",oneHourElectricityContent);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }


        private static Mock<DbSet<Panel>> MockPanelRepository()
        {
            var panel = new List<Panel>() { new Panel
                {
                    Brand = "Areva",
                    Latitude = 12.345678,
                    Longitude = 98.7655432,
                    Serial = "AAAA1111BBBB2222"
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Panel>>();

            mockSet.As<IAsyncEnumerable<Panel>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<Panel>(panel.GetEnumerator()));

            mockSet.As<IQueryable<Panel>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Panel>(panel.Provider));

            mockSet.As<IQueryable<Panel>>().Setup(m => m.Expression).Returns(panel.Expression);
            mockSet.As<IQueryable<Panel>>().Setup(m => m.ElementType).Returns(panel.ElementType);
            mockSet.As<IQueryable<Panel>>().Setup(m => m.GetEnumerator()).Returns(() => panel.GetEnumerator());
            return mockSet;
        }

        private static Mock<DbSet<OneHourElectricity>> MockAnaliticsRepository()
        {
            var oneHourElectricity = new List<OneHourElectricity>() { new OneHourElectricity
                {
                    Id=1,
                    PanelId = "AAAA1111BBBB2222",
                    DateTime = new DateTime(2018,1,1),
                    KiloWatt = 1500
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<OneHourElectricity>>();

            mockSet.As<IAsyncEnumerable<OneHourElectricity>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<OneHourElectricity>(oneHourElectricity.GetEnumerator()));

            mockSet.As<IQueryable<Panel>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<OneHourElectricity>(oneHourElectricity.Provider));

            mockSet.As<IQueryable<OneHourElectricity>>().Setup(m => m.Expression).Returns(oneHourElectricity.Expression);
            mockSet.As<IQueryable<OneHourElectricity>>().Setup(m => m.ElementType).Returns(oneHourElectricity.ElementType);
            mockSet.As<IQueryable<OneHourElectricity>>().Setup(m => m.GetEnumerator()).Returns(() => oneHourElectricity.GetEnumerator());
            return mockSet;
        }

        private static Mock<DbSet<OneHourElectricity>> MockDayResult()
        {
            var oneHourElectricity = new List<OneHourElectricity>() {
                new OneHourElectricity
                {
                    Id=1,
                    PanelId = "AAAA1111BBBB2222",
                    DateTime = new DateTime(2018,1,1),
                    KiloWatt = 1500
                },
                new OneHourElectricity
                {
                    Id=2,
                    PanelId = "AAAA1111BBBB2222",
                    DateTime = new DateTime(2018,1,1),
                    KiloWatt = 92
                },
                new OneHourElectricity
                {
                    Id=3,
                    PanelId = "AAAA1111BBBB2222",
                    DateTime = new DateTime(2018,1,1),
                    KiloWatt = 340
                },
                new OneHourElectricity
                {
                    Id=4,
                    PanelId = "AAAA1111BBBB2222",
                    DateTime = new DateTime(2018,1,2),
                    KiloWatt = 1500
                },
                new OneHourElectricity
                {
                    Id=5,
                    PanelId = "AAAA1111BBBB2222",
                    DateTime = new DateTime(2018,1,2),
                    KiloWatt = 92
                },
                new OneHourElectricity
                {
                    Id=6,
                    PanelId = "AAAA1111BBBB2222",
                    DateTime = new DateTime(2018,1,2),
                    KiloWatt = 340
                }

            }.AsQueryable();

            var mockSet = new Mock<DbSet<OneHourElectricity>>();

            mockSet.As<IAsyncEnumerable<OneHourElectricity>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<OneHourElectricity>(oneHourElectricity.GetEnumerator()));

            mockSet.As<IQueryable<Panel>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<OneHourElectricity>(oneHourElectricity.Provider));

            mockSet.As<IQueryable<OneHourElectricity>>().Setup(m => m.Expression).Returns(oneHourElectricity.Expression);
            mockSet.As<IQueryable<OneHourElectricity>>().Setup(m => m.ElementType).Returns(oneHourElectricity.ElementType);
            mockSet.As<IQueryable<OneHourElectricity>>().Setup(m => m.GetEnumerator()).Returns(() => oneHourElectricity.GetEnumerator());
            return mockSet;
        }

    }


}
