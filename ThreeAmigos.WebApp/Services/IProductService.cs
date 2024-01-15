namespace ThreeAmigos.WebApp.Services{
    public interface IProductService{
    Task<List<ProductDto>> GetProductDataAsync();
    }
}
