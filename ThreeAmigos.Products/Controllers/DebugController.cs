using Microsoft.AspNetCore.Mvc;
using ThreeAmigos.Products.Services.UnderCut;

namespace ThreeAmigos.Products.Controllers;

[ApiController]
[Route ("[controller]")]
public class DebugController : ControllerBase{

 private readonly ILogger _logger;
    private readonly IUnderCutService _underCutService;

    public DebugController(ILogger<DebugController> logger,
                             IUnderCutService underCutService)
    {
        _logger = logger;
        _underCutService = underCutService;
    }

    [HttpGet("UnderCut")]
    public async Task<IActionResult> UnderCut(){
        IEnumerable<ProductDto> products = null!;
        try{
            products = await _underCutService.GetProductsAsync();
        }
        catch{
            _logger.LogWarning("An Error has Occurred With UnderCut Service");
            products = Array.Empty<ProductDto>();
        }
        return Ok(products.ToList());
    }

}