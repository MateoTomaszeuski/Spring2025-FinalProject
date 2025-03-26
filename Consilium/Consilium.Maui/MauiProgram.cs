using CommunityToolkit.Maui;
using Consilium.Maui.Views;
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
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Inter.ttf", "Inter");
                fonts.AddFont("fa-free-regular.ttf", "FontAwesomeRegular");
                fonts.AddFont("fa-free-solid.ttf", "FontAwesomeSolid");
            });


#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddHttpClient("client", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5202");
        });

        return builder.Build();

    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder builder) {
        builder.Services.AddSingleton<AssignmentsView>();
        builder.Services.AddSingleton<DashboardView>();
        builder.Services.AddSingleton<StatsView>();
        builder.Services.AddSingleton<TodoListView>();
        builder.Services.AddSingleton<ToolsView>();

        return builder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder) {
        builder.Services.AddSingleton<AssignmentsViewModel>();
        builder.Services.AddSingleton<DashboardViewModel>();
        builder.Services.AddSingleton<StatsViewModel>();
        builder.Services.AddSingleton<TodoListViewModel>();
        builder.Services.AddSingleton<ToolsViewModel>();

        return builder;
    }

}