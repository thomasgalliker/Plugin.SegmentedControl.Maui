using CommunityToolkit.Mvvm.ComponentModel;

namespace SegmentedControlDemoApp.ViewModels
{
    public class CountryItemViewModel : ObservableObject
    {
        public CountryItemViewModel(string englishName, string officialName, string iso3166CountryCode)
        {
            this.EnglishName = englishName;
            this.OfficialName = officialName;
            this.Iso3166CountryCode = iso3166CountryCode;
        }

        public string EnglishName { get; }

        public string OfficialName { get; }

        public string Iso3166CountryCode { get; }
    }
}
