using System.Xml.Serialization;

namespace AnalysisParalysis.Data.Models.BoardGameGeek;

[XmlRoot("items")]
public class Collection
{
    [XmlAttribute("totalitems")]
    public int Count { get; set; } = 0;

    [XmlElement("item")]
    public Item[] Items { get; set; }

    public class Item
    {
        [XmlAttribute("objecttype")]
        public string Type { get; set; } = "";

        [XmlAttribute("objectid")]
        public int Id { get; set; }

        [XmlAttribute("subtype")]
        public string SubType { get; set; } = "";

        [XmlElement("name")]
        public string Name { get; set; } = "";

        [XmlElement("yearpublished")]
        public int YearPublished { get; set; }

        [XmlElement("image")]
        public string Image { get; set; } = "";

        [XmlElement("thumbnail")]
        public string Thumbnail { get; set; } = "";

        [XmlElement("status")]
        public Status Status { get; set; } = new Status();

        [XmlElement("numplays")]
        public int TimesPlayed { get; set; }
    }

    public class Status 
    {
        [XmlAttribute("own")]
        public bool Own { get; set;}

        [XmlAttribute("fortrade")]
        public bool ForTrade { get; set; }

        [XmlAttribute("want")]
        public bool Want { get; set; }

        [XmlAttribute("wanttoplay")]
        public bool WantToPlay { get; set; }

        [XmlAttribute("wanttobuy")]
        public bool WantToBuy { get; set; }

        [XmlAttribute("wishlist")]
        public bool Wishlist { get; set; }

        [XmlAttribute("preordered")]
        public bool Preordered { get; set; }
    }
}
