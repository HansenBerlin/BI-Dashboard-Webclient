namespace BI_Core;

public class RentDashboardModel : ResponseBase
{
    public int AverageBaseRent { get; set; }
    public int AverageLivingSpace { get; set; }
    public long TotalBaseRentIncomePerMonth { get; set; }
    public int TotalApartments { get; set; }
    public double AveragePricePerSqm { get; set; }
    public int AverageHouseAge { get; set; }
    public string MostCommonHeatingType { get; set; }
    public string MostCommonInteriorQual { get; set; }
    public string MostCommonTypeOfFlat { get; set; }
    public string MostCommonCondition { get; set; }
}

