namespace ThreeAmigos.Products.Data.Products;

public static class ProductsInitialiser{
    public static async Task SeedTestData(ProductContext context){
        if(context.Products.Any()){
            return;
        }
        var products = new List<Product>{
            new() { Id = 1, Name = "Test product X"},
            new() { Id = 2, Name = "Test product Y"},
            new() { Id = 3, Name = "Test product Z"},
        };
        products.ForEach(p => context.Add(p));
        await context.SaveChangesAsync();
    }
}