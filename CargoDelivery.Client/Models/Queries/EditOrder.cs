namespace CargoDelivery.Client.Models.Queries;

public class EditOrder
{
    public Guid Id { get; set; }
    public string TakeAddress { get; set; }
    public DateTime TakeDateTime { get; set; }
    public string DestinationAddress { get; set; }
    public DateTime DestinationDateTime { get; set; }
}