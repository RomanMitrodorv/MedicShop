using CatalogApi;
using Microsoft.Extensions.Logging;
using static CatalogApi.Catalog;

namespace Catalog.API.Grpc
{
    public class CatalogService : CatalogBase
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private readonly CatalogContext _catalogContext;
        private readonly CatalogSettings _settings;

        public CatalogService(ILogger<CatalogService> logger, CatalogContext catalogContext, IOptionsSnapshot<CatalogSettings> settings)
        {
            _logger = logger;
            _catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(CatalogService));
            _settings = settings.Value;
        }


        public override async Task<PaginatedItemsResponse> GetItemsByIds(CatalogItemsRequest request, ServerCallContext context)
        {
            if (!string.IsNullOrEmpty(request.Ids))
            {
                var items = await GetItemsByIdsAsync(request.Ids);

                context.Status = !items.Any() ?
                    new Status(StatusCode.NotFound, $"ids value invalid. Must be comma-separated list of numbers") :
                    new Status(StatusCode.OK, string.Empty);

                return MapToResponse(items);
            }

            var totalItems = await _catalogContext.CatalogItems
                .LongCountAsync();

            var itemsOnPage = await _catalogContext.CatalogItems
                .OrderBy(c => c.Name)
                .Skip(request.PageSize * request.PageIndex)
                .Take(request.PageSize)
                .ToListAsync();

            itemsOnPage = ChangeUriPlaceholder(itemsOnPage);

            var model = MapToResponse(itemsOnPage, totalItems, request.PageIndex, request.PageSize);
            context.Status = new Status(StatusCode.OK, string.Empty);

            return model;
        }
        public override async Task<CatalogItemResponse> GetItemById(CatalogItemRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call CatalogService.GetItemById for product id {Id}", request.Id);
            if (request.Id <= 0)
            {
                context.Status = new Status(StatusCode.FailedPrecondition, $"Id must be > 0 (received {request.Id})");
                return null;
            }

            var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(ci => ci.Id == request.Id);
            var baseUri = _settings.PicBaseUrl;
            item.FillProductUrl(baseUri);

            if (item != null)
            {
                return new CatalogItemResponse()
                {
                    AvailableStock = item.AvailableStock,
                    Description = item.Description,
                    Id = item.Id,
                    Name = item.Name,
                    PictureFileName = item.PictureFileName,
                    PictureUri = item.PictureUri
                };
            }

            context.Status = new Status(StatusCode.NotFound, $"Product with id {request.Id} do not exist");
            return null;
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

        private PaginatedItemsResponse MapToResponse(List<CatalogItem> items)
        {
            return MapToResponse(items, items.Count, 1, items.Count);
        }


        private PaginatedItemsResponse MapToResponse(List<CatalogItem> items, long count, int pageIndex, int pageSize)
        {
            var result = new PaginatedItemsResponse()
            {
                Count = count,
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            items.ForEach(i =>
            {
                var brand = i.CatalogBrand == null
                            ? null
                            : new CatalogApi.CatalogBrand()
                            {
                                Id = i.CatalogBrand.Id,
                                Name = i.CatalogBrand.Name,
                            };
                var catalogType = i.CatalogType == null
                                    ? null
                                    : new CatalogApi.CatalogType()
                                    {
                                        Id = i.CatalogType.Id,
                                        Name = i.CatalogType.Name,
                                    };

                result.Data.Add(new CatalogItemResponse()
                {
                    AvailableStock = i.AvailableStock,
                    Description = i.Description,
                    Id = i.Id,
                    Name = i.Name,
                    PictureFileName = i.PictureFileName,
                    PictureUri = i.PictureUri,
                    CatalogBrand = brand,
                    CatalogType = catalogType,
                    Price = (double)i.Price,
                });
            });

            return result;
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
    }
}
