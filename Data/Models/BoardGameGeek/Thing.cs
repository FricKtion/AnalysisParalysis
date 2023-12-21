using System.Xml.Serialization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;

namespace AnalysisParalysis.Data.Models.BoardGameGeek;

// TODO - Some of these properties aren't being properly deserialized. Ignoring for now because I don't need them.

[XmlRoot("items")]
public class Thing
{
    [XmlElement("item")]
    public Item Item { get; set; } = new Item();
}

public class Item
{
    [XmlAttribute("type")]
    public string Type { get; } = "";

    [XmlAttribute("id")]
    public int Id { get; }

    [XmlElement("thumbnail")]
    public string Thumbnail { get; set; } = "";

    [XmlElement("image")]
    public string Image { get; set; } = "";

    [XmlElement("name")]
    public NameElement[]? Names { get; set; }

    [XmlElement("description")]
    public string Description { get; set; } = "";

    [XmlElement("yearpublished")]
    public ValueOnlyElement YearPublished { get; set; } = new ValueOnlyElement();

    [XmlElement("minplayers")]
    public ValueOnlyElement MinPlayers { get; set; } = new ValueOnlyElement();

    [XmlElement("maxplayers")]
    public ValueOnlyElement MaxPlayers { get; set; } = new ValueOnlyElement();

    // [XmlElement("poll")]
    // public PollElement[] Polls { get; set; }

    // [XmlElement("link")]
    // public Link[] Links { get; set; }

    [XmlType("name")]
    public class NameElement
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "";

        [XmlAttribute("sortindex")]
        public int SortIndex { get; set; } = -1;

        [XmlAttribute("value")]
        public string Value { get; set; } = "";
    }

    [XmlType]
    public class ValueOnlyElement
    {
        [XmlAttribute("value")]
        public string Value { get; } = "";
    }

    public class PollElement
    {
        [XmlAttribute("name")]
        public string Name { get; set; } = "";

        [XmlAttribute("title")]
        public string Title { get; set; } = "";

        [XmlAttribute("totalVotes")]
        public int TotalVotes { get; set; }

        [XmlElement("results")]
        public PollResults[] Results { get; set; }
    }

    public class PollResults
    {
        [XmlAttribute("result")]
        public PollResult[] Result { get; set; }
    }

    public class PollResult
    {
        [XmlAttribute("value")]
        public string Value { get; set; } = "";

        [XmlAttribute("numvotes")]
        public int NumVotes { get; set; }
    }

    public class Link
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "";

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}
