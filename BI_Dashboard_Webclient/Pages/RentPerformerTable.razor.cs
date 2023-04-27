using BI_Core;
using Infrastructure;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BI_Dashboard_Webclient.Pages;

public partial class RentPerformerTable
{
    [Inject]
    public Repository<RentScoresModel> Repository { get; set; }

    private int selectedRowNumber = -1;
    private MudTable<RentScoresModel> mudTable;
    private List<string> clickedEvents = new();
    private IEnumerable<RentScoresModel> ElementsTop = new List<RentScoresModel>();
    private IEnumerable<RentScoresModel> ElementsFlop = new List<RentScoresModel>();
    private RentScoresModel _rentScoresModel = new();
    
    private void RowClickEvent(TableRowClickEventArgs<RentScoresModel> tableRowClickEventArgs)
    {
        _rentScoresModel = tableRowClickEventArgs.Item;
    }

    private string SelectedRowClassFunc(RentScoresModel element, int rowNumber)
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
            const string url = "https://localhost:6001/rentdata/scores/info";
            Repository.Init(url);
            ElementsTop = await Repository.GetAll("true");
            var response = await Repository.GetAll("false");
            ElementsFlop = response.OrderBy(x => x.Score);
            _rentScoresModel = ElementsTop.FirstOrDefault() ?? new RentScoresModel();
            StateHasChanged();
        }
    }
    
}