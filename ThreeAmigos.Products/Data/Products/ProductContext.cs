using Microsoft.EntityFrameworkCore;

namespace ThreeAmigos.Products.Data.Products;

public class ProductContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;

    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>(p =>
        {
            p.Property(c => c.Name).IsRequired();
        });
    }
}