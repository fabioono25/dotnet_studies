using BasicStore.Catalog.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicStore.Catalog.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.Property(c => c.Description)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(c => c.Image)
                .IsRequired()
                .HasColumnType("varchar(250)");

            builder.OwnsOne(c => c.Dimensions, cm =>
            {
                cm.Property(c => c.Height)
                    .HasColumnName("Height")
                    .HasColumnType("int");

                cm.Property(c => c.Width)
                    .HasColumnName("Width")
                    .HasColumnType("int");

                cm.Property(c => c.Deep)
                    .HasColumnName("Deep")
                    .HasColumnType("int");
            });

            builder.ToTable("Products");
        }
    }
}
