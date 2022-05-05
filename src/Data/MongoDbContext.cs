using System.Security.Authentication;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

public class MongoDbContext 
{
    private readonly IConfiguration _configuration;
    public IMongoCollection<BsonDocument> _flightsCollection { get; set; }
    public IMongoCollection<BsonDocument> _airlinesCollection { get; set; }    
    public MongoDbContext(IConfiguration configuration,SecretClient AzureKeyVaultclient)
    {

        
        _configuration = configuration;

        //Connect to a Single MongoDB Server in Azure Key Vault
        var mongoDbConnectionString   = AzureKeyVaultclient.GetSecret("ConnectionString").Value.Value;
        var mongoDbUser               = AzureKeyVaultclient.GetSecret("MongoDbUserName").Value.Value;
        var mongoDbPassword           = AzureKeyVaultclient.GetSecret("MongoDbPassword").Value.Value;
        var mongoDbDatabase           = AzureKeyVaultclient.GetSecret("MongoDbDatabase").Value.Value;
        var mongoDbFlightsCollection  = AzureKeyVaultclient.GetSecret("MongoDbFlightsCollection").Value.Value;
        var mongoDbAirlinesCollection = AzureKeyVaultclient.GetSecret("MongoDbAirlinesCollection").Value.Value;
        
        //Connect to a Single MongoDB Server with AppSettings.json
        //var mongoDbConnectionString   = configuration.GetSection("MongoDbConnectionString:ConnectionString").Value;
        //var mongoDbUser               = configuration.GetSection("MongoDbConnectionString:MongoDbUserName").Value;
        //var mongoDbPassword           = configuration.GetSection("MongoDbConnectionString:MongoDbPassword").Value;
        //var mongoDbDatabase           = configuration.GetSection("MongoDbConnectionString:MongoDbDatabase").Value;
        //var mongoDbFlightsCollection  = configuration.GetSection("MongoDbConnectionString:MongoDbFlightsCollection").Value;
        //var mongoDbAirlinesCollection = configuration.GetSection("MongoDbConnectionString:MongoDbAirlinesCollection").Value;
                
        mongoDbConnectionString = mongoDbConnectionString?.Replace("<username>", mongoDbUser);
        mongoDbConnectionString = mongoDbConnectionString?.Replace("<password>", mongoDbPassword);
        mongoDbConnectionString = mongoDbConnectionString?.Replace("<database>", mongoDbDatabase);
        var settings = MongoClientSettings.FromConnectionString(mongoDbConnectionString);
        
        //settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        
        
        var client = new MongoClient(settings);
        var database = client.GetDatabase(mongoDbDatabase);
        _flightsCollection = database.GetCollection<BsonDocument>(mongoDbFlightsCollection);
        _airlinesCollection = database.GetCollection<BsonDocument>(mongoDbAirlinesCollection);
        
    }




}