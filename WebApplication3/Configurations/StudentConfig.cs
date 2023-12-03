using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication3.Entities;

namespace WebApplication3.Configuration
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
           builder.Property(b => b.FirstName).IsRequired().HasMaxLength(50);
           builder.Property(b => b.LastName).IsRequired().HasMaxLength(50);
           builder.Property(b => b.SchoolId).IsRequired();
          
        }
    }
}
