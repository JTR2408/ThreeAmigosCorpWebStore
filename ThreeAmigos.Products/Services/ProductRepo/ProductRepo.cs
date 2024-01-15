using Microsoft.EntityFrameworkCore;
using ThreeAmigos.Products.Data.Products;

namespace ThreeAmigos.Products.Services.ProductRepo;

public class ProductRepo : IProductsRepo{
    private readonly ProductContext _productsContext;

    public ProductRepo(ProductContext productsContext){
        _productsContext = productsContext;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(){
        var products = await _productsContext.Products.Select(p => new Product
        {
            Id = p.Id,
            Name = p.Name
        }).ToListAsync();
        return products;
    }
}