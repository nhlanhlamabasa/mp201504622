using IdentityServer.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.API.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(h => h.PostalCode).IsRequired();//Contact Details
            builder.Property(p => p.Country).HasColumnName("Country").IsRequired();//Address
            builder.Property(p => p.City).HasColumnName("City").IsRequired();
            builder.Property(p => p.Province).HasColumnName("Province").IsRequired();
            builder.Property(p => p.Street).HasColumnName("Street").IsRequired();
        }
    }
}
