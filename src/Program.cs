using System.Security.Cryptography.X509Certificates;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

//Logging
using var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

logger.Information("Flights API started!");

///Azure Key Vault acess - I Used this to practice the use of it in an application
// https://docs.microsoft.com/pt-pt/azure/key-vault/secrets/quick-create-net
string keyVaultUri = configuration.GetSection("AzureKeyVault:Name").Value;
var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());



builder.Logging.AddSerilog(logger);

//End of Logging

builder.Services.AddSingleton<SecretClient>(client);

builder.Services.AddScoped<MongoDbContext>();

builder.Services.AddScoped<IFlightsRepository, FlightsMongoDbRepository>();
builder.Services.AddScoped<IAirlinesRepository, AirlinesMongoDbRepository>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen();

//Health Check service
builder.Services.AddHealthChecks();

// Teste para fazer commit e agora no devops
var app = builder.Build();

//Health Check
app.MapHealthChecks("/health");

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
