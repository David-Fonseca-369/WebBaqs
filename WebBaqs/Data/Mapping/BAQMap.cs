using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebBaqs.Entities;

namespace WebBaqs.Data.Mapping
{
    public class BAQMap : IEntityTypeConfiguration<BAQ>
    {
        public void Configure(EntityTypeBuilder<BAQ> builder)
        {
            builder.ToTable("baqs")
               .HasKey(x => x.id);

            
        }
    }
}
