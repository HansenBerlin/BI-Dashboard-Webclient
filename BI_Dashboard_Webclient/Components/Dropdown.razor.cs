using BI_Core.Enums;
using BI_Dashboard_Webclient.Models;
using BI_Dashboard_Webclient.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BI_Dashboard_Webclient.Components;

public partial class Dropdown
{
    [Parameter]
    public EventCallback<SelectionDataSetOptions> SelectedValueHasChanged { get; set; }
    
    [Parameter] 
    public DataType DataType { get; set; }

    [Parameter] 
    public string InitialSelected { get; set; } = "";
    
    [Inject]
    public DropdownViewModel Vm { get; set; }

    protected override Task OnParametersSetAsync()
    {
        Vm.InitTyp(DataType);
        Vm.Selected = InitialSelected;
        return base.OnParametersSetAsync();
    }

    private async Task OnSelectionChanged(string value)
    {
        var selection = new SelectionDataSetOptions()
        {
            Column = value,
            DataSet = MapToDataset()
        };
        
        await SelectedValueHasChanged.InvokeAsync(selection);
    }

    private DataSet MapToDataset()
    {
        return DataType switch
        {
            DataType.Demographic => DataSet.Demographic,
            DataType.Regional => DataSet.Economic,
            DataType.BuyAggregate or DataType.BuyCategorial or DataType.BuyNumerical => DataSet.Buy,
            _ => DataSet.Rent
        };
    }
}