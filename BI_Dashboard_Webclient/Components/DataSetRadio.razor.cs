using BI_Core.Enums;
using Microsoft.AspNetCore.Components;

namespace BI_Dashboard_Webclient.Components;

public partial class DataSetRadio
{
    [Parameter] 
    public EventCallback<DataSet> DataSetSelectionHasChanged { get; set; }

    [Parameter] 
    public bool ShowAggregateOptions { get; set; }

    private DataSet _selectedDataSet = DataSet.Rent;

    private async Task OnSelectionChange(DataSet datset)
    {
        _selectedDataSet = datset;
        await DataSetSelectionHasChanged.InvokeAsync(datset);
    }
}