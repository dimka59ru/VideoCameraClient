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
        
        public ICommand OpenVideoPanelCommand { get; }
        public ICommand PersonCommand { get; }
        public ICommand MainSettingsCommand { get; }

        private void VideoPanel() => CurrentView = new VideoPanelViewModel();
        private void Person() => CurrentView = new PersonViewModel();
        private void MainSettings() => CurrentView = new MainSettingsViewModel();

        public MainWindowViewModel()
        {
            OpenVideoPanelCommand = ReactiveCommand.Create(VideoPanel);
            PersonCommand = ReactiveCommand.Create(Person);
            MainSettingsCommand = ReactiveCommand.Create(MainSettings);

            CurrentView = new VideoPanelViewModel();
        }
}
