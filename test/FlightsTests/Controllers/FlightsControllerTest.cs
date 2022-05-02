
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class FlightsControllerTest
{
    private readonly FlightsController _flightsController;
    private readonly FlightsMongoDbRepository _flightRepository;
    private readonly IMapper _mapper;

    public FlightsControllerTest()
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
            MongoDbContext _context = new MongoDbContext(configuration);
            _flightRepository = new FlightsMongoDbRepository(_context);
            //End of flights repository

            //auto mapper configuration, the same one as in main project, and I created the reverseMap to adjust some tests
            var configurationIMApper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Flight,FlightReadDto>();
                cfg.CreateMap<FlightCreateDto,Flight>().ReverseMap();
                cfg.CreateMap<FlightUpdateDto,Flight>().ReverseMap();
                cfg.CreateMap<Flight,FlightSearchDto>()
                //Change to string to search
                    .ForMember(dest => dest.DepartureDate, opt => opt.MapFrom(src => src.DepartureDate.ToLongTimeString()))
                    .ForMember(dest => dest.ArrivalDate  , opt => opt.MapFrom(src => src.ArrivalDate  .ToLongDateString()))
                    .ForMember(dest => dest.Price        , opt => opt.MapFrom(src => src.Price        .ToString()))
                    .ForMember(dest => dest.PricePaid    , opt => opt.MapFrom(src => src.PricePaid    .ToString()))
                    .ForMember(dest => dest.PaymentType  , opt => opt.MapFrom(src => Enum.GetName(src.PaymentType) .ToString()));

                ;
                cfg.CreateMap<FlightSearchDto,FlightReadDto>();
            });

            configurationIMApper.AssertConfigurationIsValid();

            _mapper = configurationIMApper.CreateMapper();
            //End of auto mapper configuration

            var mock = new Mock<ILogger<FlightsController>>();
            ILogger<FlightsController> _logger = mock.Object;

            //Creating controller
            _flightsController = new FlightsController(_flightRepository, _mapper, _logger);
            //End of controller
    }

    [Fact]
    public void GetAllFlightsController()
    {
        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        {
            var actionResult = _flightsController.GetAllFlights();
            var allFlightsRepository = _flightRepository.GetAllFlights();
            var result = actionResult.Result as OkObjectResult;
            var getAllFlightsRepositoryMapped = _mapper.Map<IEnumerable<FlightReadDto>>(allFlightsRepository);

            Assert.Equal(result.StatusCode , StatusCodes.Status200OK);
            Assert.Equal(((IEnumerable<FlightReadDto>)result.Value).Count() , getAllFlightsRepositoryMapped.Count());        
            Assert.Equal(((IEnumerable<FlightReadDto>)result.Value).First().FlightId , getAllFlightsRepositoryMapped.First().FlightId);
            Assert.Equal(((IEnumerable<FlightReadDto>)result.Value).Last().FlightId , getAllFlightsRepositoryMapped.Last().FlightId);
        });
        t.RunSynchronously();
    }


    [Fact]
    public void AddFlightController()
    {
        //This is added because of the async can wait time and other taks can be conpleted in the middle, and this runs sync
        Task t = new Task(() =>
        {
            //Getting total flight count before adding new flight
            var actionResultBefore = _flightsController.GetAllFlights();
            var resultBefore = actionResultBefore.Result as OkObjectResult;
            var totalFlightCountBeforeAdding = ((IEnumerable<FlightReadDto>)resultBefore.Value).Count();

            var flightToAdd = FlightObject.CreateTestFlightObject();
            var flightToAddDto = _mapper.Map<FlightCreateDto>(flightToAdd);
            var actionResult = _flightsController.AddFlight(flightToAddDto);
            var result = actionResult.Result as CreatedAtRouteResult;



            //Getting total flight count after adding new flight
            var actionResultAfter = _flightsController.GetAllFlights();
            var resultAfter = actionResultAfter.Result as OkObjectResult;
            var totalFlightCountAfterAdding = ((IEnumerable<FlightReadDto>)resultAfter.Value).Count();
            Console.WriteLine(totalFlightCountAfterAdding);
            Console.WriteLine(totalFlightCountBeforeAdding);


            //Get if the GetAllFlights() method return status code (200) is working 
            Assert.Equal(resultBefore.StatusCode , 200);

            //Get if the GetAllFlights() method return status code (201 - Created) is working 
            Assert.Equal(result.StatusCode , 201);
            //Assert if the id of added object is the same as the id of the object that was added
            Assert.Equal(((FlightReadDto)result.Value).FlightId , flightToAdd.FlightId);

            //Get if the GetAllFlights() method return status code  (200) is working         
            Assert.Equal(resultBefore.StatusCode , StatusCodes.Status200OK);
            //Assert if the total flight count after adding new flight is equal to the total flight count before adding new flight + 1

            Assert.Equal(totalFlightCountBeforeAdding+1,totalFlightCountAfterAdding);
        });
        t.RunSynchronously();
    }

    [Fact]
    public void AddFlightControllerNullObject()
    {
        FlightCreateDto flightCreateDto = null;
        var actionResult = _flightsController.AddFlight(flightCreateDto);
        var result = actionResult.Result as BadRequestObjectResult;

        Assert.Equal(result.StatusCode , StatusCodes.Status400BadRequest);

    }

    [Fact]
    public void GetFlightByIdController()
    {
        var flightToGet = FlightObject.CreateTestFlightObject();
        var flightToGetDto = _mapper.Map<FlightCreateDto>(flightToGet);
        var actionResult = _flightsController.AddFlight(flightToGetDto);
        var result = actionResult.Result as CreatedAtRouteResult;
        var flightId = ((FlightReadDto)result.Value).FlightId;

        var actionResultGet = _flightsController.GetFlightById(flightId);
        var resultGet = actionResultGet.Result as OkObjectResult;
        var flightGetDto = _mapper.Map<FlightReadDto>(resultGet.Value);

        Assert.Equal(resultGet.StatusCode , StatusCodes.Status200OK);
        Assert.Equal(flightGetDto.FlightId , flightId);
    }

    [Fact]
    public void GetFlightByIdControllerNotFound()
    {


        var actionResultGet = _flightsController.GetFlightById(Guid.Empty);
        var resultGet = actionResultGet.Result as NotFoundResult;

        Assert.Equal(StatusCodes.Status404NotFound ,  resultGet.StatusCode);
    }

    [Fact]
    public void UpdateFlightController()
    {
        var flightToUpdate = FlightObject.CreateTestFlightObject();
        var flightToUpdateDto = _mapper.Map<FlightCreateDto>(flightToUpdate);
        var actionResult = _flightsController.AddFlight(flightToUpdateDto);
        var result = actionResult.Result as CreatedAtRouteResult;
        var flightId = ((FlightReadDto)result.Value).FlightId;

        var flightToUpdate2 = FlightObject.CreateTestFlightObject();
        var flightToUpdateDto2 = _mapper.Map<FlightUpdateDto>(flightToUpdate2);
        var actionResultUpdate = _flightsController.UpdateFlight(flightId, flightToUpdateDto2);
        var resultUpdate = actionResultUpdate as NoContentResult;


        var actionResultGet = _flightsController.GetFlightById(flightId);
        var resultGet = actionResultGet.Result as OkObjectResult;
        var flightGetDto = _mapper.Map<FlightReadDto>(resultGet.Value);

        Assert.Equal(resultGet.StatusCode , 200);
       
       //NoContent status code 204
        Assert.Equal(resultUpdate.StatusCode , 204);

        Assert.Equal(flightGetDto.FlightId , flightId);
        Assert.Equal(flightGetDto.DepartureAirport , flightToUpdate2.DepartureAirport);
        Assert.Equal(flightGetDto.ArrivalAirport , flightToUpdate2.ArrivalAirport);
        Assert.Equal(flightGetDto.DepartureDate.ToLongTimeString() , flightToUpdate2.DepartureDate.ToLongTimeString());
        //Assert.Equal(flightGetDto.ArrivalDate , flightToUpdate2.ArrivalDate);
        Assert.Equal(flightGetDto.Price , flightToUpdate2.Price);
        Assert.Equal(flightGetDto.PricePaid , flightToUpdate2.PricePaid);
        Assert.Equal(flightGetDto.PaymentType , flightToUpdate2.PaymentType);
    }

    [Fact]
    public void UpdateFlightControllerBadRequestIdWithGuidEmpty()
    {
        var flightToUpdate = FlightObject.CreateTestFlightObject();
        var flightToUpdateDto = _mapper.Map<FlightUpdateDto>(flightToUpdate);
        var actionResult = _flightsController.UpdateFlight(Guid.Empty, flightToUpdateDto);
        var result = actionResult as BadRequestObjectResult;

        Assert.Equal(result.StatusCode , StatusCodes.Status400BadRequest);
    }

    [Fact]
    public void UpdateFlightControllerBadRequestObjectNull()
    {

        FlightUpdateDto flightToUpdateDto = null;
        var actionResult = _flightsController.UpdateFlight(Guid.NewGuid(),flightToUpdateDto);
        var result = actionResult as BadRequestObjectResult;

        Assert.Equal(result.StatusCode , StatusCodes.Status400BadRequest);
    }

    [Fact]
    public void DeleteFlightController()
    {
        var flightToDelete = FlightObject.CreateTestFlightObject();
        var flightToDeleteDto = _mapper.Map<FlightCreateDto>(flightToDelete);
        var actionResult = _flightsController.AddFlight(flightToDeleteDto);
        var result = actionResult.Result as CreatedAtRouteResult;
        var flightId = ((FlightReadDto)result.Value).FlightId;

        var actionResultDelete = _flightsController.DeleteFlight(flightId);
        var resultDelete = actionResultDelete as NoContentResult;

        var actionResultGet = _flightsController.GetFlightById(flightId);
        var resultGet = actionResultGet.Result as NotFoundResult;

        Assert.Equal(resultGet.StatusCode , StatusCodes.Status404NotFound);
        Assert.Equal(resultDelete.StatusCode , StatusCodes.Status204NoContent);
    }

    [Fact]
    public void DeleteFlightControllerNotFoundFlightId()
    {
        var actionResultDelete = _flightsController.DeleteFlight(Guid.Empty);
        var resultDelete = actionResultDelete as NotFoundResult;

        Assert.Equal(resultDelete.StatusCode , StatusCodes.Status404NotFound);
    }

    [Fact]
    public void GetAllFlightsController_WithSearchString()
    {
        var flightToGet = FlightObject.CreateTestFlightObject();
        var flightToGetDto = _mapper.Map<FlightCreateDto>(flightToGet);
        _flightsController.AddFlight(flightToGetDto);

        var actionResult = _flightsController.GetAllFlights(flightToGetDto.FlightNumber);
        var result = actionResult.Result as OkObjectResult;
        var flightReadDto = _mapper.Map<List<FlightReadDto>>(result.Value);

        Assert.Equal(result.StatusCode , StatusCodes.Status200OK);
        Assert.Equal(flightReadDto.Count , 1);

    }


}