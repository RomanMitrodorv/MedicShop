using Catalog.API.IntegrationEvents.Events;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _catalogContext;
        private readonly CatalogSettings _settings;
        private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;
        public CatalogController(CatalogContext catalogContext, IOptionsSnapshot<CatalogSettings> settings, ICatalogIntegrationEventService catalogIntegrationEventService)
        {
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
            _catalogIntegrationEventService = catalogIntegrationEventService ?? throw new ArgumentNullException(nameof(catalogIntegrationEventService));
            _settings = settings.Value;
        }

        [HttpGet]
        [Route("items/{id:int}")]
        [ProducesResponseType(typeof(CatalogItem), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<CatalogItem>> ItemByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest();

            var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();

            var baseUri = _settings.PicBaseUrl;

            item.FillProductUrl(baseUri);

            return item;

        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(List<CatalogItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0, string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = await GetItemsByIdsAsync(ids);

                if (!items.Any())
                    return NotFound();

                return Ok(items);
            }

            var countItems = await _catalogContext.CatalogItems.LongCountAsync();

            var itemsOfPage = await _catalogContext.CatalogItems
                .OrderBy(x => x.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageIndex)
                .ToListAsync();

            itemsOfPage = ChangeUriPlaceholder(itemsOfPage);

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, countItems, itemsOfPage);

            return Ok(model);
        }

        [HttpGet]
        [Route("items/withname/{name:minlength(1)}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ItemsByNameAsync(string name, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = _catalogContext.CatalogItems.Where(x => x.Name.StartsWith(name));

            if (!root.Any())
                return NotFound();

            var countItems = await root.LongCountAsync();

            var itemsOfPage = await root.OrderBy(x => x.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageIndex)
                .ToListAsync();

            itemsOfPage = ChangeUriPlaceholder(itemsOfPage);

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, countItems, itemsOfPage);

            return Ok(model);

        }

        [HttpGet]
        [Route("catalogbrands")]
        [ProducesResponseType(typeof(List<CatalogBrand>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CatalogBrand>>> GetCatalogBrandsAsync()
        {
            return await _catalogContext.CatalogBrands.ToListAsync();
        }

        [HttpGet]
        [Route("catalogtypes")]
        [ProducesResponseType(typeof(List<CatalogType>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<CatalogType>>> GetCatalogTypesAsync()
        {
            return await _catalogContext.CatalogTypes.ToListAsync();
        }

        [HttpPost]
        [Route("items")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateItemAsync([FromBody] CatalogItem inputItem)
        {
            var item = new CatalogItem()
            {
                Name = inputItem.Name,
                Description = inputItem.Description,
                CatalogBrandId = inputItem.CatalogBrandId,
                CatalogTypeId = inputItem.CatalogTypeId,
                Price = inputItem.Price,
                PictureFileName = inputItem.PictureFileName
            };

            _catalogContext.CatalogItems.Add(item);

            await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteItemAsync(int id)
        {
            var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound();

            _catalogContext.CatalogItems.Remove(item);

            await _catalogContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("items/types/all/brand/{brandId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsByBrandIdAsync(int? brandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<CatalogItem>)_catalogContext.CatalogItems;

            if (brandId.HasValue)
                root = root.Where(x => x.CatalogBrandId == brandId);

            var countItems = await root.LongCountAsync();

            var itemsOfPage = await root.OrderBy(x => x.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageIndex)
                .ToListAsync();

            itemsOfPage = ChangeUriPlaceholder(itemsOfPage);

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, countItems, itemsOfPage);

            return Ok(model);
        }

        [HttpGet]
        [Route("items/type/{catalogTypeId}/brand/{catalogBrandId:int?}")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<CatalogItem>>> ItemsByTypeIdAndBrandIdAsync(int typeId, int? brandId, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<CatalogItem>)_catalogContext.CatalogItems;

            root = root.Where(ci => ci.CatalogTypeId == typeId);

            if (brandId.HasValue)
            {
                root = root.Where(ci => ci.CatalogBrandId == brandId);
            }

            var countItems = await root.LongCountAsync();

            var itemsOfPage = await root.OrderBy(x => x.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageIndex)
                .ToListAsync();

            itemsOfPage = ChangeUriPlaceholder(itemsOfPage);

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, countItems, itemsOfPage);

            return Ok(model);
        }

        private async Task<List<CatalogItem>> GetItemsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

            if (!numIds.All(x => x.Ok))
                return new List<CatalogItem>();

            var idsToSelect = numIds.Where(x => x.Ok).Select(q => q.Value);

            var items = await _catalogContext.CatalogItems.Where(x => idsToSelect.Contains(x.Id)).ToListAsync();

            items = ChangeUriPlaceholder(items);

            return items;
        }

        private List<CatalogItem> ChangeUriPlaceholder(List<CatalogItem> items)
        {
            var baseUri = _settings.PicBaseUrl;

            foreach (var item in items)
            {
                item.FillProductUrl(baseUri);
            }

            return items;
        }

        [HttpPut]
        [Route("items")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateItemAsync([FromBody] CatalogItem itemForUpdate)
        {
            var catalogItem = await _catalogContext.CatalogItems.SingleOrDefaultAsync(x => x.Id == itemForUpdate.Id);

            if (catalogItem == null)
                return NotFound(new {Message = $"Item with id {itemForUpdate.Id} not found"});

            var oldPrice = catalogItem.Price;

            var raiseProductPriceChangedEvent = oldPrice != itemForUpdate.Price;

            catalogItem = itemForUpdate;

            _catalogContext.CatalogItems.Update(catalogItem);

            if (raiseProductPriceChangedEvent)
            {
                var priceChangeEvent = new ProductPriceChangedIntegrationEvent(itemForUpdate.Price, oldPrice, catalogItem.Id);

                await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(priceChangeEvent);

                await _catalogIntegrationEventService.PublishByEventBusAsync(priceChangeEvent);
            }
            else
                await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = itemForUpdate.Id }, null);
        }
    }
}
