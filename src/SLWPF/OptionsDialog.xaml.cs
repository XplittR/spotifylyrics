using System.Windows;
using SpotifyLyricsDomain;
using SpotifyLyricsDomain.ViewModels;

namespace SpotifyLyricsWPF {
    public partial class OptionsDialog : Window {
        public OptionsViewModel ViewModel { get; set; }

        public OptionsDialog() {
            InitializeComponent();
            DataContext = ViewModel = OptionsViewModel.Instance;
        }
    }
}
