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

        [ObservableProperty]
        private User _currentUser = new();

        [ObservableProperty]
        private User _editedUser = new();

        private bool _isInitialized = false;

        public List<string> Interests { get; set; } = [];

        public List<string> Feelings { get; set; } = [];

        public EditProfilePageModel(UserRepository userRepository, StaticDataService staticDataService)
        {
            _userRepository = userRepository;
            _staticDataService = staticDataService;
            Interests = _staticDataService.Interests;
            Feelings = _staticDataService.Feelings;
        }

        [RelayCommand]
        private async Task InitialUserLoad()
        {
            if (_isInitialized)
            {
                await SetEditUser();
                return;
            }

            _isInitialized = true;
            CurrentUser = await _userRepository.GetUserAsync();
            await SetEditUser();
        }

        private async Task SetEditUser()
        {
            if (CurrentUser.InterestsString != null)
            {
                EditedUser.Interests.Clear();
                string[] interests = CurrentUser.InterestsString.Split(',');
                foreach (var interest in interests)
                {
                    EditedUser.Interests.Add(interest);
                }
            }

            if (CurrentUser.FeelingsString != null)
            {
                EditedUser.Feelings.Clear();
                string[] feelings = CurrentUser.FeelingsString.Split(',');
                foreach (var feeling in feelings)
                {
                    EditedUser.Feelings.Add(feeling);
                }
            }
        }

        [RelayCommand]
        private void EnableGPS()
        {
            AppInfo.Current.ShowSettingsUI();
        }
    }
}
