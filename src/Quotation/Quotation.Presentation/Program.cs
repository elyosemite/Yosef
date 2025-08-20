using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Quotation.Presentation;
using Quotation.Presentation.Domain;
using Quotation.Presentation.Infrastructure;
using Quotation.Presentation.Infrastructure.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddQuotationInfrastructure(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quotation API",
        Version = "v1",
        Description = "Exemplo de API usando .NET 9 com Swagger (OpenAPI)",
        Contact = new OpenApiContact
        {
            Name = "Seu Nome",
            Email = "seuemail@dominio.com"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quotation API v1");
        c.RoutePrefix = "/api/quotation";
    });
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/create", async ([FromBody] QuotationDataModel request, IQuotationRepository repository, IMapper mapper) =>
{
    var domain = mapper.Map<QuotationDomain>(request);
    var result = await repository.CreateAsync(domain);
    return Results.Ok(result);
})
.WithName("goo");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
