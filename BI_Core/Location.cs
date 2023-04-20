namespace BI_Core;

public class ScoresModelBase : ResponseBase
{
    public int Id { get; set; }
    public double Score { get; set; }
    public double RentMarginScore { get; set; }
    public double PriceSmScore { get; set; }
    public double HeatingScore { get; set; }
    public double ConditionScore { get; set; }
}

public class Location : ScoresModelBase
{
    public double Lat { get; set; }
    public double Lon { get; set; }
}

public class ScoresModelInfo : ScoresModelBase
{
    public int LivingSpace { get; set; }
    public string Condition { get; set; } = "";
    public string InteriorQual { get; set; } = "";
    public string TypeOfFlat { get; set; } = "";
    public string HeatingType { get; set; } = "";
    public int YearConstructed { get; set; }
    public double PricePerSqmBase { get; set; }
    public double PricePerSqmService { get; set; }
}