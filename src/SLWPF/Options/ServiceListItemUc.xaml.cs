using System.Windows;
using System.Windows.Controls;
using SpotifyLyricsDomain.Services;
using SpotifyLyricsDomain.ViewModels;

namespace SpotifyLyricsWPF.Options {
    public partial class ServiceListItemUc : UserControl {
        private LyricsService MyService { get { return DataContext as LyricsService; } }
        public ServiceListItemUc() {
            InitializeComponent();
        }

        private void OrderUp_OnClick(object sender, RoutedEventArgs e) {
            OptionsViewModel.Instance.OrderUp(MyService);
        }
        private void OrderDown_OnClick(object sender, RoutedEventArgs e) {
            OptionsViewModel.Instance.OrderDown(MyService);
        }
    }
}
