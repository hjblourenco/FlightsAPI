using AutoMapper;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class AirlinesController : ControllerBase
{
    private readonly IAirlinesRepository _airlinesRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AirlinesController> _logger;

    public AirlinesController(
        IAirlinesRepository airlinesRepository,
        IMapper mapper,
        ILogger<AirlinesController> logger)
        {
            _airlinesRepository = airlinesRepository;
            _mapper = mapper;
            _logger = logger;
        }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AirlineReadDto>>> GetAllAirlines()
    {
        var airlines = await _airlinesRepository.GetAllAirlinesAsync();
        var airlinesReadDto = _mapper.Map<IEnumerable<AirlineReadDto>>(airlines);
        return Ok(airlinesReadDto);
    }

    [HttpGet("{id}", Name = "GetAirlineById")]
    public async Task<ActionResult<AirlineReadDto>> GetAirlineById(Guid airlineId)
    {
        var airline = await _airlinesRepository.GetAirlineAsync(airlineId);
        if (airline == null)
        {
            _logger.LogInformation($"Airline with id: {airlineId} was not found when accessing GetAirlineByIdAsync");
            return NotFound();
        }
        var airlineReadDto = _mapper.Map<AirlineReadDto>(airline);
        return Ok(airlineReadDto);
    }
    
    [HttpPost]
    public ActionResult<AirlineReadDto> CreateAirline(AirlineCreateDto airlineCreateDto)
    {
        var airline = _mapper.Map<Airline>(airlineCreateDto);
        if (airline == null || airline.Name == null)
        {
            _logger.LogInformation($"Airline with id: {airlineCreateDto.AirlineId} was not found when accessing CreateAirline");
            return BadRequest($"Airline with id: {airlineCreateDto.AirlineId} was not found");
        }

        _airlinesRepository.AddAirlineAsync(airline);
        _airlinesRepository.Save();

        var airlineReadDto = _mapper.Map<AirlineReadDto>(airline);
        return CreatedAtRoute(nameof(GetAirlineById), new { id = airlineReadDto.AirlineId }, airlineReadDto);
    }

    [HttpPut("{airlineId}")]
    public IActionResult UpdateAirline(Guid airlineId, AirlineUpdateDto airlineUpdateDto)
    {
        var airline = _mapper.Map<Airline>(airlineUpdateDto);
        airline.AirlineId = airlineId;

        if (airlineUpdateDto == null || airlineUpdateDto.Name == null)
        {
            _logger.LogInformation($"Airline is null");
            return BadRequest($"Airline with id: {airlineId} was not found");
        }

        if ( ! _airlinesRepository.AirlineExistsAsync(airlineId))
        {
            _logger.LogInformation($"Airline with id: {airlineId} was not found when accessing UpdateAirline");
            return NotFound($"Airline with id: {airlineId} was not found");
        }

        _airlinesRepository.UpdateAirlineAsync(airlineId, airline);
        _airlinesRepository.Save();
        return NoContent();
    }

    [HttpDelete("{airlineId}")]
    public ActionResult DeleteAirline(Guid airlineId)
    {
        if ( ! _airlinesRepository.AirlineExistsAsync(airlineId))
        {
            _logger.LogInformation($"Airline with id: {airlineId} was not found when accessing AirlineExists");
            return NotFound($"Airline with id: {airlineId} was not found");
        }


        _airlinesRepository.DeleteAirlineAsync(airlineId);
        _airlinesRepository.Save();
        return NoContent();
    }

}