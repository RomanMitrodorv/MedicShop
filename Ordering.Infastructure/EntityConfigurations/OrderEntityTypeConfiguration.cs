namespace Ordering.Infastructure.EntityConfigurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders", OrderingContext.DEFAULT_SCHEMA);

        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.DomainEvents);

        builder.Property(x => x.Id)
            .UseHiLo("orderseq", OrderingContext.DEFAULT_SCHEMA);

        builder
            .OwnsOne(o => o.Address, a =>
            {
                a.Property<int>("OrderId")
                .UseHiLo("orderseq", OrderingContext.DEFAULT_SCHEMA);
                a.WithOwner();
            });

        builder
            .Property<int?>("_buyerId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("BuyerId")
            .IsRequired(false);

        builder
            .Property<DateTime>("_orderDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("OrderDate")
            .IsRequired();

        builder
            .Property<int>("_orderStatusId")
            // .HasField("_orderStatusId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("OrderStatusId")
            .IsRequired();

        builder
            .Property<int?>("_paymentMethodId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("PaymentMethodId")
            .IsRequired(false);

        builder.Property<string>("Description").IsRequired(false);

        var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));

        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne<PaymentMethod>()
            .WithMany()
            // .HasForeignKey("PaymentMethodId")
            .HasForeignKey("_paymentMethodId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Buyer>()
            .WithMany()
            .IsRequired(false)
            // .HasForeignKey("BuyerId");
            .HasForeignKey("_buyerId");

        builder.HasOne(o => o.OrderStatus)
            .WithMany()
            // .HasForeignKey("OrderStatusId");
            .HasForeignKey("_orderStatusId");

    }
}

