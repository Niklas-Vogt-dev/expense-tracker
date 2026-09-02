using System.Windows;
using expense_tracker.Data;
using expense_tracker.Windows;

namespace expense_tracker
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            AppSettings settings = SettingsManager.Load();

            if (string.IsNullOrWhiteSpace(settings.UserName))
            {
                WelcomeWindow welcomeWindow = new WelcomeWindow();

                bool? result = welcomeWindow.ShowDialog();

                if (result != true)
                {
                    Shutdown();
                    return;
                }

                settings.UserName = welcomeWindow.UserName;
                SettingsManager.Save(settings);
            }

            MainWindow mainWindow = new MainWindow();

            MainWindow = mainWindow;

            ShutdownMode = ShutdownMode.OnMainWindowClose;

            mainWindow.Show();
        }
    }
}