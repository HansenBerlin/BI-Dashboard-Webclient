using BI_Core;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BI_Dashboard_Webclient.Components;

public partial class PerformerInfo
{
    [Parameter]
    public ScoresModelInfo ScoresModel { get; set; }

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
}