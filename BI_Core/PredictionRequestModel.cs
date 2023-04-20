namespace BI_Core;

public class PredictionRequestModel
{
    public double BaseRent { get; set; } = 0;
    public int LivingSpace { get; set; } = 100;
    public int NoRooms { get; set; } = 2;
    public int Condition { get; set; } = 1;
    public int InteriorQual { get; set; } = 3;
    public int Lift { get; set; } = 0;
    public int TypeOfFlat { get; set; } = 4;
    public int Garden { get; set; } = 0;
    public int HeatingType { get; set; } = 10;
    public int YearConstructed { get; set; } = 2000;
    public double Lon { get; set; }
    public double Lat { get; set; }
}

public class PredictionResponseModel : PredictionRequestModel
{
    public double Score { get; set; }
    public double[] Features { get; set; }
}