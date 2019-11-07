namespace Booking.API.Configurations
{
#pragma warning disable
    using Booking.API.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class CheckInCheckListConfiguration : IEntityTypeConfiguration<CheckInCheckList>
    {
        public void Configure(EntityTypeBuilder<CheckInCheckList> builder)
        {
            builder.ToTable("CheckInCheckLists");
            builder.HasKey(r => r.Id);
            builder.Property<DateTime>(r => r.CreationTime).HasColumnType("datetime2").HasColumnName("CreationTime").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.UtcNow);
            builder.Property<DateTime>(r => r.ModifiedDate).HasColumnType("datetime2").HasColumnName("ModifiedDate").ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.UtcNow);
            builder.Property<bool>(r => r.KeysReceived).HasColumnName("KeysReceived");
            builder.Property<bool>(r => r.AllAmenities).HasColumnName("AllAmenities");
            builder.Property<bool>(r => r.PrintedRecepiet).HasColumnName("PrintedRecepiet");
        }
    }
}
