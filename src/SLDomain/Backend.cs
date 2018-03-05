using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace SpotifyLyricsDomain {
    [AddINotifyPropertyChangedInterface]
    public class Backend  {
        public string WindowTitle { get; set; }
        public string Lyrics { get; set; }

        public void UpdateTitle() {
            WindowTitle = Class1.GetWindowTitle();
        }
    }
}
