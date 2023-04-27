using BI_Core;
using BI_Core.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BI_Dashboard_Webclient.Components;

public partial class PerformerInfoBuy
{
    [Parameter]
    public BuyScoresModel BuyScoresModel { get; set; }
    
    [Inject]
    public RealEstateBookmark RealEstateBookmark { get; set; }
    
    [Inject] 
    public ISnackbar SnackbarService { get; set; }

    private Color MapColorFromScore(double score)
    {
        return score switch
        {
            > 0.85 => Color.Success,
            > 0.55 => Color.Info,
            > 0.25 => Color.Warning,
            _ => Color.Error
        };
    }

    private void AddModelToBookMarks()
    {
        if (RealEstateBookmark.IsAdded(BuyScoresModel))
        {
            SnackbarService.Add("Bookmark hinzugfügt", Severity.Info);
        }
    }
}