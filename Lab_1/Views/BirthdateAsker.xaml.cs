using Lab_1.ViewModels;

namespace Lab_1.Views
{
    public partial class BirthdateAsker
    {
        internal BirthdateAsker()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
        }
    }
}