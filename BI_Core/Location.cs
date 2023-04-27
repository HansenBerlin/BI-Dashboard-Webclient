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

public class RentScoresModel : ScoresModelBase
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

public class BuyScoresModel : ResponseBase
{
    public int Id { get; set; }
    public double Score { get; set; }
    public double ScLivingspaceSqmPrice { get; set; }
    public double ScLotareaSqmPrice { get; set; }
    public double ScPurchasePrice { get; set; }
    public double ScPricetrend { get; set; }
    public double ScCondition { get; set; }
    public double ScFiringType { get; set; }
    public int YearConstructed { get; set; }
    public int LotArea { get; set; }
    public int LivingSpace { get; set; }
    public string Condition { get; set; } = "";
    public int NoRooms { get; set; }
    public int PurchasePrice { get; set; }
    public int PricePerSqmLivingSpace { get; set; }
    public int PricePerSqmLotArea { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
}