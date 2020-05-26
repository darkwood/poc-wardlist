using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace pocwardlist
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsViewModel vm { get; set; }
        public SettingsPage(ObservableCollection<ColumnGroup> columns)
        {
            vm = new SettingsViewModel(columns);
            BindingContext = vm;
            InitializeComponent();
        }


        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }

    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ColumnGroup> ColumnGroups { get; set; }
        
        public SettingsViewModel(ObservableCollection<ColumnGroup> groups)
        {
            ColumnGroups = groups;
        }
    }

    public class ColumnGroup : List<Column>
    {
        public string Title { get; set; }
        public string ShortName { get; set; } //will be used for jump lists
        public string Subtitle { get; set; }
        public ColumnGroup(string title, string shortName)
        {
            Title = title;
            ShortName = shortName;
        }

    }
    public class Column : INotifyPropertyChanged
    {
        public string Header { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public bool Show { get; set; }
        public Color StatusColor { get; set; }
        public Command Tapped { get; set; }

        public Column()
        {
            Tapped = new Command(() => {
                Show = !Show;
                StatusColor = Show ? Color.LightGreen : Color.White;

            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
