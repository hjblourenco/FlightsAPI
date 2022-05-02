using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

public class MongoDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public IMongoCollection<BsonDocument> _flightsCollection { get; set; }
    public IMongoCollection<BsonDocument> _airlinesCollection { get; set; }
    

    public MongoDbContext(IConfiguration configuration)
    {
        _configuration = configuration;


        //Connect fo a Single MongoDB Server
        var mongoDbConnectionString = configuration.GetSection("MongoDbConnectionString:ConnectionString").Value;
        var mongoDbUser             = configuration.GetSection("MongoDbConnectionString:MongoDbUserName").Value;
        var mongoDbPassword         = configuration.GetSection("MongoDbConnectionString:MongoDbPassword").Value;
        var mongoDbDatabase         = configuration.GetSection("MongoDbConnectionString:MongoDbDatabase").Value;
        var mongoDbFlightsCollection       = configuration.GetSection("MongoDbConnectionString:MongoDbFlightsCollection").Value;
        var mongoDbAirlinesCollection       = configuration.GetSection("MongoDbConnectionString:MongoDbAirlinesCollection").Value;
        
        
        mongoDbConnectionString = mongoDbConnectionString.Replace("<username>", mongoDbUser);
        mongoDbConnectionString = mongoDbConnectionString.Replace("<password>", mongoDbPassword);
        mongoDbConnectionString = mongoDbConnectionString.Replace("<database>", mongoDbDatabase);
        var settings = MongoClientSettings.FromConnectionString(mongoDbConnectionString);
        //var settings = MongoClientSettings.FromConnectionString($"mongodb+srv://{mongoDbUser}:{mongoDbPassword}@cluster0.rljqx.mongodb.net/{mongoDbDatabase}?retryWrites=true&w=majority");
        
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        
        
        var client = new MongoClient(settings);
        var database = client.GetDatabase(mongoDbDatabase);
        _flightsCollection = database.GetCollection<BsonDocument>(mongoDbFlightsCollection);
        _airlinesCollection = database.GetCollection<BsonDocument>(mongoDbAirlinesCollection);
        
    }




}