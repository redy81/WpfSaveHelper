using System.IO;
using System.Windows;
using WpfGuiDataSaver;

namespace WpfGuiSaverTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GuiSavedDataContainer? saved;
        public MainWindow()
        {
            InitializeComponent();

            saved = null;
        }

        private void Button_Click( object sender, RoutedEventArgs e )
        {
            saved = GuiSaveHelper.SaveData( this, "", GuiCheckIdType.Name );

            // File.WriteAllText( @"C:\temp\textSave.txt", saved.SerializeXml() );      // Test serialization to file
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            if ( saved is not null )
            {
                GuiSaveHelper.RestoreData( this, saved );
            }

            //GuiSaveHelper.RestoreData( this, GuiSavedDataContainer.DeserializeXml( File.ReadAllText( @"C:\temp\textSave.txt" )! )! );     // Test deserialization to file
        }

        private void Button_Click_2( object sender, RoutedEventArgs e )
        {
            GuiSaveHelper.ClearData( this, "", GuiCheckIdType.Name );
        }
    }
}