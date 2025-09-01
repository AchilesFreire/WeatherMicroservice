using System.Net.Http.Headers;
using WeatherMicroservice.Clients;
using WeatherMicroservice.Infrastructure;
using WeatherMicroservice.Services;
using Microsoft.EntityFrameworkCore;
using WeatherMicroservice.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<WeatherDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IWeatherService, WeatherService>();
builder.Services.AddTransient<WeatherRepository>();

builder.Services.AddHttpClient<WeatherApiClient>((sp, client) =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();
    var apiKey = cfg["WeatherApi:ApiKey"];
    if (string.IsNullOrEmpty(apiKey))
    {
        throw new InvalidOperationException("WeatherApi:ApiKey is not configured.");
    }
    client.BaseAddress = new Uri(cfg["WeatherApi:BaseUrl"]);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});



var app = builder.Build();


if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization(); 

app.Use(async (context, next) =>
{
    try
    {
        await next();
        // Handle non-successful status codes for API responses
        if (context.Response.StatusCode >= 400 && context.Response.StatusCode < 600 && !context.Response.HasStarted)
        {
            var problem = new
            {
                status = context.Response.StatusCode,
                title = "API Error",
                detail = $"The API returned status code {context.Response.StatusCode}."
            };
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
    catch (ArgumentException argEx)
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";
        var problem = new
        {
            status = context.Response.StatusCode,
            title = "Bad Request",
            detail = argEx.Message
        };
        await context.Response.WriteAsJsonAsync(problem);
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var problem = new
        {
            status = context.Response.StatusCode,
            title = "Unhandled Exception",
            detail = ex.Message
        };
        await context.Response.WriteAsJsonAsync(problem);
    }
});

app.MapControllers();

app.Run();

