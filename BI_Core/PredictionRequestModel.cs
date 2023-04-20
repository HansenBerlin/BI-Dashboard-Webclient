namespace BI_Core;

public class PredictionRequestModel
{
    public double BaseRent { get; set; }
    public int LivingSpace { get; set; } = 100;
    public int NoRooms { get; set; } = 1;
    public int Condition { get; set; }
    public int InteriorQual	{ get; set; }
    public int Lift	{ get; set; }
    public int TypeOfFlat { get; set; }
    public int Garden { get; set; }
    public int HeatingType { get; set; }
    public int YearConstructed { get; set; }
    public double Lon { get; set; }
    public double Lat { get; set; }
}