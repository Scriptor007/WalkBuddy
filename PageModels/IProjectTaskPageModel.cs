using CommunityToolkit.Mvvm.Input;
using WalkBuddy.Models;

namespace WalkBuddy.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}