using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Consilium.Shared.Models;
using Consilium.Shared.Services;
using System.Collections.ObjectModel;

namespace Consilium.Shared.ViewModels;

public partial class DashboardViewModel : ObservableObject {
    private readonly IPersistenceService persistenceService;
    private readonly ILogInService logInService;
    private readonly IToDoService toDoService;
    private readonly IAssignmentService assignmentService;
    [ObservableProperty]
    private string username = "Guest";
    [ObservableProperty]
    private string printMessage = String.Empty;
    [ObservableProperty]
    private ObservableCollection<Assignment> assignments = new();
    [ObservableProperty]
    private ObservableCollection<TodoItem> toDos = new();
    [ObservableProperty]
    private bool online = false;
    [ObservableProperty]
    private bool showDashboard = false;
    public DashboardViewModel(IPersistenceService persistenceService, ILogInService logInService, IToDoService toDoService, IAssignmentService assignmentService) {

        var u = persistenceService.GetUserName();
        u = u.Split('@')[0];
        Username = u != String.Empty ? u : "Guest";
        this.persistenceService = persistenceService;
        this.logInService = logInService;
        this.toDoService = toDoService;
        this.assignmentService = assignmentService;
    }

    [RelayCommand]
    public async Task Initialize() {
        Online = await logInService.CheckAuthStatus();
        if (Username != "Guest" && Online) {
            IEnumerable<Assignment> a = await assignmentService.GetAllAssignmentsAsync();
            Assignments = new(a.Take(5));
            await toDoService.InitializeTodosAsync();
            ToDos = new(toDoService.GetTodoItems().Take(5));
            ShowDashboard = true;
        } else {
            ShowDashboard = false;
            PrintMessage = "You are in Guest mode, make sure to login or connect to the internet";
        }
    }
}