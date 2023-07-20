using API.Controllers;
using API.Extensions;
using API.Filters;
using API.ResponseModels;
using Domain.Extensions;
using RiskFirst.Hateoas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(config =>
{
    config.Filters.Add<HttpCustomExceptionFilter>();
});

builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddMappers();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddValidators();
builder.Services.AddLinks(config =>
{
    config.AddPolicy<ItemHateoasResponse>(policy =>
    {
        policy.RequireRoutedLink(nameof(ItemHateoasController.Get), nameof(ItemController.Get))
              .RequireRoutedLink(nameof(ItemHateoasController.GetById), nameof(ItemController.GetById), _ => new { id = _.Data?.Id })
              .RequireRoutedLink(nameof(ItemHateoasController.Post), nameof(ItemController.Post))
              .RequireRoutedLink(nameof(ItemHateoasController.Put), nameof(ItemController.Put), x => new { id = x.Data?.Id })
              .RequireRoutedLink(nameof(ItemHateoasController.Delete), nameof(ItemController.Delete), x => new { id = x.Data?.Id });
    });
});
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

public partial class Program { }
