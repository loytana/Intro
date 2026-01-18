using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using System;
using WallAnalysis.Abstractions;
using WallAnalysis.ViewModels;
using WallAnalysis.Views;

namespace WallAnalysis
{
    [Transaction(TransactionMode.ReadOnly)]
    [Regeneration(RegenerationOption.Manual)]
    public class Cmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements, MainViewModel viewModel)
        {
            try
            {
                var uiApp = commandData.Application;
                var uiDoc = uiApp.ActiveUIDocument;

                // Настройка DI
                var serviceProvider = DependencyInjection.ConfigureServices(uiDoc);

                // Создание ViewModel и View
                var _viewModel = serviceProvider.GetRequiredService<MainViewModel>();
                var window = new MainWindow
                {
                    DataContext = viewModel,
                    Owner = null // Или передать родительское окно Revit
                };
                _viewModel.CloseCommand = new RelayCommand(() => window.Close());

                window.ShowDialog();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            throw new NotImplementedException();
        }
    }
}
