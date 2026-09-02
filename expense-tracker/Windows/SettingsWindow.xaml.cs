using System.Windows;

namespace expense_tracker.Windows
{
    public partial class SettingsWindow : Window
    {
        public string UserName { get; private set; }

        public SettingsWindow(string userName)
        {
            InitializeComponent();

            UserName = userName;
            NameTextBox.Text = userName;

            NameTextBox.Focus();
            NameTextBox.SelectAll();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(
                    "Bitte gib einen Namen ein.",
                    "Ungültiger Name",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            UserName = name;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}