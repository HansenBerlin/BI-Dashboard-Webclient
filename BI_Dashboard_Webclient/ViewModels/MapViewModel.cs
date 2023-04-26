namespace BI_Dashboard_Webclient.ViewModels;

public class MapViewModel
{
    public List<string> Colors { get; } = new()
    {
        "Bluered", "Blues", "Cividis", "Earth", 
        "Electric", "Greens", "Greys", "Hot", "Jet", "Picnic",
        "Portland", "Rainbow", "RdBu", "Reds", "Viridis", "YlGnBu", "YlOrRd"
    };
}