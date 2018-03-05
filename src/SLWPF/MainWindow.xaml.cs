using System;
using System.Windows;
using System.Windows.Threading;
using SpotifyLyricsDomain;

namespace SpotifyLyricsWPF {
    public partial class MainWindow : Window {
        public Backend ViewModel { get; set; }

        public MainWindow() {
            InitializeComponent();
            DataContext = ViewModel = new Backend();
            DispatcherTimer t = new DispatcherTimer();
            t.Tick += Tick;
            t.Interval = TimeSpan.FromSeconds(1);
            t.Start();
        }

        private void Tick(object sender, EventArgs e) {
            ViewModel.UpdateTitle();
        }
    }
}
