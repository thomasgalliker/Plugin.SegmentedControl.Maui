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

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // await Task.Delay(4000);
                // this.Countries.First().EnglishName = "Updated Country";

                await Task.Delay(4000);
                this.Countries =
                [
                    new CountryItemViewModel("New Country 1", "New Country 1", "CH"),
                    new CountryItemViewModel("New Country 2", "New Country 2", "CH"),
                ];
            });
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
