using System.IO;
using System.Text.Json;
using System.Xml.Serialization;


namespace WpfGuiDataSaver
{
    [Serializable]
    public class GuiSaveDataItem
    {
        public GuiSaveDataItem()
        {
            Text = null;
            Id = string.Empty;
        }

        public GuiSaveDataItem( string id )
        {
            Id = id;
        }

        public string Id { get; set; }

        public string? Text { get; set; }
        public bool ShouldSerializeText() => Text is not null;

        public bool? IsChecked { get; set; }
        public bool ShouldSerializeIsChecked() => IsChecked.HasValue;

        public double? Value { get; set; }
        public bool ShouldSerializeValue() => Value.HasValue;

        public int? SelectedIndex { get; set; }
        public bool ShouldSerializeSelectedIndex() => SelectedIndex.HasValue;
    }


    [Serializable]
    public class GuiSavedDataContainer
    {
        public List<GuiSaveDataItem> Data { get; set; }
        public bool UseIds { get; set; }

        public GuiSavedDataContainer()
        {
            Data = new();
        }

        public string SerializeXml()
        {
            StringWriter sw = new();
            XmlSerializer serializer = new XmlSerializer( typeof( GuiSavedDataContainer ) );

            serializer.Serialize( sw, this );

            return sw.ToString();
        }

        public string SerializeJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };

            return JsonSerializer.Serialize( this, options );
        }

        public static GuiSavedDataContainer? DeserializeXml( string text )
        {
            StringReader sr = new( text );
            XmlSerializer serializer = new XmlSerializer( typeof( GuiSavedDataContainer ) );

            return (GuiSavedDataContainer?)serializer.Deserialize( sr );
        }

        public static GuiSavedDataContainer? DeserializeJson( string text )
        {
            return JsonSerializer.Deserialize<GuiSavedDataContainer>( text );
        }
    }
}

