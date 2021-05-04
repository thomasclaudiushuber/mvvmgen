using Sample.WpfApp.ViewModel;
using System.Windows;

namespace Sample.WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
