namespace AnalysisParalysis.Data.Models;

public class BoardGame
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public int? YearPublished { get; set; }

    public int TimesPlayed { get; set; } = 0;

    public Uri? Thumbnail { get; set; }
}
