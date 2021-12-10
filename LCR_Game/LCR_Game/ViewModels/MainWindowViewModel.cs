using Prism.Mvvm;

namespace LCR_Game.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "LCR Game";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public MainWindowViewModel()
        {

        }
    }
}
