namespace AnalysisParalysis.Data.Models;

public class BoardGame
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public int? YearPublished { get; set; }

    public int TimesPlayed { get; set; } = 0;

    public int MinimumPlayers { get; set; } = 0;

    public int MaximumPlayers { get; set; } = 0;

    public Uri? Thumbnail { get; set; }

    public bool IsSelected { get; set; } = false;

    public bool IsHidden { get; set; } = false;
}
