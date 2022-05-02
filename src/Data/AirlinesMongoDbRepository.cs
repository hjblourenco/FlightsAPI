using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

public class AirlinesMongoDbRepository : IAirlinesRepository
{
    
    private readonly IMongoCollection<BsonDocument> _context;
    private readonly ILogger<AirlinesMongoDbRepository> _logger;

    public AirlinesMongoDbRepository()
    {
    }

    public AirlinesMongoDbRepository(MongoDbContext context,ILogger<AirlinesMongoDbRepository> logger)
    {
        _context = context._airlinesCollection;
        _logger = logger;
    }


    public async void AddAirlineAsync(Airline airline)
    {
        if (airline == null)
        {
            throw new ArgumentNullException(nameof(airline));
        }


        await _context.InsertOneAsync(airline.ToBsonDocument());
        
    }

    public bool AirlineExistsAsync(Guid airlineId)
    {
        _logger.LogDebug("--> AirlineExistsAsync repository");     

        var filter = Builders<BsonDocument>.Filter.Eq("_id", airlineId.ToString());
        
        var airlineExists = _context.Find(filter).ToList().Count()>0;

        return airlineExists;
    }

    public async Task<bool> AirlineExistsByNameAsync(string name)
    {



        _logger.LogDebug("--> AirlineExistsByNameAsync repository");     

        var filter = Builders<BsonDocument>.Filter.Eq("Name", name);
        //var airlineExists = _context.FindAsync(filter).Result.ToList().Count()>0;
        var airlineResult = await _context.FindAsync(filter);
        var airlineResultToListAsync = await airlineResult.ToListAsync();
        var airlineExists = airlineResultToListAsync.Count()>0;
 

        return airlineExists;
     }

    public async void DeleteAirlineAsync(Guid airlineId)
    {
        _logger.LogDebug("--> DeleteAirline repository");     

        var filter = Builders<BsonDocument>.Filter.Eq("_id", airlineId.ToString());
        var flightDeleteResult = await _context.FindOneAndDeleteAsync(filter);

        _logger.LogDebug($"Delete airline repository return: {flightDeleteResult.ToString()}");
    }

    public async Task<Airline> GetAirlineAsync(Guid airlineId)
    {
        _logger.LogDebug("--> GetAirlineAsync repository");     

        var filter = Builders<BsonDocument>.Filter.Eq("_id", airlineId.ToString());
        var airline = await _context.FindAsync(filter);
        var airlineResult = await airline.FirstOrDefaultAsync();
        var airlineObject = BsonSerializer.Deserialize<Airline>(airlineResult);

        return airlineObject;
    }

    public async Task<IEnumerable<Airline>> GetAirlineByNameAsync(string name)
    {
        _logger.LogDebug("--> GetAirlineByNameAsync repository");     

        var builder = Builders<BsonDocument>.Filter;
        var filter = builder.Regex("Name", name);


        var airline = await _context.FindAsync(filter);
        var airlineToListAsync = await airline.ToListAsync();
        var airlineDeserialized = airlineToListAsync.Select(value => BsonSerializer.Deserialize<Airline>(value));

        return airlineDeserialized;
    }

    public async Task<IEnumerable<Airline>> GetAllAirlinesAsync()
    {
        _logger.LogDebug("--> GetAllAirlinesAsync repository");     

        var airline = await _context.Find(new BsonDocument()).ToListAsync();
        var airlineToListAsync = airline.Select(value => BsonSerializer.Deserialize<Airline>(value));

        return airlineToListAsync;
    }

    public bool Save()
    {
        return true;
    }

    public async void UpdateAirlineAsync(Guid airlineId, Airline airline)
    {
         _logger.LogDebug("--> UpdateAirlineAsync repository"); 
        if (! AirlineExistsAsync(airlineId))
        {
            throw new ArgumentException("Airline does not exist");
        }

        var filter = Builders<BsonDocument>.Filter.Eq("_id", airlineId.ToString());
        var airlineUpdateResult = await _context.FindOneAndReplaceAsync(filter, airline.ToBsonDocument());

 
    }
}
