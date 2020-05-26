using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using static Bogus.DataSets.Name;

namespace pocwardlist
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private WardListViewModel vm;
        public int Span { get; set; }

        public MainPage()
        {
            vm = new WardListViewModel(Navigation);
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
            Span = Device.Idiom == TargetIdiom.Phone ? 1 : 2;
            BindingContext = vm;
            InitializeComponent();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.OnAppearing();
        }

        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            await vm.ShowSettings();
        }


        private void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            Span = (Device.Idiom == TargetIdiom.Tablet ||
                   e.DisplayInfo.Orientation == DisplayOrientation.Landscape)
                   ? 2 : 1;
            
        }
    }

    public class WardListViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public ObservableCollection<ColumnGroup> ColumnGroups { get; private set; }
        public ObservableCollection<Patient> Patients { get; set; }
        
        public WardListViewModel(INavigation navigation)
        {

            this.Navigation = navigation;
            ColumnGroups = PatientListService.CreateColumns();
            
        }


        public async Task ShowSettings()
        {
            await Navigation.PushModalAsync(new NavigationPage(new SettingsPage(ColumnGroups)));
        }

        internal void OnAppearing()
        {
            Patients = PatientListService.GetPatients();
            foreach (var p in Patients)
            {
                p.TopLeftGroup = new ObservableCollection<DynamicValue>();
                p.TopRightGroup = new ObservableCollection<DynamicValue>();
                p.LeftGroup = new ObservableCollection<DynamicValue>();
                p.RightGroup = new ObservableCollection<DynamicValue>();

                foreach (var c in ColumnGroups.FirstOrDefault(c => c.Title == "TopLeft").ToList())
                {
                    p.TopLeftGroup.Add(new DynamicValue
                    {
                        Header = c.Header,
                        Value = c.Value
                    });
                }
                foreach (var c in ColumnGroups.FirstOrDefault(c => c.Title == "TopRight").ToList())
                {
                    p.TopRightGroup.Add(new DynamicValue
                    {
                        Header = c.Header,
                        Value = c.Value
                    });
                }
                foreach (var c in ColumnGroups.FirstOrDefault(c => c.Title == "Left").ToList())
                {
                    p.LeftGroup.Add(new DynamicValue
                    {
                        Header = c.Header,
                        Value = c.Value
                    });
                }
                foreach (var c in ColumnGroups.FirstOrDefault(c => c.Title == "Right").ToList())
                {
                    p.RightGroup.Add(new DynamicValue
                    {
                        Header = c.Header,
                        Value = c.Value
                    });
                }

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }

    public class Patient : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string SSN { get; set; }

        public ObservableCollection<DynamicValue> LeftGroup { get; set; }
        public ObservableCollection<DynamicValue> TopLeftGroup { get; set; } 
        public ObservableCollection<DynamicValue> TopRightGroup { get; set; } 
        public ObservableCollection<DynamicValue> RightGroup { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class DynamicValue
    {
        public string Header { get; set; }
        public string Value { get; set; }
    }
}
