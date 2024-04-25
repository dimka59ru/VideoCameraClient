using System.Windows.Input;
using ReactiveUI;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
     private object? _currentView;
        public object? CurrentView
        {
            get =>_currentView;
            set
            {
                this.RaiseAndSetIfChanged(ref _currentView, value);
                RaiseProperties();
            }
        }

        public ICommand OpenVideoPanelCommand { get; }
        public ICommand PersonCommand { get; }
        public ICommand MainSettingsCommand { get; }
        
        public bool IsVideoPanelSelected => CurrentView is VideoPanelViewModel;
        public bool IsMainSettingSelected => CurrentView is MainSettingsViewModel;
        
        public MainWindowViewModel()
        {
            OpenVideoPanelCommand = ReactiveCommand.Create(VideoPanel);
            PersonCommand = ReactiveCommand.Create(Person);
            MainSettingsCommand = ReactiveCommand.Create(MainSettings);

            CurrentView = new VideoPanelViewModel();
        }
        
        private void VideoPanel() => CurrentView = new VideoPanelViewModel();
        private void Person() => CurrentView = new PersonViewModel();
        private void MainSettings() => CurrentView = new MainSettingsViewModel();

        private void RaiseProperties()
        {
            this.RaisePropertyChanged(nameof(IsVideoPanelSelected));
            this.RaisePropertyChanged(nameof(IsMainSettingSelected));
        }
}
