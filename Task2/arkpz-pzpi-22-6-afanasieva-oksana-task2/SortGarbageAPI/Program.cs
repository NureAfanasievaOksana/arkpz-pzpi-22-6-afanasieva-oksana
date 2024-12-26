using Microsoft.EntityFrameworkCore;
using SortGarbageAPI.Models;
using SortGarbageAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SortGarbageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ContainerService>();
builder.Services.AddScoped<SensorService>();
builder.Services.AddScoped<SensorDataService>();
builder.Services.AddScoped<NotificationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();