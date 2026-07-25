using ClosetCompanionApp.Repository;
using ClosetCompanionApp.Repository.Implementations;
using ClosetCompanionApp.Repository.Interfaces;
using ClosetCompanionApp.Service.Implementations;
using ClosetCompanionApp.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database Context (PostgreSQL via Supabase)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IGarmentRepository, GarmentRepository>();
builder.Services.AddScoped<IPosePhotoRepository, PosePhotoRepository>();
builder.Services.AddScoped<IOutfitRepository, OutfitRepository>();

builder.Services.AddScoped<IGarmentService, GarmentService>();
builder.Services.AddScoped<IPosePhotoService, PosePhotoService>();
builder.Services.AddScoped<IOutfitService, OutfitService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
