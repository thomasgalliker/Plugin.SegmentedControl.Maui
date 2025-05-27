
using SegmentedControlTest.Views;
namespace SegmentedControlTest;

public partial class Tab1Page : ContentPage
{
	int count = 0;

	public Tab1Page()
	{
		InitializeComponent();
	}

	private async void OnOpenModalClicked(object sender, EventArgs e)
	{
		count++;

		await Navigation.PushModalAsync(new NavigationPage(new Modal1Page()));
	}
}

