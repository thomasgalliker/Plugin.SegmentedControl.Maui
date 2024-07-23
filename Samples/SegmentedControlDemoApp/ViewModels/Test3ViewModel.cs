using CommunityToolkit.Mvvm.ComponentModel;

namespace SegmentedControlDemoApp.ViewModels
{
    public class Test3ViewModel : ObservableObject
    {
        private CountryItemViewModel selectedItem;
        private CountryItemViewModel[] countries;

        public Test3ViewModel()
        {
            this.Countries =
            [
               new CountryItemViewModel("Switzerland", "Swiss Confederation", "CH"),
               new CountryItemViewModel("Sweden", "Kingdom of Sweden", "SE"),
               new CountryItemViewModel("United States of America", "United States of America", "US"),
            ];

            this.SelectedCountry = this.Countries.First();
        }

        public CountryItemViewModel[] Countries
        {
            get => this.countries;
            private set => this.SetProperty(ref this.countries, value);
        }

        public CountryItemViewModel SelectedCountry
        {
            get => this.selectedItem;
            set => this.SetProperty(ref this.selectedItem, value);
        }
    }
}
