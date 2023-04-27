namespace BI_Core.Services;

public class RealEstateBookmark
{
    private readonly List<BuyScoresModel> _selectedHouses = new();

    public BuyScoresModel Get(int id)
    {
        var model = _selectedHouses.FirstOrDefault(x => x.Id == id) ?? new BuyScoresModel();
        return model;
    }

    public List<int> GetallIds()
    {
        var ids = _selectedHouses.Select(x => x.Id).ToList();
        return ids;
    }

    public bool IsAdded(BuyScoresModel house)
    {
        var foundIds = _selectedHouses
            .Where(x => x.Id == house.Id).ToList();
        if (foundIds.Count == 0)
        {
            _selectedHouses.Add(house);
            return true;
        }

        return false;
    }
}