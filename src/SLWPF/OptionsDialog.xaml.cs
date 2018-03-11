using System;
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

        private void OptionsDialog_OnClosed(object sender, EventArgs e) {
            ViewModel.SaveSettings();
        }
    }
}
