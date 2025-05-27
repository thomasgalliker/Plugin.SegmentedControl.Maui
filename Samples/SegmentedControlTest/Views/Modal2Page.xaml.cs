namespace SegmentedControlTest.Views;

public partial class Modal2Page : ContentPage
{

	public Modal2Page()
	{
		InitializeComponent();
	}

	private async void OnCloseModalClicked(object sender, EventArgs e)
	{		
		await Navigation.PopModalAsync();
	}
}

