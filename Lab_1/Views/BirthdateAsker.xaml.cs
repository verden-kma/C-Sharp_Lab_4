using System.Windows;
using System.Windows.Controls;
using Lab_1.ViewModels;

namespace Lab_1.Views
{
    public partial class BirthdateAsker : UserControl
    {
        public BirthdateAsker()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
        }
    }
}