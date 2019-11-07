namespace Booking.API.Configurations
{
    using Booking.API.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    /// <summary>
    /// Defines the <see cref="InvoiceConfiguration" />
    /// </summary>
    public class LineConfiguration : IEntityTypeConfiguration<Line>
    {
        /// <summary>
        /// The Configure
        /// </summary>
        /// <param name="builder">The builder<see cref="EntityTypeBuilder{Invoice}"/></param>
        public void Configure(EntityTypeBuilder<Line> builder)
        {
            builder.ToTable("Lines");
            builder.HasKey(x => x.Id);
            builder.Property<decimal>(x => x.Line_Price).HasColumnName("Line_Price").IsRequired().HasColumnType("money");
            builder.Property<int>(x => x.Line_Units).HasColumnName("Line_Units").IsRequired().HasDefaultValue(value: 0);
            builder.Property<string>(x => x.Room_Type).IsRequired();
            builder.Property<DateTime>(r => r.CreationTime).HasColumnType("datetime2").HasColumnName("CreationTime").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.UtcNow);
            builder.Property<DateTime>(r => r.ModifiedDate).HasColumnType("datetime2").HasColumnName("ModifiedDate").ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.UtcNow);
            builder.HasOne(x => x.Invoice).WithMany(x => x.Lines).HasForeignKey(x => x.InvoiceId);
        }
    }
}
