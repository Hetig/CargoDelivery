namespace CargoDelivery.API.Dtos;

public class OrderCreateDto
{
    public ClientDto Client { get; init; }
    public CargoDto Cargo { get; init; }
    public string TakeAddress { get; init; }
    public DateTime TakeDateTime { get; init; }
    public CourierDto Courier { get; init; }
    public string DestinationAddress { get; init; }
    public DateTime DestinationDateTime { get; init; }
}