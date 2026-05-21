using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using WalkBuddy.Models;

namespace WalkBuddy.PageModels
{
    public partial class OnboardingPageModel : ObservableObject
    {
        private readonly UserRepository _userRepository;

        [ObservableProperty]
        private User _user = new User();

        public OnboardingPageModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [RelayCommand]
        private async Task SaveUser()
        {
            await _userRepository.SaveUserAsync(User);
        }

        private async Task LoadUser(int id)
        {
            User user = await _userRepository.GetUserAsync(id);
            User = user;
        }
    }
}
