namespace ThreeAmigos.Products.Services.UnderCut;

public interface IUnderCutService{
    Task<IEnumerable<ProductDto>> GetProductsAsync();
}