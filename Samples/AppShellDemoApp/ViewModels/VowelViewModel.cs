using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SegmentedControlReproduce.Views;

namespace SegmentedControlReproduce.ViewModels
{
    public partial class VowelsViewModel : ObservableObject
    {
        public class VowelViewOption
        {
            public string Name { get; set; }
            public IContentView View { get; set; }
        }

        public class SoundTypeOption
        {
            public string Latin { get; set; }
            public string Thai { get; set; }
        }

        public class VowelVisualizationOption
        {
            public string Latin { get; set; }
            public string Thai { get; set; }
        }

        [ObservableProperty]
        ObservableCollection<SoundTypeOption> soundTypeOptions = [
            new SoundTypeOption { Latin = "all", Thai = "เสียง-ทั้งหมด" },
            new SoundTypeOption { Latin = "short", Thai = "สระ-เสียง-สั้น" },
            new SoundTypeOption { Latin = "long", Thai = "สระ-เสียง-ยาว" }
        ];

        [ObservableProperty]
        List<VowelVisualizationOption> vowelVisualizationOptions = [
            new VowelVisualizationOption() { Latin = "Sound", Thai = "สระ-เสียง" },
            new VowelVisualizationOption() { Latin = "Open", Thai = "คำ-เป็น" },
            new VowelVisualizationOption() { Latin = "Closed", Thai = "คำ-ตาย" }
        ];

        [ObservableProperty]
        List<VowelViewOption> vowelViewOptions = [
            new VowelViewOption { Name = "List", View = new AllVowels() },
            new VowelViewOption { Name = "List by sound", View = new GraphemesBySound() }
        ];

        [ObservableProperty]
        SoundTypeOption selectedSoundTypeOption;

        [ObservableProperty]
        VowelViewOption selectedVowelViewOption;

        [ObservableProperty]
        VowelVisualizationOption selectedVowelVisualizationOption;

        public VowelsViewModel()
        {
            this.SelectedVowelViewOption = this.VowelViewOptions[0];
            this.SelectedSoundTypeOption = this.SoundTypeOptions[0];
            this.SelectedVowelVisualizationOption = this.VowelVisualizationOptions[0];
        }
    }
}
