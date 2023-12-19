using System;

namespace ThreeAmigos.Products.Services.ProductRepo;

public interface IProductsRepo{
    Task<IEnumerable<Product>> GetProductsAsync();
}