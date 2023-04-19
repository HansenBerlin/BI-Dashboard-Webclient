namespace BI_Core;

public class ImmoRentDataModelBase : ResponseBase
{
    public int Id { get; set; }
    public double Lon { get; set; }
    public double Lat { get; set; }
    public double PricePerSqMBase { get; set; }
    public double PricePerSqMService { get; set; }
    public double PricePerSqMTotal { get; set; }
    public string Condition { get; set; }
    public string InteriorQual { get; set; }
    public string TypeOfFlat { get; set; }
    public string HeatingType { get; set; }
    public int YearConstructed { get; set; }
}