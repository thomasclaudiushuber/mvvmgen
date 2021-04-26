using MvvmGen.WpfApp.ViewModel;
using System.Windows;

namespace MvvmGen.WpfApp
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      DataContext = new EmployeeViewModel();
    }
  }
}
