using DeliveryVHGP.Data;
using DeliveryVHGP.Repositories;
using DeliveryVHGP.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Config API for everyone can use
builder.Services.AddCors(options => options.AddPolicy(name: "MyPolicy", policy => 
policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Declare context and configuration
builder.Services.AddDbContext<ShipperContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
});

// Add AutoMapper service
builder.Services.AddAutoMapper(typeof (Program));

builder.Services.AddScoped<IShipperRepository, ShipperRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "SampleInstance";
});


builder.Services.AddScoped<ICacheService, CacheService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
