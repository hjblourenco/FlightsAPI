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
builder.Logging.AddSerilog(logger);
logger.Information("Flights API started!");

///Azure Key Vault acess - I Used this to practice the use of it in an application
// https://docs.microsoft.com/pt-pt/azure/key-vault/secrets/quick-create-net

string keyVaultUri = configuration.GetSection("AzureKeyVault:Name").Value;


const string tenantId = "9be98aa3-d3a1-4dad-ad56-09bafd151bec";
const string clientId = "7369d4d4-5419-4447-9615-3c7b70739f69";
const string clientSecret = "Jwa8Q~ZnUfEXxPaUPHomYhi931byJS94PQkCydrv";
var keyvaultCredentials = new ClientSecretCredential(tenantId, clientId, clientSecret);

var client = new SecretClient(new Uri(keyVaultUri), keyvaultCredentials);



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
