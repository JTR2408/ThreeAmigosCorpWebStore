namespace ThreeAmigos.Products.Services.UnderCut;

public class ProductDto
{
    public int Id {get; set;}

    public string Name {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    
    public float Price { get; set; }

    public string BrandName { get; set; } = string.Empty;

    public int BrandId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public bool InStock { get; set; }
}