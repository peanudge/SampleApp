using System.Reflection;
using Cart.API.BackgroundServices;
using Cart.API.Extensions;
using Domain.Repositories;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRedisSettings(builder.Configuration);

builder.Services.AddEventBus(builder.Configuration);

builder.Services.AddCatalogService(new Uri(builder.Configuration["CatalogApiUrl"]));

builder.Services.AddScoped<ICartRepository, CartRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHostedService<ItemSoldOutBackgroundService>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program { }
