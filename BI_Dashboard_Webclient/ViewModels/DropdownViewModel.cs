using BI_Core.Enums;
using BI_Dashboard_Webclient.Models;

namespace BI_Dashboard_Webclient.ViewModels;

public class DropdownViewModel
{
    public string Selected { get; set; } 
    private readonly DropdownModel _model;
    private DataType _dataType = DataType.Unset;

    public DropdownViewModel(DropdownModel model)
    {
        _model = model;
    }

    public void InitTyp(DataType dataType)
    {
        _dataType = dataType;
    }

    public Dictionary<string, string> GetDropdownData()
    {
        TypeIsSetCheck();
        return _dataType switch
        {
            DataType.BuyNumerical or DataType.BuyCategorial => _model.Buy,
            DataType.RentNumerical or DataType.RentCategorial => _model.Rent,
            DataType.Regional => _model.Regions,
            _ => new Dictionary<string, string>()
        };
    }

    public string GetHelperText()
    {
        TypeIsSetCheck();
        return _dataType switch
        {
            DataType.BuyNumerical or DataType.RentNumerical 
                or DataType.BuyAggregate or DataType.RentAggregate => "numerische Spalte auswählen",
            DataType.BuyCategorial or DataType.RentCategorial => "kategoriale Spalte auswählen",
            _ => ""
        };
    }
    
    public string GetLabel()
    {
        TypeIsSetCheck();
        return _dataType switch
        {
            DataType.BuyNumerical or DataType.BuyCategorial => "Datenset Kaufangebote",
            DataType.RentNumerical or DataType.RentCategorial => "Datenset Mietbestand",
            DataType.Regional => "Datenset Regionaldaten",
            _ => ""
        };
    }

    private void TypeIsSetCheck()
    {
        if (_dataType == DataType.Unset)
        {
            throw new TypeInitializationException("DataType",
                new Exception("Data type is unset. Call the init method and pass " +
                              "the datatype argument before calling this method."));
        }
    }
}