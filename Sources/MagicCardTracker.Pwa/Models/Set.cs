namespace MagicCardTracker.Pwa.Models;

internal class Set
{
    public Set(string code, string name)
    {
        Code = code;
        Name = name;
    }
    
    public string Code { get; set; }

    public string Name { get; set; }

    public int NumberOfCards { get; set; }

    public int NumberOfCollectedCards { get; set; }

    public DateTime ReleaseDate { get; set; }

    public string? SetIconUrl { get; set; }

    public bool IsCoreOrExpansionSet { get; set; }

    public int Completeness => NumberOfCards > 0
        ? NumberOfCollectedCards * 100 / NumberOfCards 
        : 0;
}
