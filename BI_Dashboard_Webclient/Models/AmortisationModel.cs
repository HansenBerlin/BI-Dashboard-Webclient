namespace BI_Dashboard_Webclient.Models;

public class AmortisationModel
{
    public double PurchasePrice { get; set; } = 200_000;
    public double AdditionalPurchaseCosts { get; set; } = 0.10;
    public double MaintenanceCosts { get; set; } = 0.015;
    public double AdministrativeCosts { get; set; } = 300;
    public double BaseRentMonthlyIncome { get; set; } = 1000;
    public double RentIncreasePerYear { get; set; } = 0.04;
    public int Years { get; set; }

    public void CalculateAmortisation()
    {
        double resultTop = Math.Sqrt(
                Math.Pow(AdministrativeCosts,2) + 2
                * (PurchasePrice * MaintenanceCosts - 6 * BaseRentMonthlyIncome * (RentIncreasePerYear + 2)) 
                * AdministrativeCosts + Math.Pow(PurchasePrice,2) * Math.Pow(MaintenanceCosts,2) - 12 * PurchasePrice 
                * (MaintenanceCosts * (RentIncreasePerYear + 2) - 2 * (AdditionalPurchaseCosts + 1) * RentIncreasePerYear) 
                * BaseRentMonthlyIncome + 36 * Math.Pow(BaseRentMonthlyIncome,2) * Math.Pow(RentIncreasePerYear + 2,2))
            + AdministrativeCosts + PurchasePrice * MaintenanceCosts - 6 * BaseRentMonthlyIncome * (RentIncreasePerYear + 2);
        double result = resultTop / (12 * BaseRentMonthlyIncome * RentIncreasePerYear);
        Years = (int)Math.Round(result, 0);
    }
    
}