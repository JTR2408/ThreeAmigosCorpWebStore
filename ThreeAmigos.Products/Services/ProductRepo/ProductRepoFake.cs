using System;
using ThreeAmigos.Products.Services.ProductRepo;

namespace ThreeAmigos.Products.Services.ProductRepo;

public class ProductRepoFake : IProductsRepo{

    private readonly Product[] _products ={
        new Product {Id = 4, Name = "Fake Name 4"},
        new Product {Id = 5, Name = "Fake Name 5"},
        new Product {Id = 6, Name = "Fake Name 6"}
    };

    public Task<IEnumerable<Product>> GetProductsAsync(){
        var products = _products.AsEnumerable();
        return Task.FromResult(products);
    }

}
