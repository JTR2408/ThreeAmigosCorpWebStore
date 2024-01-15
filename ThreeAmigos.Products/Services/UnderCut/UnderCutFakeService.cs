namespace ThreeAmigos.Products.Services.UnderCut;

public class UnderCutServiceFake : IUnderCutService{

    private readonly ProductDto[] _products ={
        new ProductDto {Id = 1, Name = "Fake Name 1"},
        new ProductDto {Id = 2, Name = "Fake Name 2"},
        new ProductDto {Id = 3, Name = "Fake Name 3"}
    };

    public Task<IEnumerable<ProductDto>> GetProductsAsync(){
        var products = _products.AsEnumerable();
        return Task.FromResult(products);
    }

}
