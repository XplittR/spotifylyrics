using System.Windows;
using SpotifyLyricsDomain;

namespace SpotifyLyricsWPF {
    public partial class OptionsDialog : Window {
        public OptionsViewModel ViewModel { get; set; }

        public OptionsDialog() {
            InitializeComponent();
            DataContext = ViewModel = OptionsViewModel.Instance;
        }

        private void MagicButton_OnClicked(object sender, RoutedEventArgs e) {
            Backend.ShuffleServices();
        }
    }
}
