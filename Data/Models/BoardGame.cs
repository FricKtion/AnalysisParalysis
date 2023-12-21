using System.Xml.Serialization;
using Microsoft.AspNetCore.Http.Features;

namespace AnalysisParalysis.Data.Models;

// TODO - Not all of these need to be public. Try to clean this up such that deserialization still works but only the necessary properties are public
// TODO - Make these the proper data types instead of all strings.

[XmlRoot("items")]
public class BoardGame
{
    public string Id { get => InternalItem.Id; }
    
    public string Name 
    { 
        get =>  InternalItem.InternalName?.Single(x => x.Type == "primary")?.Value ?? ""; 
    }

    public string YearPublished { get => InternalItem.InternalYearPublished.Value; }

    public int TimesPlayed { get; } = 0;

    public Uri? Thumbnail 
    { 
        get => string.IsNullOrEmpty(InternalItem.InternalThumnail) 
            ? null : new Uri(InternalItem.InternalThumnail); 
    }

    [XmlElement("item")]
    public Item InternalItem { get; set; } = new Item();
}

public class Item
{
    [XmlAttribute("type")]
    public string Type { get; } = "";

    [XmlAttribute("id")]
    public string Id { get; }

    [XmlElement("thumbnail")]
    public string InternalThumnail { get; set; } = "";

    [XmlElement("name")]
    public Name[]? InternalName { get; set; }

    [XmlElement("yearpublished")]
    public YearPublished InternalYearPublished { get; set ;} = new YearPublished();
}

public class YearPublished
{
    [XmlAttribute("value")]
    public string Value { get; }
}

public class Name 
{
    [XmlAttribute("type")]
    public string Type { get; set; }

    [XmlAttribute("sortindex")]
    public string SortIndex { get; set; }

    [XmlAttribute("value")]
    public string Value { get; set; }
}
