namespace BI_Core;

public class GenericRawDataModel : ResponseBase
{
    public int Id { get; set; }
    public double Lon { get; set; }
    public double Lat { get; set; }
    public string? GenericProperty { get; set; }
}