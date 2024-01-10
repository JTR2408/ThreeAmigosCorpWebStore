using ThreeAmigos.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Auth0.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register ProductService as a transient service
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddHostedService<RefreshService>();

builder.Services.AddAuth0WebAppAuthentication(options => {
    options.Domain = builder.Configuration["AuthO:Domain"];
    options.ClientId = builder.Configuration["AuthO:ClientId"];
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
