using ImdbScraperApi.Helpers;
using ImdbScraperApi.Data;
using ImdbScraperApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using ImdbScraperApi;
using ImdbScraperApi.Middlewares;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ImdbScraperApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<ActorServices>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(ms => ms.Value.Errors.Count > 0)
            .ToDictionary(
                ms => ms.Key,
                ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        var errorResponse = new ApiResponse<object>(400, "Validation errors occurred", errors: errors);

        return new BadRequestObjectResult(errorResponse);
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Register the ImdbScraperService for dependency injection
builder.Services.AddHttpClient<ImdbScraperService>();
// Register the RankerScraperService for dependency injection
builder.Services.AddHttpClient<RankerScraperService>();
// Register the RankerScraperAPIService for dependency injection
builder.Services.AddHttpClient<RankerScraperAPIService>();

// Register the DbContext using the in-memory database provider
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ActorsDb").LogTo(Console.WriteLine, LogLevel.Information));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
await app.SeedActorDataAsync(); 

app.Run();
