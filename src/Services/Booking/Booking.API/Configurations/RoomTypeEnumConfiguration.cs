namespace Booking.API.Configurations
{
#pragma warning disable
    using Booking.API.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public class RoomTypeEnumConfiguration : IEntityTypeConfiguration<RoomTypeEnum>
    {
        public void Configure(EntityTypeBuilder<RoomTypeEnum> builder)
        {
            builder.ToTable("RoomTypeEnums");
            builder.HasKey(x => x.Id);
            builder.Property<DateTime>(r => r.CreationTime).HasColumnType("datetime2").HasColumnName("CreationTime").IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.UtcNow);
            builder.Property<DateTime>(r => r.ModifiedDate).HasColumnType("datetime2").HasColumnName("ModifiedDate").ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.UtcNow);
            builder.Property<decimal>(x => x.Cost).HasColumnName("Cost").HasColumnType("money");
            builder.Property<int>(x => x.Capacity).HasColumnName("Capacity").IsRequired();
            builder.Property<string>(x => x.ImageName).HasColumnName("ImageName").IsRequired();
            builder.Property<string>(r => r.ImageID).HasColumnName("ImageID").IsRequired();
            builder.HasIndex(x => x.Type).IsUnique();
        }
    }
}
