using System.Windows.Controls;
using SpotifyLyricsDomain;
using SpotifyLyricsDomain.Services;

namespace SpotifyLyricsWPF.Options {
    public partial class DefaultServiceUc : UserControl {
        private LyricsService MyService { get { return DataContext as LyricsService; } }
        public DefaultServiceUc() {
            InitializeComponent();
            //Note: DataContext is not available during contructor, but available later.
        }
    }
}
