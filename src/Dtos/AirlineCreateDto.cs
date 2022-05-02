public class AirlineCreateDto
{

    public Guid AirlineId { get; set; }  = Guid.NewGuid();

    public string Name { get; set; } 

}