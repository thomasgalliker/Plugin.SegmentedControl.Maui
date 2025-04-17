using SegmentedControlTest.ViewModels;

namespace SegmentedControlTest.Views;

public partial class Modal1Page : ContentPage
{
	private readonly Modal1ViewModel _viewModel;
	public Modal1Page()
	{
		InitializeComponent();
		BindingContext = _viewModel = new Modal1ViewModel();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.OnPropertyChanged(nameof(_viewModel.IsImageVisible));
	}

	private async void OnCloseModalClicked(object sender, EventArgs e)
	{		
		await Navigation.PopModalAsync();
	}

	private async void OnOpenModalClicked(object sender, EventArgs e)
	{		
		await Navigation.PushModalAsync(new NavigationPage(new Modal2Page()));
	}
}

