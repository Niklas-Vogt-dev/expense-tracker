using System.Windows;

namespace expense_tracker.Windows
{
    public partial class WelcomeWindow : Window
    {
        public string UserName { get; private set; } = "";

        public WelcomeWindow()
        {
            InitializeComponent();

            NameTextBox.Focus();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(
                    "Bitte gib deinen Namen ein.",
                    "Name fehlt",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                NameTextBox.Focus();
                return;
            }

            UserName = name;
            DialogResult = true;
        }
    }
}