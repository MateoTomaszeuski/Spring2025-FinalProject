using CommunityToolkit.Maui;
using Consilium.Maui.Views;
using Consilium.Shared.Models;
using Consilium.Shared.Services;
using Consilium.Shared.ViewModels;
using Microsoft.Extensions.Logging;

namespace Consilium.Maui;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .RegisterViews()
            .RegisterViewModels()
            .RegisterServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Inter.ttf", "Inter");
                fonts.AddFont("fa-free-regular.otf", "FontAwesomeRegular");
                fonts.AddFont("fa-free-solid.otf", "FontAwesomeSolid");
            });


#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddHttpClient("ApiClient", client =>
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS) {
                client.BaseAddress = new Uri("https://main.consilium.duckdns.org/todo");
            } else {
                client.BaseAddress = new Uri("http://localhost:5202");
            }
        });


        var app = builder.Build();

        using (var scope = app.Services.CreateScope()) {
            var service = scope.ServiceProvider.GetRequiredService<IPersistenceService>();
            service.OnStartup();
        }

        return app;

    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder) {
        builder.Services.AddSingleton<AssignmentsView>();
        builder.Services.AddSingleton<DashboardView>();
        builder.Services.AddSingleton<StatsView>();
        builder.Services.AddSingleton<TodoListView>();
        builder.Services.AddSingleton<ToolsView>();
        builder.Services.AddSingleton<ProfileView>();

        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder) {
        builder.Services.AddSingleton<AssignmentsViewModel>();
        builder.Services.AddSingleton<DashboardViewModel>();
        builder.Services.AddSingleton<StatsViewModel>();
        builder.Services.AddSingleton<TodoListViewModel>();
        builder.Services.AddSingleton<ToolsViewModel>();
        builder.Services.AddSingleton<ProfileViewModel>();

        return builder;
    }
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder) {
        builder.Services.AddSingleton<IToDoService, ToDoService>();
        builder.Services.AddSingleton<IPersistenceService, PersistenceService>();
        builder.Services.AddSingleton<ILogInService, LogInService>();
        builder.Services.AddSingleton<IClientService, ClientService>();

        return builder;
    }
}