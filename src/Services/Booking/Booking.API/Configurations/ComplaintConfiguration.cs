namespace Booking.API.Configurations
{
#pragma warning disable
    using Booking.API.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
    {
        public void Configure(EntityTypeBuilder<Complaint> builder)
        {
            builder.ToTable("Complaints");
            builder.HasKey(r => r.Id);
            builder.Property<DateTime>(r => r.CreationTime).HasColumnType("datetime2").HasColumnName("CreationTime").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.UtcNow);
            builder.Property<DateTime>(r => r.ModifiedDate).HasColumnType("datetime2").HasColumnName("ModifiedDate").ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.UtcNow);
            builder.Property<string>(r => r.Description).HasColumnName("Description").IsRequired();
            builder.Property<string>(r => r.ComplainType).HasColumnName("ComplaintType").IsRequired();
            builder.Property<bool>(r => r.IsResolved).HasColumnName("IsResolved").IsRequired();
            builder.HasOne(r => r.Booking).WithMany(r => r.Complaints).HasForeignKey(r => r.BookingId);
            builder.Property<string>(r => r.LevelOfSatisfaction).HasColumnName("LevelOfSatisfaction");
        }
    }
}
