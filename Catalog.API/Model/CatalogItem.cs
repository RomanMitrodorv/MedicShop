
namespace Catalog.API.Model
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUri { get; set; }
        public string PictureFileName { get; set; }
        public int CatalogTypeId { get; set; }
        public CatalogType CatalogType { get; set; }
        public int CatalogBrandId { get; set; }
        public CatalogBrand CatalogBrand { get; set; }
        public int AvailableStock { get; set; }

        public int RemoveStock(int quantity)
        {
            if(AvailableStock == 0)
                throw new CatalogDomainException($"Empty stock, product item {Name} is sold out");

            if (quantity <= 0)
                throw new CatalogDomainException("Item units desired should be greater than zero");

            int removed = Math.Min(quantity, AvailableStock);

            AvailableStock -= removed;

            return removed;
        }

        public int AddStock(int quantity)
        {
            int origin = AvailableStock;

            AvailableStock += quantity;

            return AvailableStock - origin;
        }
    }
}
