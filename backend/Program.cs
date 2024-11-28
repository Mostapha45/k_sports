using Azure.Storage.Blobs;
using backend.Data;
using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Stripe;

DotEnv.Load();

var stripeSecretKey = Environment.GetEnvironmentVariable("STRIPE_KEY") ?? throw new ArgumentNullException("STRIPE_KEY");
var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? throw new Exception("Must provide db connection string");
var azureStorageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING") ?? throw new Exception("Must provide Azure Storage connection string");

StripeConfiguration.ApiKey = stripeSecretKey;

var builder = WebApplication.CreateBuilder(args);

ConfigureCors(builder.Services, builder.Environment);

ConfigureServices(builder.Services, dbConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
ConfigurePipeline(app);

app.Run();

/// <summary>
/// Configures CORS settings for the application.
/// </summary>
/// <param name="services">The service collection to add the CORS policy to.</param>
/// <param name="environment">The web host environment to determine the origin.</param>
void ConfigureCors(IServiceCollection services, IWebHostEnvironment environment)
{
    const string CorsPolicyName = "CorsSettings";
    services.AddCors(options =>
    {
        options.AddPolicy(CorsPolicyName, corsBuilder =>
        {
            corsBuilder.WithOrigins(environment.IsDevelopment() ? "http://localhost:3000" : "https://k-sports.vercel.app")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
        });
    });
}


/// <summary>
/// Configures services for the application.
/// </summary>
/// <param name="services">The service collection to add services to.</param>
/// <param name="dbConnectionString">The database connection string.</param>
void ConfigureServices(IServiceCollection services, string dbConnectionString)
{
    // Add controllers
    services.AddControllers();
    
    // Add API explorer and Swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddSingleton(new BlobServiceClient(azureStorageConnectionString));
    
    // Add database context
    services.AddDbContext<DataContext>(options =>
        options.UseNpgsql(dbConnectionString));
}


/// <summary>
/// Configures the HTTP request pipeline.
/// </summary>
/// <param name="app">The web application to configure the pipeline for.</param>
void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors("CorsSettings");

    app.MapGet("/", () => "Hello World");
    app.MapControllers();
}