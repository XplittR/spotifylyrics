using System;
using System.Windows.Controls;
using SpotifyLyricsDomain;
using SpotifyLyricsDomain.Services;

namespace SpotifyLyricsWPF.Options {
    public partial class GeniusServiceUc : UserControl {
        private GeniusService MyService { get { return DataContext as GeniusService; } }
        public GeniusServiceUc() {
            InitializeComponent();
            //Note: DataContext is not available during contructor, but available later.
        }

        private void Magic_OnClicked(object sender, System.Windows.RoutedEventArgs e) {
            Console.WriteLine(MyService?.ServiceName);
        }
    }
}
