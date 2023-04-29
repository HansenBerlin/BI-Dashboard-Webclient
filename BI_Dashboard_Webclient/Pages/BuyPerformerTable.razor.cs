using BI_Core;
using Infrastructure;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BI_Dashboard_Webclient.Pages;

public partial class BuyPerformerTable
{
    [Inject]
    public Repository<BuyScoresModel> Repository { get; set; }

    private int selectedRowNumber = -1;
    private MudTable<BuyScoresModel> mudTable;
    private List<string> clickedEvents = new();
    private IEnumerable<BuyScoresModel> ElementsTop = new List<BuyScoresModel>();
    private IEnumerable<BuyScoresModel> ElementsFlop = new List<BuyScoresModel>();
    private BuyScoresModel _rentScoresModel = new();
    
    private void RowClickEvent(TableRowClickEventArgs<BuyScoresModel> tableRowClickEventArgs)
    {
        _rentScoresModel = tableRowClickEventArgs.Item;
    }

    private string SelectedRowClassFunc(BuyScoresModel element, int rowNumber)
    {
        if (selectedRowNumber == rowNumber)
        {
            selectedRowNumber = -1;
            clickedEvents.Add("Selected Row: None");
            return string.Empty;
        }
        if (mudTable.SelectedItem != null && mudTable.SelectedItem.Equals(element))
        {
            selectedRowNumber = rowNumber;
            clickedEvents.Add($"Selected Row: {rowNumber}");
            return "selected";
        }
        return string.Empty;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            const string url = "/buydata/scores/info";
            Repository.Init(url);
            ElementsTop = await Repository.GetAll("true");
            var response = await Repository.GetAll("false");
            ElementsFlop = response.OrderBy(x => x.Score);
            _rentScoresModel = ElementsTop.FirstOrDefault() ?? new BuyScoresModel();
            StateHasChanged();
        }
    }
    
}