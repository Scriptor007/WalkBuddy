using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using WalkBuddy.Models;

namespace WalkBuddy.PageModels
{
    public partial class EditProfilePageModel : ObservableObject
    {
        private readonly UserRepository _userRepository;

        private readonly StaticDataService _staticDataService;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private bool _isBusy;

        [ObservableProperty]
        private User _currentUser = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateUserCommand))]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _placeholderInfo = string.Empty;

        public List<string> Interests { get; set; } = [];
        public List<string> ChallengingStates { get; set; } = [];
        public List<string> PositiveStates { get; set; } = [];
        public List<string> Mobility { get; set; } = [];

        public EditProfilePageModel(UserRepository userRepository, StaticDataService staticDataService)
        {
            _userRepository = userRepository;
            _staticDataService = staticDataService;
            Interests = _staticDataService.Interests;
            ChallengingStates = _staticDataService.ChallengingStates;
            PositiveStates = _staticDataService.PositiveStates;
            Mobility = _staticDataService.Mobility;
        }

        [RelayCommand]
        private async Task Appearing()
        {
            if (_isBusy)
                return;

            _isBusy = true;

            try
            {
                var user = await _userRepository.GetUserAsync();

                BuildUser(user);
                Username = CurrentUser.Username;
                PlaceholderInfo = CurrentUser.AdditionalInfo;
            }
            finally
            {
                _isBusy = false;
            }
        }

        private void BuildUser(User user)
        {
            CurrentUser.ID = user.ID;
            CurrentUser.Username = user.Username;
            CurrentUser.AdditionalInfo = user.AdditionalInfo;
            CurrentUser.Mobility = user.Mobility;

            CurrentUser.Interests.Clear();
            foreach (var item in user.Interests)
            {
                CurrentUser.Interests.Add(item);
            }

            CurrentUser.PositiveStates.Clear();
            foreach (var item in user.PositiveStates)
            {
                CurrentUser.PositiveStates.Add(item);
            }

            CurrentUser.ChallengingStates.Clear();
            foreach (var item in user.ChallengingStates)
            {
                CurrentUser.ChallengingStates.Add(item);
            } 
        }


        [RelayCommand(CanExecute = nameof(CanUpdateUser))]
        private async Task UpdateUser()
        {
            if (_isBusy)
                return;

            _isBusy = true;

            try
            {
                CurrentUser.Username = Username;
                await _userRepository.SaveUserAsync(CurrentUser);
                var toast = Toast.Make("Successfully updated your account", CommunityToolkit.Maui.Core.ToastDuration.Short);
                await toast.Show(_cancellationTokenSource.Token);
                await Shell.Current.GoToAsync("//start");
            }
            finally
            {
                _isBusy = false;
            }
        }

        [RelayCommand]
        private void EnableGPS()
        {
            AppInfo.Current.ShowSettingsUI();
        }

        private bool CanUpdateUser() => !string.IsNullOrWhiteSpace(Username);
    }
}
