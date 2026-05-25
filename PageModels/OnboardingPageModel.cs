using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WalkBuddy.Models;

namespace WalkBuddy.PageModels
{
    public partial class OnboardingPageModel : ObservableObject
    {
        private readonly UserRepository _userRepository;

        private readonly StaticDataService _staticDataService;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        [ObservableProperty]
        private User _user = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateUserCommand))]
        private string _username = string.Empty;

        public List<string> Interests { get; set; } = [];
        public List<string> ChallengingStates { get; set; } = [];
        public List<string> PositiveStates { get; set; } = [];
        public List<string> Mobility { get; set; } = [];

        public OnboardingPageModel(UserRepository userRepository, StaticDataService staticDataService)
        {
            _userRepository = userRepository;
            _staticDataService = staticDataService;
            Interests = _staticDataService.Interests;
            ChallengingStates = _staticDataService.ChallengingStates;
            PositiveStates = _staticDataService.PositiveStates;
            Mobility = _staticDataService.Mobility;
        }

        [RelayCommand(CanExecute = nameof(CanSaveUser))]
        private async Task CreateUser()
        {
            User.Username = Username;
            await _userRepository.SaveUserAsync(User);
            Preferences.Default.Set("isRegistered", true);
            var toast = Toast.Make("Successfully saved data", CommunityToolkit.Maui.Core.ToastDuration.Short);
            await toast.Show(_cancellationTokenSource.Token);
            await Shell.Current.GoToAsync("//homepage");

        }

        [RelayCommand]
        private void EnableGPS()
        {
            AppInfo.Current.ShowSettingsUI();
        }

        private bool CanSaveUser() => !string.IsNullOrWhiteSpace(Username);
    }
}
