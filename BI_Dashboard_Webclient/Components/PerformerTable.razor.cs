using BI_Core;
using Infrastructure;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BI_Dashboard_Webclient.Components;

public partial class PerformerTable
{
    [Inject]
    public Repository<ScoresModelInfo> Repository { get; set; }

    private int selectedRowNumber = -1;
    private MudTable<ScoresModelInfo> mudTable;
    private List<string> clickedEvents = new();
    private IEnumerable<ScoresModelInfo> ElementsTop = new List<ScoresModelInfo>();
    private IEnumerable<ScoresModelInfo> ElementsFlop = new List<ScoresModelInfo>();
    private ScoresModelInfo _scoresModel = new();
    
    private void RowClickEvent(TableRowClickEventArgs<ScoresModelInfo> tableRowClickEventArgs)
    {
        _scoresModel = tableRowClickEventArgs.Item;
    }

    private string SelectedRowClassFunc(ScoresModelInfo element, int rowNumber)
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
            _scoresModel = ElementsTop.FirstOrDefault() ?? new ScoresModelInfo();
            StateHasChanged();
        }
    }
    
}