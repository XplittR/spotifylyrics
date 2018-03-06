using System;
using System.Windows;
using System.Windows.Threading;
using SpotifyLyricsDomain;

namespace SpotifyLyricsWPF {
    public partial class MainWindow : Window {
        public BackendViewModel ViewModel { get; set; }

        public MainWindow() {
            InitializeComponent();
            DataContext = ViewModel = new BackendViewModel();
            DispatcherTimer t = new DispatcherTimer();
            t.Tick += Tick;
            t.Interval = TimeSpan.FromSeconds(1);
            t.Start();

        }

        private void Tick(object sender, EventArgs e) {
            try {
                ViewModel.UpdateTitle();
            } catch (Exception ex) {
                ViewModel.WindowTitle = "error";
            }
        }

        private void Options_OnClicked(object sender, RoutedEventArgs e) {
            //todo: stop the timer.
            var dlg = new OptionsDialog(new OptionsViewModel()) { Owner = this };
            dlg.ShowDialog();
        }
    }
}
