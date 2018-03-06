using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SpotifyLyricsDomain;

namespace SpotifyLyricsWPF {
    public partial class OptionsDialog : Window {
        public OptionsViewModel ViewModel { get; set; }

        private OptionsDialog() {
            InitializeComponent();
        }

        public OptionsDialog(OptionsViewModel viewModel)
            : this() {
            DataContext = ViewModel = viewModel;
        }

        private void MagicButton_OnClicked(object sender, RoutedEventArgs e) {
            Backend.ShuffleServices();
        }
    }
}
