using System.Windows.Input;
using ReactiveUI;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
     private object? _currentView;
        public object? CurrentView
        {
            get =>_currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }
        
        public ICommand HomeCommand { get; }
        public ICommand PersonCommand { get; }
        public ICommand MainSettingsCommand { get; }

        private void Home() => CurrentView = new HomeViewModel();
        private void Person() => CurrentView = new PersonViewModel();
        private void MainSettings() => CurrentView = new MainSettingsViewModel();

        public MainWindowViewModel()
        {
            HomeCommand = ReactiveCommand.Create(Home);
            PersonCommand = ReactiveCommand.Create(Person);
            MainSettingsCommand = ReactiveCommand.Create(MainSettings);

            CurrentView = new HomeViewModel();
        }
}
