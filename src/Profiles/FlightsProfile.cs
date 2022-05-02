using AutoMapper;

public class FlightsProfile : Profile
{
    public FlightsProfile()
    {
        CreateMap<Flight,FlightReadDto>();
        CreateMap<FlightCreateDto,Flight>();
        CreateMap<FlightUpdateDto,Flight>();
        CreateMap<Flight,FlightSearchDto>()
        //Change to string to search
            .ForMember(dest => dest.DepartureDate, opt => opt.MapFrom(src => src.DepartureDate.ToLongTimeString()))
            .ForMember(dest => dest.ArrivalDate  , opt => opt.MapFrom(src => src.ArrivalDate  .ToLongDateString()))
            .ForMember(dest => dest.Price        , opt => opt.MapFrom(src => src.Price        .ToString()))
            .ForMember(dest => dest.PricePaid    , opt => opt.MapFrom(src => src.PricePaid    .ToString()))
            .ForMember(dest => dest.PaymentType  , opt => opt.MapFrom(src => Enum.GetName(src.PaymentType) .ToString()));

        ;
        CreateMap<FlightSearchDto,FlightReadDto>();

        CreateMap<Airline,AirlineCreateDto>();

    }
}