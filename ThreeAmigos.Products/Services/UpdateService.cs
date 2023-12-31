using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using ThreeAmigos.Products.Services.UnderCut;

public class UpdateService : BackgroundService
{
    private readonly IUnderCutService _underCutService;
    private readonly ILogger<UpdateService> _logger;

    public UpdateService(IUnderCutService underCutService, ILogger<UpdateService> logger)
    {
        _underCutService = underCutService ?? throw new ArgumentNullException(nameof(underCutService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Call the UnderCut method to fetch products
                IEnumerable<ProductDto> products = await _underCutService.GetProductsAsync();

                // Process the products or do something with the data here
                // For example, you can update a cache or a database with this data

                // Log that the task was executed successfully
                _logger.LogInformation("Product update task executed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating products");
            }

            // Wait for 5 minutes before executing the task again
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
