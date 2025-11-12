using BLL.Interface;
using BLL.RN;
using DataLayer.Interface;
using DataLayer.RN;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis:Configuration").Value;
    options.InstanceName = "AppCache_";
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ITicketsServices, TicketsServices>();
builder.Services.AddScoped<IEjecutarRepository, EjecutarRepository>();
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api Prueba DoubleV",
        Version = "v1",
        Description = "Prueba DoubleV",
    });


});

var app = builder.Build();
app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

if (app.Environment.IsDevelopment())
{

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Prueba DoubleV");
    });
}
else
{
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api Prueba DoubleV");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
