using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class AirlinesControllerTest
{
    private readonly AirlinesController _airlinesController;
    private readonly IMapper _mapper;

    public AirlinesControllerTest()
    {
            //Creating repository
            var myConfiguration = new Dictionary<string, string>
            {
                {"MongoDbConnectionString:ConnectionString",  "mongodb+srv://<username>:<password>@cluster0.rljqx.mongodb.net/<database>?retryWrites=true&w=majority"},
                {"MongoDbConnectionString:MongoDbUserName",   "flights"},
                {"MongoDbConnectionString:MongoDbPassword",   "flights"},
                {"MongoDbConnectionString:MongoDbDatabase",   "FlightsTests"},
                {"MongoDbConnectionString:MongoDbFlightsCollection", "FlightsTests"},
                {"MongoDbConnectionString:MongoDbAirlinesCollection", "AirlinesTests"}
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            var mockRepositoryLogger = new Mock<ILogger<AirlinesMongoDbRepository>>();
            ILogger<AirlinesMongoDbRepository> _loggerRepository = mockRepositoryLogger.Object;

            MongoDbContext _context = new MongoDbContext(configuration);
            var _airlinesRepository = new AirlinesMongoDbRepository(_context, _loggerRepository);
            //End of flights repository

            //auto mapper configuration, the same one as in main project, and I created the reverseMap to adjust some tests
            var configurationIMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Airline,AirlineCreateDto>().ReverseMap();
                cfg.CreateMap<Airline,AirlineReadDto>();
                cfg.CreateMap<AirlineUpdateDto,Airline>().ReverseMap();

            });

            configurationIMapper.AssertConfigurationIsValid();

            _mapper = configurationIMapper.CreateMapper();
            //End of auto mapper configuration

            var mockAirlinesController = new Mock<ILogger<AirlinesController>>();
            ILogger<AirlinesController> _loggerController = mockAirlinesController.Object;
            

            //Creating controller
            _airlinesController = new AirlinesController(_airlinesRepository, _mapper, _loggerController);
            //End of controller
    }

    [Fact]
    public async Task GetAllAirlines_ReturnsOk()
    {
        //Arrange
        //Act
        var result = await _airlinesController.GetAllAirlines();
        //Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }
    
    [Fact]
    public async Task GetAirlineById_ReturnsOk()
    {
        //Arrange
        var airlineToAdd = AirlinesObjects.CreateTestAirlineObject();

        var airline = _mapper.Map<AirlineCreateDto>(airlineToAdd); 
        _airlinesController.CreateAirline(airline);
        //Act
        var result = await _airlinesController.GetAirlineById(airlineToAdd.AirlineId);
        //Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async void GetAirlineById_ReturnsNotFound()
    {
        //Arrange
        var airlineId = Guid.NewGuid();
        //Act
        var result = await _airlinesController.GetAirlineById(airlineId);
        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void CreateAirline_ReturnsOk()
    {
        //Arrange
        var airlineToAdd = AirlinesObjects.CreateTestAirlineObject();
        var airline = _mapper.Map<AirlineCreateDto>(airlineToAdd); 
        //Act
        var result = _airlinesController.CreateAirline(airline);
        //Assert
        Assert.IsType<CreatedAtRouteResult>(result.Result);
    }

    [Fact]
    public void CreateAirline_ReturnsBadRequest()
    {
        //Arrange
        var airlineToAdd = AirlinesObjects.CreateTestAirlineObject();
        airlineToAdd.Name = null;
        var airline = _mapper.Map<AirlineCreateDto>(airlineToAdd);

        //Act
        var result = _airlinesController.CreateAirline(airline);      
        //Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public void UpdateAirline_ReturnsOk()
    {
     
        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        {     
            //Arrange
            var airlineToAdd = AirlinesObjects.CreateTestAirlineObject();
            var airline = _mapper.Map<AirlineCreateDto>(airlineToAdd);        
            _airlinesController.CreateAirline(airline);
            Task.Delay(2000).Wait();
            var airlineToUpdate = AirlinesObjects.CreateTestAirlineObject();
            var airlineUpdate = _mapper.Map<AirlineUpdateDto>(airlineToUpdate);
            airlineUpdate.AirlineId=  airline.AirlineId;
            //Act
            var result = _airlinesController.UpdateAirline(airline.AirlineId, airlineUpdate);
            //Assert
            Assert.IsType<OkObjectResult>(result);   
        });
        t.RunSynchronously();              
    }

    [Fact]
    public void UpdateAirline_ReturnsBadRequest()
    {
        //Arrange
        var airlineToAdd = AirlinesObjects.CreateTestAirlineObject();
        var airline = _mapper.Map<AirlineCreateDto>(airlineToAdd);
        _airlinesController.CreateAirline(airline);
        Task.Delay(250).Wait();
        var airlineToUpdate = AirlinesObjects.CreateTestAirlineObject();
        var airlineUpdate = _mapper.Map<AirlineUpdateDto>(airlineToUpdate);
        airlineUpdate.Name = null;
        //Act
        var result = _airlinesController.UpdateAirline(airlineToAdd.AirlineId, airlineUpdate);
        //Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void UpdateAirline_ReturnsNotFound()
    {

        var airlineToUpdate = AirlinesObjects.CreateTestAirlineObject();
        var airlineUpdate = _mapper.Map<AirlineUpdateDto>(airlineToUpdate);
        airlineUpdate.AirlineId = Guid.NewGuid();
        //Act
        var result = _airlinesController.UpdateAirline(airlineUpdate.AirlineId, airlineUpdate);
        //Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void DeleteAirline_ReturnsNoContent()
    {
        //Arrange
        var airlineToAdd = AirlinesObjects.CreateTestAirlineObject();
        var airline = _mapper.Map<AirlineCreateDto>(airlineToAdd);
        _airlinesController.CreateAirline(airline);
        //Act
        var result = _airlinesController.DeleteAirline(airlineToAdd.AirlineId);
        //Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteAirline_ReturnsNotFound()
    {
        //Arrange
        var airlineId = Guid.NewGuid();
        //Act
        var result = _airlinesController.DeleteAirline(airlineId);
        //Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }



}