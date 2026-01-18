using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using WallAnalysis.Services;
using WallAnalysis.ViewModels;

namespace WallAnalysis.Abstractions
{
    public static class DependencyInjection
    {
        public static IServiceProvider ConfigureServices(UIDocument uiDocument)
        {
            var services = new ServiceCollection();

            // Регистрация сервисов
            services.AddSingleton<IRevitService, RevitService>();

            // Регистрация ViewModel с зависимостью UIDocument
            services.AddTransient<MainViewModel>(provider =>
                new MainViewModel(
                    provider.GetRequiredService<IRevitService>(),
                    uiDocument
                ));

            return services.BuildServiceProvider();
        }
    }
}
