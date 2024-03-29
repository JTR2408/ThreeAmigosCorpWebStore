﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ThreeAmigos.Products.Data.Products;
using ThreeAmigos.Products.Services.UnderCut;
using Polly;
using Polly.Extensions.Http;
using ThreeAmigos.Products.Services.ProductRepo;;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.Audience = builder.Configuration["Auth:Audience"];
    });
builder.Services.AddAuthorization();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IUnderCutService, UnderCutServiceFake>();
}

builder.Services.AddDbContext<ProductContext>(options =>{
    if(builder.Environment.IsDevelopment()){
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "products.db");
        options.UseSqlite($"Data Source={dbPath}");
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
    else{
        var cs = builder.Configuration.GetConnectionString("ProductsContext");
        options.UseSqlServer(cs, sqlServerOptionsAction: sqlOptions =>
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(2),
                errorNumbersToAdd: null
            ));       
    }
});

if(builder.Environment.IsDevelopment()){
    builder.Services.AddSingleton<IUnderCutService, UnderCutServiceFake>();
}
else{
    builder.Services.AddHttpClient<IUnderCutService, UnderCutService>()
        .AddPolicyHandler(GetRetryPolicy());
}

builder.Services.AddTransient<IProductsRepo, ProductRepo>();

var app = builder.Build();

using (var scope = app.Services.CreateScope()){
    var services = scope.ServiceProvider;
    var env = services.GetRequiredService<IWebHostEnvironment>();
    if (env.IsDevelopment()){
        var context = services.GetRequiredService<ProductContext>();
        try{
            ProductsInitialiser.SeedTestData(context).Wait();
        }
        catch (Exception e){
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogDebug("Seeding test data failed.");
        }
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(){
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(5, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}