using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfGuiDataSaver
{
    public enum GuiCheckIdType
    {
        Name = 0,
        Uid
    }

    public static class GuiSaveHelper
    {
        public static bool SkipPasswordFields { get; set; } = false;

        private static List<T> FindChildren<T>( DependencyObject depObj ) where T : DependencyObject
        {
            if ( depObj == null )
                return new List<T>();

            var output = new List<T>();

            var childs = LogicalTreeHelper.GetChildren( depObj );

            foreach ( var child in childs )
            {
                if ( child is T )
                {
                    output.Add( (T)child );
                }

                if ( child is DependencyObject )
                {
                    output.AddRange( FindChildren<T>( (DependencyObject)child ) );
                }
            }

            return output;
        }

        public static GuiSavedDataContainer SaveData( DependencyObject rootObj, string prefix, GuiCheckIdType scanType = GuiCheckIdType.Name )
        {
            var saveData = new GuiSavedDataContainer();
            bool skipCheck = string.IsNullOrEmpty( prefix );

            var guiObjects = FindChildren<FrameworkElement>( rootObj );

            saveData.UseIds = scanType == GuiCheckIdType.Uid;

            foreach ( var obj in guiObjects )
            {

                string checkValue;

                if ( scanType == GuiCheckIdType.Name )
                {
                    checkValue = obj.Name;
                }
                else
                {
                    checkValue = obj.Uid;
                }

                bool process = false;

                if ( skipCheck )
                {
                    if ( !string.IsNullOrWhiteSpace( checkValue ) )
                    {
                        process = true;
                    }
                }
                else
                {
                    if ( checkValue.StartsWith( prefix, StringComparison.OrdinalIgnoreCase ) )
                    {
                        process = true;
                    }
                }

                if ( process )
                {
                    switch ( obj )
                    {
                        case TextBox tb:
                            saveData.Data.Add( new GuiSaveDataItem( checkValue ) );
                            saveData.Data.Last().Text = tb.Text;
                            break;

                        case ListBox lb:
                            saveData.Data.Add( new GuiSaveDataItem( checkValue ) );
                            saveData.Data.Last().SelectedIndex = lb.SelectedIndex;
                            break;

                        case ComboBox cb:
                            saveData.Data.Add( new GuiSaveDataItem( checkValue ) );
                            saveData.Data.Last().Text = cb.Text;
                            saveData.Data.Last().SelectedIndex = cb.SelectedIndex;
                            break;

                        case RadioButton:
                        case CheckBox:
                        case ToggleButton:
                            saveData.Data.Add( new GuiSaveDataItem( checkValue ) );

                            object? value = obj?.GetType()?.GetProperty( "IsChecked" )?.GetValue( obj );

                            if ( value != null )
                            {
                                saveData.Data.Last().IsChecked = (bool)value;
                            }
                            break;

                        case PasswordBox pb:
                            if ( !SkipPasswordFields )
                            {
                                saveData.Data.Add( new GuiSaveDataItem( checkValue ) );
                                saveData.Data.Last().Text = pb.Password;
                            }
                            break;

                        case Slider sl:
                            saveData.Data.Add( new GuiSaveDataItem( checkValue ) );
                            saveData.Data.Last().Value = sl.Value;
                            break;
                    }
                }

            }

            return saveData;
        }

        public static void RestoreData( DependencyObject rootObj, GuiSavedDataContainer saveData )
        {
            var Objects = FindChildren<FrameworkElement>( rootObj );

            foreach ( var obj in Objects )
            {
                string checkValue;

                if ( saveData.UseIds )
                {
                    checkValue = obj.Uid;
                }
                else
                {
                    checkValue = obj.Name;
                }

                if ( saveData.Data.Any( x => string.Equals( x.Id, checkValue, StringComparison.OrdinalIgnoreCase ) ) )
                {
                    switch ( obj )
                    {
                        case TextBox tb:
                            tb.Text = saveData.Data.First( x => string.Compare( x.Id, checkValue, true ) == 0 ).Text;
                            break;

                        case ListBox lb:
                            lb.SelectedIndex = saveData.Data.First( x => string.Compare( x.Id, checkValue, true ) == 0 ).SelectedIndex!.Value;
                            break;

                        case ComboBox cb:
                            cb.SelectedIndex = saveData.Data.First( x => string.Compare( x.Id, checkValue, true ) == 0 ).SelectedIndex!.Value;
                            cb.Text = saveData.Data.First( x => string.Compare( x.Id, checkValue, true ) == 0 ).Text;
                            break;

                        case CheckBox:
                        case RadioButton:
                        case ToggleButton:
                            obj?.GetType()?.GetProperty( "IsChecked" )?.SetValue( obj, saveData.Data.First( x => string.Compare( x.Id, checkValue, true ) == 0 ).IsChecked );
                            break;

                        case PasswordBox pb:
                            pb.Password = saveData.Data.First( x => string.Compare( x.Id, checkValue, true ) == 0 ).Text;
                            break;

                        case Slider sl:
                            double? val = saveData.Data.First( x => string.Compare( x.Id, checkValue, true ) == 0 ).Value;

                            if ( val is not null )
                            {
                                sl.Value = val.Value;
                            }
                            break;
                    }

                }
            }
        }

        public static void ClearData( DependencyObject rootObj, string prefix, GuiCheckIdType scanType = GuiCheckIdType.Name )
        {
            bool skipCheck = string.IsNullOrEmpty( prefix );

            var guiObjects = FindChildren<FrameworkElement>( rootObj );

            foreach ( var obj in guiObjects )
            {

                string checkValue;

                if ( scanType == GuiCheckIdType.Name )
                {
                    checkValue = obj.Name;
                }
                else
                {
                    checkValue = obj.Uid;
                }

                bool process = false;

                if ( skipCheck )
                {
                    if ( !string.IsNullOrWhiteSpace( checkValue ) )
                    {
                        process = true;
                    }
                }
                else
                {
                    if ( checkValue.StartsWith( prefix, StringComparison.OrdinalIgnoreCase ) )
                    {
                        process = true;
                    }
                }

                if ( process )
                {
                    switch ( obj )
                    {
                        case TextBox tb:
                            tb.Text = string.Empty;
                            break;

                        case ListBox lb:
                            lb.SelectedIndex = -1;
                            break;

                        case ComboBox cb:
                            cb.Text = string.Empty;
                            cb.SelectedIndex = -1;
                            break;

                        case RadioButton:
                        case CheckBox:
                        case ToggleButton:
                            obj?.GetType()?.GetProperty( "IsChecked" )?.SetValue( obj, false );
                            break;

                        case PasswordBox pb:
                            if ( !SkipPasswordFields )
                            {
                                pb.Password = string.Empty;
                            }
                            break;

                        case Slider sl:
                            sl.Value = 0.0;
                            break;
                    }
                }

            }
        }
    }
}

