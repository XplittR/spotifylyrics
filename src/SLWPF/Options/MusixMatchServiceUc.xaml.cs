using System.Windows.Controls;
using SpotifyLyricsDomain;

namespace SpotifyLyricsWPF.Options {
    public partial class MusixMatchServiceUc : UserControl {
        private MusixMatchService MyService { get { return DataContext as MusixMatchService; } }
        public MusixMatchServiceUc() {
            InitializeComponent();
            //Note: DataContext is not available during contructor, but available later.
        }
    }
}
