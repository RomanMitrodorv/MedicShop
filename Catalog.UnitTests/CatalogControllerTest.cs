using Catalog.API;
using Catalog.API.Controllers;
using Catalog.API.Infastructure;
using Catalog.API.IntegrationEvents;
using Catalog.API.Model;
using Catalog.API.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.UnitTests;

public class CatalogControllerTest
{
    private readonly DbContextOptions<CatalogContext> _dbOptions;

    public CatalogControllerTest()
    {
        _dbOptions = new DbContextOptionsBuilder<CatalogContext>()
            .UseInMemoryDatabase("in-memory")
            .Options;

        using var dbContext = new CatalogContext(_dbOptions);

    }

    [Fact]
    public async Task Get_catalog_items_success()
    {
        var brandFilterApplied = 1;
        var typesFilterApplied = 2;

        var pageSize = 4;
        var pageIndex = 1;

        var expectedItemsInPage = 2;
        var expectedTotalItems = 6;

        var catalogContext = new CatalogContext(_dbOptions);

        catalogContext.AddRange(GetFakeCatalog());
        catalogContext.SaveChanges();

        var catalogSettings = new TestCatalogSettings();

        var integrationServiceMock = new Mock<ICatalogIntegrationEventService>();

        var catalogController = new CatalogController(catalogContext, catalogSettings, integrationServiceMock.Object);

        var actionResult = await catalogController.ItemsByTypeIdAndBrandIdAsync(typesFilterApplied, brandFilterApplied, pageSize, pageIndex);

        Assert.IsType<ActionResult<PaginatedItemsViewModel<CatalogItem>>>(actionResult);

        var page = Assert.IsAssignableFrom<PaginatedItemsViewModel<CatalogItem>>(actionResult.Value);

        Assert.Equal(expectedTotalItems, page.Count);
        Assert.Equal(pageIndex, page.PageIndex);
        Assert.Equal(pageSize, page.PageSize);

        Assert.Equal(expectedItemsInPage, page.Data.Count());
    }

    private List<CatalogItem> GetFakeCatalog()
    {
        return new List<CatalogItem>()
        {
            new CatalogItem()
            {
                Id = 1,
                Name = "fakeItemA",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemA.png"
            },
            new CatalogItem()
            {
                Id = 2,
                Name = "fakeItemB",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemB.png"
            },
            new CatalogItem()
            {
                Id = 3,
                Name = "fakeItemC",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemC.png"
            },
            new CatalogItem()
            {
                Id = 4,
                Name = "fakeItemD",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemD.png"
            },
            new CatalogItem()
            {
                Id = 5,
                Name = "fakeItemE",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemE.png"
            },
            new CatalogItem()
            {
                Id = 6,
                Name = "fakeItemF",
                CatalogTypeId = 2,
                CatalogBrandId = 1,
                PictureFileName = "fakeItemF.png"
            }
        };
    }
}



public class TestCatalogSettings : IOptionsSnapshot<CatalogSettings>
{
    public CatalogSettings Value => new CatalogSettings
    {
        PicBaseUrl = "http://image-server.com/"
    };

    public CatalogSettings Get(string name) => Value;
}