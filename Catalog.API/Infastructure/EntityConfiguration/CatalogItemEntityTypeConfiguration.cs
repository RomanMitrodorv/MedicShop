namespace Catalog.API.Infastructure.EntityConfiguration
{
    public class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("Catalog");

            builder.Property(x => x.Id).UseHiLo("catalog_item_hilo").IsRequired();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal(16, 2)");

            builder.Property(x => x.PictureFileName).IsRequired(false);

            builder.Ignore(x => x.PictureUri);

            builder.HasOne(x => x.CatalogBrand).WithMany().HasForeignKey(q => q.CatalogBrandId);

            builder.HasOne(x => x.CatalogType).WithMany().HasForeignKey(q => q.CatalogTypeId);
        }
    }
}
