using BI_Core;
using BI_Core.Enums;
using BI_Dashboard_Webclient.Models;
using BI_Dashboard_Webclient.ViewModels;
using Infrastructure;
using Microsoft.AspNetCore.Components;
using Plotly.Blazor;
using Plotly.Blazor.ConfigLib;
using Plotly.Blazor.LayoutLib;
using Plotly.Blazor.LayoutLib.ColorAxisLib;
using Plotly.Blazor.LayoutLib.ColorAxisLib.ColorBarLib;
using Plotly.Blazor.LayoutLib.MapBoxLib;
using Plotly.Blazor.Traces;
using Plotly.Blazor.Traces.ScatterMapBoxLib;

namespace BI_Dashboard_Webclient.Pages;

public partial class Map
{
    private DataSet _selectedAggregateDataset = DataSet.Rent;
    private DataSet _selectedOverlayDataset = DataSet.Rent;
    
    private string selectedColorValue = "";
    
    [Inject]
    public DataStatefulRepository StatefulRepository { get; set; }
    
    [Inject]
    public Repository<GenericAggregatesModel> AggregatesRepository { get; set; }
    
    [Inject]
    public Repository<GenericRawDataModel> ImmoDataRepository { get; set; }
    
    [Inject]
    public MapViewModel Vm { get; set; }
    
    private const string Token = "pk.eyJ1IjoiaGFuc2VuYXVzYmVybGluIiwiYSI6ImNsZzRjNm51eDBvM3gzbHFlbzd1YzFucnQifQ.jZmB_ZdTHVkSlVddks7XfQ";
    private PlotlyChart _plotlyChart;
    private bool _isInitDone;
    private ScatterMapBox _scatterMap;
    private ChoroplethMapBox _choromap;
    //private List<GenericRegionsModel> _regionsDataModels;
    private Config _config = new();
    private Layout _layout = new();
    private IList<ITrace> _data = new List<ITrace>();


    public override async Task SetParametersAsync(ParameterView parameters)
    {
        string url = "https://localhost:6001/aggregates";
        AggregatesRepository.Init(url);
        url = "https://localhost:6001/raw";
        ImmoDataRepository.Init(url);

        var geoJsonData = await StatefulRepository.Get();

        _config.MapboxAccessToken = Token;
        _choromap = new ChoroplethMapBox
        {
            GeoJson = geoJsonData,
            FeatureIdKey = "properties.AGS",
            ColorAxis = "coloraxis",
            Below = "scatter", 
            UId = "base", 
            Name = "base", 
            Marker = new Plotly.Blazor.Traces.ChoroplethMapBoxLib.Marker
            {
                Opacity = 0.6M
            }
        };

        _scatterMap = new ScatterMapBox
        {
            Mode = ModeFlag.Markers, 
            //Opacity = 0.7M, 
            Name = "scatterdata", 
            Text = "hello",
            HoverText = "test", 
            UId = "scatter",
            Visible = VisibleEnum.True,
            Marker = new Marker
            {
                ColorAxis = "coloraxis2",
                //ColorArray = new List<object>(), 
                //Size = _selectedScatterMarkerSize,
                Size = 6,
                Opacity = 0.9M
            }
        };

        _layout.MapBox = new List<MapBox>
        {
            new()
            {
                Style = "carto-positron",
                Center = new Center
                {
                    Lon = 10.4515m,
                    Lat = 51.1657m
                },
                Zoom = 5.7m
            }
        };
        _layout.ColorAxis = new List<ColorAxis>
        {
            new()
            {
                ColorScale = "Cividis",
                ShowScale = true,
                ColorBar = new ColorBar
                {
                    YAnchor = 0, 
                    Len = 0.8m,
                    Y = 0.9m,
                    X = 0.05m,
                    TickLabelPosition = TickLabelPositionEnum.OutsideLeft,
                    BgColor = "#ffffff88",
                    BorderColor = "#99999999"
                }
            },
            new()
            {
                ColorScale = "Jet",
                ShowScale = true,
                ColorBar = new ColorBar
                {
                    YAnchor = 0,
                    Y = 0.9m, 
                    Len = 0.8m,
                    X = 0.9m,
                    //TickLabelPosition = TickLabelPositionEnum.OutsideRight,
                    BgColor = "#ffffff88",
                    BorderColor = "#99999999"
                }
            }
        };
        
        _layout.Margin = new Margin
        {
            B = 0,
            L = 0,
            R = 0,
            T = 0,
            Pad = 0,
            AutoExpand = true
        };

        _config.MapboxAccessToken = Token;
        _config.Responsive = true;
        _config.DisplayLogo = false;
        _config.DisplayModeBar = DisplayModeBarEnum.False;
        _config.Editable = false;

        _data.Add(_choromap);
        _data.Add(_scatterMap);

        //StateHasChanged();

        await base.SetParametersAsync(ParameterView.Empty);
    }

    protected override async Task OnAfterRenderAsync(bool isFirstRender)
    {
        Console.WriteLine("------------------ RENDERED ---------------------------------");
        if (isFirstRender)
        {
            try
            {
                //await _plotlyChart.AddTrace(_choromap);
                //await _plotlyChart.AddTrace(_scatterMap);
                //await _plotlyChart.Relayout();
            }
            catch (Exception) { Console.WriteLine("-------");}
            _isInitDone = true;
            await InvokeAsync(StateHasChanged);
        }
    }
    
