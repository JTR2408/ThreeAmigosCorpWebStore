using ThreeAmigos.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
﻿using Auth0.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register ProductService as a transient service
builder.Services.AddHttpClient<IProductService, ProductService>();

// builder.Services.AddAuth0WebAppAuthentication(options =>
// {
//     options.Domain = builder.Configuration["Auth0:Domain"];
//     options.ClientId = builder.Configuration["Auth0:ClientId"];
// });

builder.Services.AddHostedService<RefreshService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// app.UseAuthentication();
// app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
