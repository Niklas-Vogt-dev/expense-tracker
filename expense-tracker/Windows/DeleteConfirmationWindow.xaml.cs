using System.Windows;

namespace expense_tracker.Windows
{
    public partial class DeleteConfirmationWindow : Window
    {
        public bool DontShowAgain { get; private set; }

        public DeleteConfirmationWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        private void DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            DontShowAgain = ChkDontShowAgain.IsChecked == true;

            DialogResult = true;
        }

        private void CancelDeletion_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
