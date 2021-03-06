using AutoMapper;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

public class AirlinesControllerTest
{
    private readonly AirlinesController _airlinesController;
    private readonly IMapper _mapper;

    public AirlinesControllerTest()
    {
            //Creating repository
            IConfigurationRoot configuration = RepositoryTests.RepositoryTestsConstructor();

            var mockRepositoryLogger = new Mock<ILogger<AirlinesMongoDbRepository>>();
            ILogger<AirlinesMongoDbRepository> _loggerRepository = mockRepositoryLogger.Object;

            ///Azure Key Vault acess - I Used this to practice the use of it in an application
            // https://docs.microsoft.com/pt-pt/azure/key-vault/secrets/quick-create-net
            string keyVaultUri = configuration.GetSection("AzureKeyVault:Name").Value;
            var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

            MongoDbContext _context = new MongoDbContext(configuration,client);
            var _airlinesRepository = new AirlinesMongoDbRepository(_context, _loggerRepository);

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
        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task( async () =>
        {     
            //Arrange
            var airlineToAdd = AirlinesObjects.CreateTestAirlineObject();

            var airline = _mapper.Map<AirlineCreateDto>(airlineToAdd); 
            _airlinesController.CreateAirline(airline);

            //Task.Delay(500).Wait();
            //Act
            var result = await _airlinesController.GetAirlineById(airlineToAdd.AirlineId);
            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        });
        t.RunSynchronously();                
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
            //Task.Delay(500).Wait();
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
        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        { 

            //Arrange
            var airlineToAdd = AirlinesObjects.CreateTestAirlineObject();
            var airline = _mapper.Map<AirlineCreateDto>(airlineToAdd);
            _airlinesController.CreateAirline(airline);
            //Task.Delay(250).Wait();
            var airlineToUpdate = AirlinesObjects.CreateTestAirlineObject();
            var airlineUpdate = _mapper.Map<AirlineUpdateDto>(airlineToUpdate);
            airlineUpdate.Name = null;
            //Act
            var result = _airlinesController.UpdateAirline(airlineToAdd.AirlineId, airlineUpdate);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        });
        t.RunSynchronously();               
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

        Task t = new Task(() =>
        {   
            //Arrange
            var airlineToAdd = AirlinesObjects.CreateTestAirlineObject();
            var airline = _mapper.Map<AirlineCreateDto>(airlineToAdd);
            _airlinesController.CreateAirline(airline);
            //Act
            var result = _airlinesController.DeleteAirline(airlineToAdd.AirlineId);
            //Assert
            Assert.IsType<NoContentResult>(result);
        });
        t.RunSynchronously();              
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