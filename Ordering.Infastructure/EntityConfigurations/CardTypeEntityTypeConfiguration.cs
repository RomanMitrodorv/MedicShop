namespace Ordering.Infastructure.EntityConfigurations;

public class CardTypeEntityTypeConfiguration : IEntityTypeConfiguration<CardType>
{
    public void Configure(EntityTypeBuilder<CardType> builder)
    {
        builder.ToTable("cardtype", OrderingContext.DEFAULT_SCHEMA);

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

