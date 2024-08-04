using Application.DTOs;
using Application.Mappings;
using Application.Services.Interfaces;
using Application.Validation;
using CartService.Middleware;
using Domain.RepositoriyInterfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Add the DbContext service
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsAssembly("Infrastructure")
    ));


// Register AutoMapper and specify the assembly containing the profiles
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);


builder.Services.AddScoped<IValidator<CartDto>, CartValidator>();
builder.Services.AddScoped<IValidator<CartItemDto>, CartItemValidator>();

// Register repositories and services
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, Application.Services.CartService>();



var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

