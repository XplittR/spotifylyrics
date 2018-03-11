using System.Text;
using System.Windows;
using NLog;
using NLog.Layouts;

namespace SpotifyLyricsWPF {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            SetupLogger();
        }

        protected override void OnExit(ExitEventArgs e) {
            NLog.LogManager.Shutdown(); // Flush and close down internal threads and timer
            base.OnExit(e);
        }

        private void SetupLogger() {
            const string format = "${longdate} ${message} ${exception:format=tostring}";
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget() {
                FileName = "SpotifyLyrics.log",
                Name = "logfile",
                Layout = new SimpleLayout(format),
                Encoding = Encoding.UTF8,
            };
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Debug, logfile));

#if DEBUG
            var logconsole = new NLog.Targets.ConsoleTarget() {
                Name = "logconsole",
                Layout = new SimpleLayout(format),
                Encoding = Encoding.UTF8,
            };
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Info, logconsole));
#endif

            NLog.LogManager.Configuration = config;
        }
    }
}
