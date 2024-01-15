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

builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddAuth0WebAppAuthentication(options => {
    options.Domain = builder.Configuration["Auth:Domain"];
    options.ClientId = builder.Configuration["Auth:ClientId"];
});

builder.Services.AddControllers();

builder.Services.AddHostedService<RefreshService>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
