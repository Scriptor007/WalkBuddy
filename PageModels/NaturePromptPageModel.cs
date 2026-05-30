using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WalkBuddy.Models;

namespace WalkBuddy.PageModels
{
    [QueryProperty(nameof(MistralAnswer), "answer")]
    public partial class NaturePromptPageModel : ObservableObject
    {
        private readonly MistralService _mistralService;

        [ObservableProperty]
        private string _mistralAnswer = string.Empty;

        public Session Session { get; } = new();

        public NaturePromptPageModel(MistralService mistralService)
        {
            _mistralService = mistralService;
            Session = mistralService.CurrentSession;
        }

        [RelayCommand]
        private async Task<int> Another()
        {
            //await _mistralService.AnotherRequest();
            return 1;
        }
    }
}
