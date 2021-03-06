﻿using System;
using System.Windows;
using System.Windows.Threading;
using SpotifyLyricsDomain;
using SpotifyLyricsDomain.Helpers;
using SpotifyLyricsDomain.ViewModels;

namespace SpotifyLyricsWPF {
    public partial class MainWindow : Window {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        public BackendViewModel ViewModel { get; set; }

        public MainWindow() {
            InitializeComponent();
            OptionsViewModel.Create();
            DataContext = ViewModel = new BackendViewModel();
            ViewModel.SetLoadingLyrics();
            DispatcherTimer t = new DispatcherTimer();
            t.Tick += Tick;
            t.Interval = TimeSpan.FromSeconds(1);
            t.Start();


        }

        private void Tick(object sender, EventArgs e) {
            try {
                ViewModel.UpdateTitle();
            } catch (Exception ex) {
                Log.Error(ex, "Failed to update lyrics");
                ViewModel.WindowTitle = "error";
            }
        }

        private void Options_OnClicked(object sender, RoutedEventArgs e) {
            //todo: stop the DispatcherTimer?
            var dlg = new OptionsDialog() { Owner = this };
            dlg.ShowDialog();
        }

        private void ChangeLyrics_OnClicked(object sender, RoutedEventArgs e) {
            try {
                ViewModel.ChangeLyrics();
            } catch (Exception ex) {
                Log.Error(ex, "Failed to change lyrics");
                ViewModel.WindowTitle = "error";
            }
        }
    }
}
