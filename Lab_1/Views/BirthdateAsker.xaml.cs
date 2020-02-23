using Lab_1.ViewModels;

namespace Lab_1.Views
{
    public partial class BirthdateAsker
    {
        public BirthdateAsker()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
        }
    }
}