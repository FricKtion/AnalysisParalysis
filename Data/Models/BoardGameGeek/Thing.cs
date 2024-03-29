using System.Xml.Serialization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;

namespace AnalysisParalysis.Data.Models.BoardGameGeek;

[XmlRoot("items")]
public class Thing
{
    [XmlElement("item")]
    public Item[] Items { get; set; } = default!;

    public class Item
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "";

        [XmlAttribute("id")]
        public int Id { get; set; }

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

        [XmlElement("playingtime")]
        public ValueOnlyElement  PlayingTime { get; set; } = new ValueOnlyElement();

        [XmlElement("minplaytime")]
        public ValueOnlyElement MinPlayTime { get; set; } = new ValueOnlyElement();

        [XmlElement("maxplaytime")]
        public ValueOnlyElement MaxPlayTime { get; set; } = new ValueOnlyElement();

        public class NameElement
        {
            [XmlAttribute("type")]
            public string Type { get; set; } = "";

            [XmlAttribute("sortindex")]
            public int SortIndex { get; set; } = -1;

            [XmlAttribute("value")]
            public string Value { get; set; } = "";
        }

        public class ValueOnlyElement
        {
            [XmlAttribute("value")]
            public string Value { get; set; } = "";
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
            public PollResults[] Results { get; set; } = default!;
        }

        public class PollResults
        {
            [XmlAttribute("result")]
            public PollResult[] Result { get; set; } = default!;
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
            public string Value { get; set; } = "";
        }
    }
}
