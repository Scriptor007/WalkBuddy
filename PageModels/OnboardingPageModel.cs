using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WalkBuddy.Models;

namespace WalkBuddy.PageModels
{
    public partial class OnboardingPageModel : ObservableObject
    {
        private readonly UserRepository _userRepository;

        private readonly StaticDataService _staticDataService;

        [ObservableProperty]
        private User _user = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateUserCommand))]
        private string _username = string.Empty;

        public List<string> Interests { get; set; } = [];
        public List<string> Feelings { get; set; } = [];

        public OnboardingPageModel(UserRepository userRepository, StaticDataService staticDataService)
        {
            _userRepository = userRepository;
            _staticDataService = staticDataService;
            Interests = _staticDataService.Interests;
            Feelings = _staticDataService.Feelings;
        }

        [RelayCommand(CanExecute = nameof(CanSaveUser))]
        private async Task CreateUser()
        {
            User.Username = Username;
            await _userRepository.SaveUserAsync(User);
            Preferences.Default.Set("isRegistered", true);
            await Shell.Current.GoToAsync("//homepage");

        }

        [RelayCommand(CanExecute = nameof(CanSaveUser))]
        private async Task SaveUser()
        {
            User.Username = Username;
            await _userRepository.SaveUserAsync(User);
        }

        private async Task LoadUser(int id)
        {
            User user = await _userRepository.GetUserAsync(id);
            User = user;
        }

        [RelayCommand]
        private void EnableGPS()
        {
            AppInfo.Current.ShowSettingsUI();
        }

        private bool CanSaveUser() => !string.IsNullOrWhiteSpace(Username);
    }
}
