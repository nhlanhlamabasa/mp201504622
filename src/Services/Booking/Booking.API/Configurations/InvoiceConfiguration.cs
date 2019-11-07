namespace Booking.API.Configurations
{
    using Booking.API.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    /// <summary>
    /// Defines the <see cref="InvoiceConfiguration" />
    /// </summary>
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        /// <summary>
        /// The Configure
        /// </summary>
        /// <param name="builder">The builder<see cref="EntityTypeBuilder{Invoice}"/></param>
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");
            builder.HasKey(x => x.Id);
            builder.Property<Guid>(x => x.AppUser_BookerId).HasColumnName("AppUser_BookerId").IsRequired();
            builder.Property<DateTime>(x => x.Inv_Date).HasColumnType("datetime2").HasColumnName("Inv_Date").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.Now);
            builder.Property<DateTime>(r => r.CreationTime).HasColumnType("datetime2").HasColumnName("CreationTime").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.UtcNow);
            builder.Property<DateTime>(r => r.ModifiedDate).HasColumnType("datetime2").HasColumnName("ModifiedDate").ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.UtcNow);
            builder.HasMany(x => x.Lines).WithOne(x => x.Invoice).HasForeignKey(r => r.InvoiceId);
        }
    }
}
