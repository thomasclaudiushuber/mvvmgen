using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MvvmGen.ViewModels
{
  public class ViewModelBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      PropertyChanged?.Invoke(this, e);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
  }
}
