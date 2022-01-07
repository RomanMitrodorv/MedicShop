namespace Catalog.API.Infastructure.EntityConfiguration
{
    public class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
    {
        public void Configure(EntityTypeBuilder<CatalogBrand> builder)
        {
            builder.ToTable("CatalogBrand");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("catalog_brand_hilo").IsRequired();

            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        }
    }
}
