using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Entities;

namespace WebApplication3.Configurations
{
    public class SchoolConfig : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.Property(s => s.Number).IsRequired().HasMaxLength(10);
            builder.Property(s => s.Name).IsRequired().HasMaxLength(50);
        }
    }
}
