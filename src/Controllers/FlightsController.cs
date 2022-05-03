using AutoMapper;
using Helpers.Pagination;
using Helpers.QueryStringParameters;
//using Helpers.Pagination;
//using Helpers.QueryStringParameters;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IFlightsRepository _flightsRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<FlightsController> _logger;

    public FlightsController(IFlightsRepository flightsRepository,
                            IMapper mapper,
                            ILogger<FlightsController> logger)
    {

        _flightsRepository = flightsRepository;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<FlightReadDto>> GetAllFlights([FromQuery] string? searchString=null,[FromQuery] string? orderBy=null, [FromQuery] QueryStringParameters? queryStringParameters=null)

    {

        _logger.LogDebug("--> Getting GetAllFlights...");        

        var flights = _flightsRepository.GetAllFlights();

        //Map to the readDto because searchstring could be empty
        var flightsDto = _mapper.Map<IEnumerable<FlightReadDto>>(flights);

        //Search
        //api/flights?searchString=[SearchString]
        if (searchString!=null)
        {
            var FlightSearchDto = _mapper.Map<IEnumerable<FlightSearchDto>>(flights);
            ISearch<FlightSearchDto> Search =  new Search<FlightSearchDto>();
            var flightsSearchDto = Search.ApplySearch(FlightSearchDto, searchString);
            //Map to the return Dto
            flightsDto = _mapper.Map<IEnumerable<FlightReadDto>>(flightsSearchDto);
        }

        //Sort
        //api/flights?orderBy=[OrderByQueryString]
        //Separate by , (comma) to sort by multiple properties and after the property name add " desc" to sort descending
        if (orderBy != null)
        {
            ISort<FlightReadDto> _sortHelper = new Sort<FlightReadDto>();
            flightsDto = _sortHelper.ApplySort(flightsDto.AsQueryable<FlightReadDto>(), orderBy).ToList();
        }



        //The classes tha apply the paging algorythm
        //https://code-maze.com/paging-aspnet-core-webapi/
        //In my tests It will work with wrong parameters
        //api/flights?pageNumber=[PageNumber]&pageSize=[PageSize]  
        Pagination<FlightReadDto> flightsPaged;
        if (queryStringParameters!=null)
        {
            flightsPaged = Pagination<FlightReadDto>.ToPagedList(
                flightsDto.AsQueryable<FlightReadDto>(),
                queryStringParameters.PageNumber,
                queryStringParameters.PageSize
            );

            flightsDto = flightsPaged.Items;
        }

        return Ok(flightsDto);
    }


    [HttpGet("{flightId}", Name = "GetFlightById")]
    public ActionResult<FlightReadDto> GetFlightById(Guid flightId)
    {
        _logger.LogDebug("--> Getting GetFlightById...");   

        var flight = _flightsRepository.GetFlightById(flightId);
        if (flight == null)
        {
            return NotFound();
        }

        var flightReadDto = _mapper.Map<FlightReadDto>(flight);

        return Ok(flightReadDto);
    }

    [HttpPost]
    public ActionResult<FlightReadDto> AddFlight(FlightCreateDto flightCreateDto)
    {
        _logger.LogDebug("--> Add AddFlight...");   

        if (flightCreateDto == null)
        {
            return BadRequest("Flight is null");
        }

        var flight = _mapper.Map<Flight>(flightCreateDto);
        
        _flightsRepository.AddFlight(flight);
        _flightsRepository.Save();

        var flightReadDto = _mapper.Map<FlightReadDto>(flight);

        return CreatedAtRoute(nameof(GetFlightById),new {FlightId= flightReadDto.FlightId}, flightReadDto);
    }

    [HttpPut("{flightId}")]
    public ActionResult UpdateFlight(Guid flightId, FlightUpdateDto flightUpdateDto)
    {
        _logger.LogDebug("--> Updating UpdateFlight...");   

        if (flightUpdateDto == null)
        {
            return BadRequest("Flight to update is null");
        }

        if (flightId == Guid.Empty)
        {
            return BadRequest("FlightId mismatch");
        }


        var flight = _mapper.Map<Flight>(flightUpdateDto);
        flight.FlightId = flightId;

        _flightsRepository.UpdateFlight(flightId,flight);
        _flightsRepository.Save();

        return NoContent();
    }

    [HttpDelete("{flightId}")]
    public ActionResult DeleteFlight(Guid flightId)
    {
        _logger.LogDebug("--> Deleting DeleteFlight...");   

        var flight = _flightsRepository.GetFlightById(flightId);
        if (flight == null)
        {
            return NotFound();
        }

        _flightsRepository.DeleteFlight(flightId);
        _flightsRepository.Save();

        return NoContent();
    }
}
