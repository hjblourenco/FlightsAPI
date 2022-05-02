using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class AirlinesRepositoryTests
{
    private readonly MongoDbContext _context;
    private readonly AirlinesMongoDbRepository _airlinesRepository;

    public AirlinesRepositoryTests()
    {
            IConfigurationRoot configuration = RepositoryTests.RepositoryTestsConstructor();
            _context = new MongoDbContext(configuration);  

            var mock = new Mock<ILogger<AirlinesMongoDbRepository>>();
            ILogger<AirlinesMongoDbRepository> _logger = mock.Object;          
            _airlinesRepository = new AirlinesMongoDbRepository(_context,_logger);

    }



    [Fact]
    public void AddAirlinesTest()
    {
        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        {        
            IEnumerable<Airline> getAllAirline =  _airlinesRepository.GetAllAirlinesAsync().Result.ToList();
            
            var airline = AirlinesObjects.CreateTestAirlineObject();
            _airlinesRepository.AddAirlineAsync(airline);

            IEnumerable<Airline> getAllAirlineAfterAdd = _airlinesRepository.GetAllAirlinesAsync().Result.ToList();
            Assert.Equal(getAllAirline.Count() + 1, getAllAirlineAfterAdd.Count());
        });
        t.RunSynchronously();
    }

    [Fact]
    public void AirlineExistsAsyncTest()
    {

        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        {
        var airline = AirlinesObjects.CreateTestAirlineObject();
        _airlinesRepository.AddAirlineAsync(airline);

        bool airlineExists = _airlinesRepository.AirlineExistsAsync(airline.AirlineId);
        Assert.True(airlineExists);
        });
        t.RunSynchronously();
    }

    [Fact]
    public void AirlineExistsByNameAsyncTest()
    {

        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        {        
            var airline = AirlinesObjects.CreateTestAirlineObject();
            _airlinesRepository.AddAirlineAsync(airline);

            bool airlineExists = _airlinesRepository.AirlineExistsByNameAsync(airline.Name).Result;
            Assert.True(airlineExists);
        });
        t.RunSynchronously();
    }


    [Fact]
    public void GetAirlineAsyncTest()
    {
    
        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        {
            var airline = AirlinesObjects.CreateTestAirlineObject();
            _airlinesRepository.AddAirlineAsync(airline);

            Airline airlineFromGetAirline = _airlinesRepository.GetAirlineAsync(airline.AirlineId).Result;
            Assert.Equal(airline.AirlineId, airlineFromGetAirline.AirlineId);
        });
        t.RunSynchronously();

    }

    [Fact]
    public void GetAirlineByNameAsyncTest()
    {

        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        {
            var airline = AirlinesObjects.CreateTestAirlineObject();
            airline.Name = airline.Name+"TestAirlineName";
            _airlinesRepository.AddAirlineAsync(airline);


            //then next method returns a list but the airline name has 30 characters generated randomly and it's almost impossible to find more than one
            var airlineFromGetAirline = _airlinesRepository.GetAirlineByNameAsync(airline.Name).Result;
            
            Assert.Equal(airlineFromGetAirline.Count()>0, true);

        });
        t.RunSynchronously();
    }

    [Fact]
    public void GetAllAirlinesAsyncTest()
    {

        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        {
            var airlinesCountBeforeAdd = _airlinesRepository.GetAllAirlinesAsync().Result.Count();

            var airline = AirlinesObjects.CreateTestAirlineObject();
            _airlinesRepository.AddAirlineAsync(airline);

            var airlinesCountAfterAdd = _airlinesRepository.GetAllAirlinesAsync().Result.Count();

            Assert.Equal(airlinesCountBeforeAdd + 1, airlinesCountAfterAdd);
        });
        t.RunSynchronously();
    }

    [Fact]
    public void UpdateAirlineAsyncTest()
    {



        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        {
            var airline = AirlinesObjects.CreateTestAirlineObject();
            _airlinesRepository.AddAirlineAsync(airline);

            Task.Delay(250).Wait();
            airline.Name = airline.Name + "TestAirlineUpdate";
            _airlinesRepository.UpdateAirlineAsync(airline.AirlineId, airline);

            Airline airlineFromGetAirline = _airlinesRepository.GetAirlineAsync(airline.AirlineId).Result;
            Assert.Equal(airline.Name, airlineFromGetAirline.Name);
        });
        t.RunSynchronously();
    }

    [Fact]
    public void DeleteAirlineAsyncTest()
    {
        Task.Delay(1000).Wait();
        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        {
            var airline = AirlinesObjects.CreateTestAirlineObject();
            _airlinesRepository.AddAirlineAsync(airline);

            Task.Delay(250).Wait();
            _airlinesRepository.DeleteAirlineAsync(airline.AirlineId);

            bool airlineExists = _airlinesRepository.AirlineExistsAsync(airline.AirlineId);
            Assert.False(airlineExists);
        });

        t.RunSynchronously();
    }

}