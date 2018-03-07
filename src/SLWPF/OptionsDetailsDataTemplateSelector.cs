using System.Windows;
using System.Windows.Controls;
using SpotifyLyricsDomain;

namespace SpotifyLyricsWPF {
    public class OptionsDetailsDataTemplateSelector : DataTemplateSelector {
        public override DataTemplate SelectTemplate(object item,
            DependencyObject container) {
            LyricsService obj = item as LyricsService;
            if (obj is GeniusService) {
                return Application.Current.FindResource("OptionsGeniusDataTemplate") as DataTemplate;
            } else if (obj is MusixMatchService) {
                return Application.Current.FindResource("OptionsMusixMatchTemplate") as DataTemplate;
            }

            return Application.Current.FindResource("OptionsDefaultDataTemplate") as DataTemplate;
        }
    }
}