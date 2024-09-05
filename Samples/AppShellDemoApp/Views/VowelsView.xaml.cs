using SegmentedControlReproduce.ViewModels;

namespace SegmentedControlReproduce.Views;

public partial class VowelsView : ContentPage
{
    public VowelsView(VowelsViewModel vvw)
    {
        this.InitializeComponent();
        this.BindingContext = vvw;
    }
}