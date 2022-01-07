namespace Catalog.API.Infastructure.EntityConfiguration
{
    public class CatalogTypeEntityTypeConfiguration : IEntityTypeConfiguration<CatalogType>
    {
        public void Configure(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CatalogType");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseHiLo("catalog_type_hilo").IsRequired();

            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        }
    }
}
