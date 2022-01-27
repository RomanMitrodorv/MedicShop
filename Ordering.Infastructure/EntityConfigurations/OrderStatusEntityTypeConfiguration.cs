namespace Ordering.Infastructure.EntityConfigurations;

public class OrderStatusEntityTypeConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.ToTable("orderstatus", OrderingContext.DEFAULT_SCHEMA);

        builder.HasKey(builder => builder.Id);

        builder.Property(o => o.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(o => o.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}

