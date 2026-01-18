using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallOpening.Abstractions;
using WallOpening.Services;
using WallOpening.ViewModels;
using WallOpening.Views;

namespace WallOpening
{
    [Transaction(TransactionMode.Manual)]
    internal class Cmd : IExternalCommand

    {        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<ExternalCommandData>(commandData);
            services.AddSingleton<ISelectionService, SelectionService>();
            services.AddSingleton<IGeometryService, GeometryService>();
            services.AddSingleton<MainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<MainWindow, MainWindow>();
            var provider = services.BuildServiceProvider();

            var mainWindow = provider.GetRequiredService<MainWindow>();
            
            mainWindow.Show();
            return Result.Succeeded;
        }
    }
}