    private async Task DatasetSelectionChangedCallback(DataSet dataset, bool isAggregate)
    {
        if (isAggregate)
        {
            _selectedAggregateDataset = dataset;
        }
        else
        {
            _selectedOverlayDataset = dataset;
        }

        //await InvokeAsync(StateHasChanged);
    }

    private async Task UpdateMarkerSize()
    {
        _scatterMap.Marker.Size = _selectedScatterMarkerSize;
        Console.WriteLine(_selectedScatterMarkerSize);
        //await _plotlyChart.NewPlot();
        await _plotlyChart.Update();
        
        await InvokeAsync(StateHasChanged);
    }

    private async Task Download()
    {
        await _plotlyChart.DownloadImage(ImageFormat.Png, 1000, 1000, "map" + Guid.NewGuid());
    }

    private string _selectedOverlayValue = "";
    //private string _selectedOverlaySize = "";
    private string _selectedBaseValue = "";
    private int _selectedScatterMarkerSize = 6;

    private async Task OverlaySizeValueChanged(SelectionDataSetOptions selected)
    {
        _isInitDone = false;
        _selectedOverlayValue = selected.Column;
        string appendToUrl = $"{selected.DataSet.ToString()}/{selected.Column}";
        var scatterOverlayDataSize = await ImmoDataRepository.GetAll(appendToUrl);
        
        _scatterMap.Marker.SizeArray = scatterOverlayDataSize
            .Select(x => decimal
                .Parse(x.GenericProperty))
            .ToList() as IList<decimal?>;
        //await _plotlyChart.NewPlot();
        await _plotlyChart.Update();
        _isInitDone = true;
        await InvokeAsync(StateHasChanged);

    }

    private async Task OverlayColorValueChanged(SelectionDataSetOptions selected)
    {
        _isInitDone = false;
        _selectedOverlayValue = selected.Column;

        string appendToUrl = $"{selected.DataSet.ToString().ToLower()}/{selected.Column}";
        var scatterOverlayDataColor = await ImmoDataRepository.GetAll(appendToUrl);
        
        if (scatterOverlayDataColor[0].IsResponseSuccess == false)
        {
            _isInitDone = true;
            StateHasChanged();
            return;
        }
        
        _scatterMap.Marker.ColorArray = scatterOverlayDataColor
            .Select(x => (object) x.GenericProperty)
            .ToList();
        
        _scatterMap.Lat = scatterOverlayDataColor
            .Select(x => (object) x.Lat)
            .ToList();
        _scatterMap.Lon = scatterOverlayDataColor
            .Select(x => (object) x.Lon)
            .ToList();
        
        (decimal min, decimal max) = CalculateMinMax(scatterOverlayDataColor
            .Select(x => double.Parse(x.GenericProperty))
            .ToList());
        
        _layout.ColorAxis[1].CMax = max;
        _layout.ColorAxis[1].CMin = min;
        //_plotlyChart.Layout.ColorAxis[1].ColorBar.Title.Text = _selectedOverlayValue;

        //await _plotlyChart.React();
        await _plotlyChart.NewPlot();
        //await _plotlyChart.Update();
        _isInitDone = true;
        //await InvokeAsync(StateHasChanged);

    }

   
    
    private async Task BaseValueChanged(SelectionDataSetOptions selected)
    {
        _isInitDone = false;
        _selectedBaseValue = selected.Column;
        string appendToUrl = $"{selected.DataSet.ToString().ToLower()}/{selected.Column}";
        var choroplethOverlayData = await AggregatesRepository.GetAll(appendToUrl);
        if (choroplethOverlayData[0].IsResponseSuccess == false)
        {
            _isInitDone = true;
            return;
        }
        
        (decimal min, decimal max) = CalculateMinMax(choroplethOverlayData
            .Select(x => double.Parse(x.GenericProperty))
            .ToList());

        _choromap.Z = choroplethOverlayData
            .Select(x => (object)x.GenericProperty)
            .ToList();
        _choromap.Locations = choroplethOverlayData
            .Select(x => (object)x.AgsKey)
            .ToList();
        
        _layout.ColorAxis[0].CMax = max;
        _layout.ColorAxis[0].CMin = min;
        //_plotlyChart.Layout.ColorAxis[0].ColorBar.Title.Text = _selectedBaseValue;

        await _plotlyChart.Update();
        //await _plotlyChart.NewPlot();
        _isInitDone = true;
        //await InvokeAsync(StateHasChanged);

    }

    private async Task ColorChanged(string toColor, int index)
    {
        _plotlyChart.Layout.ColorAxis[index].ColorScale = toColor;
        await _plotlyChart.Update();
        await InvokeAsync(StateHasChanged);
    }

    private (decimal min, decimal max) CalculateMinMax(List<double> values)
    {
        double median = CalculateMedian(values);
        var max = values.Max(x => x);
        var min = values.Min(x => x);
        max = median * 1.5 < max ? median * 1.5 : max;
        return ((decimal)min, (decimal)max);
    }
    
    private double CalculateMedian(List<double> values)
    {
        int count = values.Count;
        if (count == 0) 
            return 0;

        values.Sort();
        int mid = count / 2;
        double median = (count % 2 != 0) ? values[mid] : (values[mid - 1] + values[mid]) / 2.0;
        return median;
    }
}