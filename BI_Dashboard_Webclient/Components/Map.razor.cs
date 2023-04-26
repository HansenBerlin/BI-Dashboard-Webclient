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
using Title = Plotly.Blazor.LayoutLib.Title;
using XAnchorEnum = Plotly.Blazor.LayoutLib.LegendLib.XAnchorEnum;
using YAnchorEnum = Plotly.Blazor.LayoutLib.LegendLib.YAnchorEnum;

namespace BI_Dashboard_Webclient.Components;

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
    private PlotlyChart _plotlyChart = new PlotlyChart();
    private bool _isInitDone;

    private ScatterMapBox _scatterMap;
    private ChoroplethMapBox _choromap;
    //private List<GenericRegionsModel> _regionsDataModels;
    


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            string url = "https://localhost:6001/aggregates";
            AggregatesRepository.Init(url);
            url = "https://localhost:6001/raw";
            ImmoDataRepository.Init(url);
            
            var geoJsonData = await StatefulRepository.Get();

            _plotlyChart.Config = new Config
            {
                MapboxAccessToken = Token, 
                //Responsive = true
            };
            
            _choromap =  new ChoroplethMapBox
            {
                GeoJson = geoJsonData,
                FeatureIdKey = "properties.AGS",
                ColorAxis = "coloraxis"
            };

            _scatterMap = new ScatterMapBox
            {
                Mode = ModeFlag.Markers, 
                Marker = new Marker
                {
                    ColorAxis = "coloraxis2", 
                }};

            
            _plotlyChart.Layout = new Layout
            {
                Legend = new Legend
                {
                    YAnchor = YAnchorEnum.Top,
                    XAnchor = XAnchorEnum.Left,
                    Y = 0.99m,
                    X = 0.99m
                },
                MapBox = new List<MapBox>
                {
                    new ()
                    {
                        Style = "carto-positron",
                        Center = new Center
                        {
                            Lon = 10.4515m, 
                            Lat = 51.1657m
                        },
                        Zoom = 5.5m
                    }
                }, 
                ColorAxis = new List<ColorAxis>
                {
                    new()
                    {
                        ColorScale = "Viridis",
                        ShowScale = true, 
                        ColorBar = new ColorBar
                        {
                            YAnchor = 0,
                            Y = 1, 
                            X = 0.05m, 
                            Title = new Plotly.Blazor.LayoutLib.ColorAxisLib.ColorBarLib.Title
                            {
                                Text = _selectedBaseValue
                            },
                            TickLabelPosition = TickLabelPositionEnum.OutsideLeft,
                            BgColor = "#ffffff88", 
                            BorderColor = "#99999999"

                        }
                    },
                    new()
                    {
                        ColorScale = "Cividis",
                        ShowScale = true,
                        ColorBar = new ColorBar
                        {
                            YAnchor = 0,
                            Y = 1, 
                            X = 0.9m,
                            Title = new Plotly.Blazor.LayoutLib.ColorAxisLib.ColorBarLib.Title
                            {
                                Text = _selectedOverlayValue
                            }, 
                            TickLabelPosition = TickLabelPositionEnum.OutsideRight, 
                            BgColor = "#ffffff88",
                            BorderColor = "#99999999"
                        }
                    }
                }, 
                //Height = 1000, 
                Margin = new Margin()
                {
                    B = 0, 
                    L = 0, 
                    R = 0, 
                    T = 0, 
                    Pad = 0, 
                    AutoExpand = true
                }
            };

            try
            {
                await _plotlyChart.Relayout();
            }
            catch (Exception) { }
            
            _plotlyChart.Config = new Config
            {
                MapboxAccessToken = Token,
                Responsive = true, 
                DisplayLogo = false, 
                DisplayModeBar = DisplayModeBarEnum.False, 
                Editable = false
            };
            
            await _plotlyChart.AddTrace(_choromap);
            await _plotlyChart.AddTrace(_scatterMap);
            
            //await BaseValueChanged("consumerInsolvencies");
            //await OverlayColorValueChanged("baseRent");
            //await _plotlyChart.NewPlot();
            await _plotlyChart.Update();
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

        await InvokeAsync(StateHasChanged);
    }

    

    private string _selectedOverlayValue = "";
    //private string _selectedOverlaySize = "";
    private string _selectedBaseValue = "";

    private async Task OverlaySizeValueChanged(SelectionDataSetOptions selected)
    {
        _selectedOverlayValue = selected.Column;
        _isInitDone = false;
        string appendToUrl = $"{selected.DataSet.ToString()}/{selected.Column}";
        var scatterOverlayDataSize = await ImmoDataRepository.GetAll(appendToUrl);
        _scatterMap.Marker.SizeArray = scatterOverlayDataSize
            .OrderBy(x => x.Id)
            .Select(x => decimal
                .Parse(x.GenericProperty))
            .ToList() as IList<decimal?>;
        AddLatLonOverlay(scatterOverlayDataSize);
        //await _plotlyChart.NewPlot();
        await _plotlyChart.Update();
        _isInitDone = true;
        await InvokeAsync(StateHasChanged);

    }

    private async Task OverlayColorValueChanged(SelectionDataSetOptions selected)
    {
        _isInitDone = false;
        string appendToUrl = $"{selected.DataSet.ToString()}/{selected.Column}";
        var scatterOverlayDataColor = await ImmoDataRepository.GetAll(appendToUrl);
        
        if (scatterOverlayDataColor[0].IsResponseSuccess == false)
        {
            _isInitDone = true;
            return;
        }
        _scatterMap.Marker.ColorArray = scatterOverlayDataColor
            .Select(x => (object) x.GenericProperty)
            .ToList();
        
        (double min, double max) = CalculateMinMax(scatterOverlayDataColor
            .Select(x => double.Parse(x.GenericProperty))
            .ToList());

        _scatterMap.Marker.CMax = (decimal)max;
        _scatterMap.Marker.CMin = (decimal)min;
        AddLatLonOverlay(scatterOverlayDataColor);
        _plotlyChart.Layout.ColorAxis[1].ColorBar.Title.Text = _selectedOverlayValue;

        await _plotlyChart.Update();
        //await _plotlyChart.NewPlot();
        _isInitDone = true;
        await InvokeAsync(StateHasChanged);

    }

    private void AddLatLonOverlay(List<GenericRawDataModel> values)
    {
        _scatterMap.Lat = values
            .OrderBy(x => x.Id)
            .Select(x => (object) x.Lat)
            .ToList();
        _scatterMap.Lon = values
            .OrderBy(x => x.Id)
            .Select(x => (object) x.Lon)
            .ToList();
    }
    
    private async Task BaseValueChanged(SelectionDataSetOptions selected)
    {
        _isInitDone = false;
        _selectedBaseValue = selected.Column;
        string appendToUrl = $"{selected.DataSet.ToString()}/{selected.Column}";
        var choroplethOverlayData = await AggregatesRepository.GetAll(appendToUrl);
        if (choroplethOverlayData[0].IsResponseSuccess == false)
        {
            _isInitDone = true;
            return;
        }
        
        (double min, double max) = CalculateMinMax(choroplethOverlayData
            .Select(x => double.Parse(x.GenericProperty))
            .ToList());
        _choromap.ZMin = (decimal)min;
        _choromap.ZMax = (decimal)max;

        _choromap.Z = choroplethOverlayData
            .Select(x => (object)x.GenericProperty)
            .ToList();
        _choromap.Locations = choroplethOverlayData
            .Select(x => (object)x.AgsKey)
            .ToList();

        _plotlyChart.Layout.ColorAxis[0].ColorBar.Title.Text = _selectedBaseValue;

        await _plotlyChart.Update();
        //await _plotlyChart.NewPlot();
        _isInitDone = true;
        await InvokeAsync(StateHasChanged);

    }

    private async Task ColorChanged(string toColor, int index)
    {
        _plotlyChart.Layout.ColorAxis[index].ColorScale = toColor;
        await _plotlyChart.Update();
        await InvokeAsync(StateHasChanged);
    }

    private (double min, double max) CalculateMinMax(List<double> values)
    {
        double median = CalculateMedian(values);
        var max = values.Max(x => x);
        var min = values.Min(x => x);
        max = median * 1.5 < max ? median * 1.5 : max;
        return (min, max);
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