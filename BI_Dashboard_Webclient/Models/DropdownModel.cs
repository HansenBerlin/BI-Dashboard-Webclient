namespace BI_Dashboard_Webclient.Models;

public class DropdownModel
{
    public Dictionary<string, string> Regions { get; } = new()
    {
        {"buildingPermits", "Baugenehmigungen"},
        {"landPrices", "Bodenpreise"},
        {"householdIncome2019", "Haushaltseinkommen"},
        {"consumerInsolvencies", "Insolvenzen"}
    };
    
    public Dictionary<string, string> Rent { get; } = new()
    {
        {"priceTrend", "Preistrend"},
        {"totalRent", "Warmmiete"},
        {"serviceCharge", "Nebenkosten"},
        {"baseRent", "Kaltmiete"},
        {"livingSpace", "Wohnungsgröße"},
        {"noRooms", "Zimmeranzahl"},
        {"noParkingSpaces", "Parkplätze"},
        {"telekomUploadSpeed", "verfügbare Bandbreite"},
        {"yearConstructed", "Baujahr"},
        {"pricepersqmbase", "QM Preis kalt"},
        {"pricepersqmservice", "QM Preis Nebenkosten"},
        {"pricepersqmtotal", "QM Preis warm"}
    };
    
    public Dictionary<string, string> Buy { get; } = new()
    {
        {"pricetrendbuy", "Preistrend"},
        {"lotArea", "Grundstücksgröße"},
        {"livingSpace", "Wohnfläche"},
        {"yearConstructed", "Baujahr"},
        {"noRooms", "Zimmeranzahl"},
        {"purchasePrice", "Kaufpreis"},
        {"telkomUploadSpeed", "verfügbare Bandbreite"},
        {"pricepersqmlotarea", "QM Preis Grundstückfläche"},
        {"pricepersqmlivingspace", "QM Preis Wohnfläche"}
    };
}