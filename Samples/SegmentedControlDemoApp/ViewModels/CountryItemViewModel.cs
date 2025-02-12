using CommunityToolkit.Mvvm.ComponentModel;

namespace SegmentedControlDemoApp.ViewModels
{
    public class CountryItemViewModel : ObservableObject
    {
        private string englishName;

        public CountryItemViewModel(string englishName, string officialName, string iso3166CountryCode)
        {
            this.EnglishName = englishName;
            this.OfficialName = officialName;
            this.Iso3166CountryCode = iso3166CountryCode;
        }

        public string EnglishName
        {
            get => this.englishName;
            set => this.SetProperty(ref this.englishName, value);
        }

        public string OfficialName { get; }

        public string Iso3166CountryCode { get; }
    }
}
