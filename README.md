# WpfSaveHelper
### Easy way to save settings from a WPF window

The target of this project is to have an easy way to save all the user-editable data from a WPF, without introducing a ton of extra code.

The project contains two main classes:

*   GuiSavedDataContainer - a class where the relevant data is saved and can be serialized/deserialized to XML or JSON
    
*   GuiSaveHelper - the class that contains the methods to save/restore the data
    

## Usage

There are three methods implemented in the static class.
```cs
    public static GuiSavedDataContainer SaveData( DependencyObject rootObj, string prefix, GuiCheckIdType scanType = GuiCheckIdType.Name )
```

This function retrieves the information from the controls and returns a `GuiSavedDataContainer` object that contains all the data.

The scan starts at `rootObj`, which can be any `DependecyObject`. The scan work recursively on all the children of the root object.

The parameter `scanType` selects which object property is used to filter the controls: `Name` or `Uid`.

The parameter `prefix` allows to enter a prefix which the controls must have in the property in order to be saved. The comparison is *case insensitive*. If `prefix` is null or an empty string, all the controls that have a non-empty `Name`/`Uid` are saved.

<br>

```cs
    public static void RestoreData( DependencyObject rootObj, GuiSavedDataContainer saveData )
```
This method restores the data from a `GuiSavedDataContainer` object.

The parameters are the GUI root `rootObj` and the saved data `saveData`.

<br>

```cs
    public static void ClearData( DependencyObject rootObj, string prefix, GuiCheckIdType scanType = GuiCheckIdType.Name )
```

This method was mostly implement for testing. It uses the same logic as the function `SaveData` to select the controls and it will clear the controls content. You can refer to the `SaveData` description of the function's parameters.

#### Configuration
A static field is included in the `GuiSaveHelper` class, which can be used to configure whether the PasswordBox is included or not in the save data:

```cs
    public static bool SkipPasswordFields {  get; set; }
```
    
If set to true, the `PasswordBox.Password` will not be included when using the function `SaveData`. By default, `SkipPasswordFields` is set to `false`.

## Included controls
The following controls and properties are included in the save data:
* TextBox - Text
* ComboBox - SelectedIndex, Text
* ListBox - SelectedIndex
* Slider - Value
* CheckBox - IsChecked (supports tristate value)
* ToggleButton - IsChecked
* RadioButton - IsChecked
* PasswordBox - Password (conditionally)


## GuiSavedDataContainer
This class is just a container to store the information retrieved from the GUI.
It implements two functions to serialize it and two static function to deserialized a text. It can serialized/deserialize to XML and JSON.
```cs
        public string SerializeXml()
        public string SerializeJson()
        public static GuiSavedDataContainer? DeserializeXml( string text )
        public static GuiSavedDataContainer? DeserializeJson( string text )
```
The functions are self explanatory.

## Example
Considering the following XAML:
```xml
        <!-- TextBox -->
        <TextBox Grid.Row="2" Margin="10" Height="25" Text="stuff" x:Name="mybox" Uid="ubox"/>

        <!-- CheckBox -->
        <CheckBox Grid.Row="3" Margin="10" Content="Check this" x:Name="saveCheck"/>

        <!-- Slider -->
        <Slider Grid.Row="4" Margin="10" Minimum="0" Maximum="100" Value="50" x:Name="saveSlider" Uid="mySlider"/>
```

| prefix | scanType | Included Controls Name (Uid) |
|--|--|--|
| "" | `GuiCheckIdType.Name` | mybox, saveCheck, saveSlider |
| "save" | `GuiCheckIdType.Name` | saveCheck, saveSlider |
| "my" | `GuiCheckIdType.Name` | mybox |
| "" | `GuiCheckIdType.Uid` | mybox (ubox), saveSlider (mySlider) |
| "u" | `GuiCheckIdType.Uid` | mybox (ubox) |



### Coffee found

If you find this useful, or it helped you to save time, you can buy me a coffee.

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/donate/?hosted_button_id=VNMV7XY9J5HBG)

**Bitcoin**: bc1q4379vq6jfg8swgqajyaetm02fyk2mwj0mwy8wj<br>
**Bitcoin**: 1DbK9AoXxMRYYENwsoDEzqyxppo1cMUQUN
