using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using CreateSection.Abstractions;
using CreateSection.Services;
using CreateSection.Views;
using CreateSection.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CreateSection
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<ExternalCommandData>(commandData);
            services.AddSingleton<ISelectionService, SelectionService>();
            services.AddSingleton<ISectionService, SectionService>();
            services.AddSingleton<MainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<MainWindow, MainWindow>();
            var provider = services.BuildServiceProvider();

            var mainWindow = provider.GetRequiredService<MainWindow>();

            mainWindow.Show();
            return Result.Succeeded;
        }
    }

        //public void VisualizeTransform(Transform transform, Autodesk.Revit.DB.Document document, double scale = 3)
        //{
        //    var colors = new List<Color>()
        //    { 
        //        new Color(255, 0, 0), // X - красный
        //        new Color(0, 255, 0), // Y - зеленый
        //        new Color(0, 0, 255)  // Z - синий
        //    };
        //     var colorToLines = Enumerable.Range(0, 3)
        //    .Select(transform.get_Basis)
        //    .Select(x => Line.CreateBound(
        //        transform.Origin,
        //        transform.Origin + x * scale))
        //    .Zip(colors, (line, color) => (Line: line, Color: color))
        //    .ToList();

        //    foreach (var (line, color) in colorToLines)
        //    {
        //        var directShape = DirectShape.CreateElement(document, new ElementId
        //        (BuiltInCategory.OST_GenericModel));
        //        directShape.SetShape(new List<GeometryObject>() { line
        //        });
        //        var overrideGraphics = new OverrideGraphicSettings();
        //        overrideGraphics.SetProjectionLineColor(color);
        //        overrideGraphics.SetProjectionLineWeight(4);
        //        document.ActiveView.SetElementOverrides(directShape.Id, overrideGraphics);
        //    }
        //    }
        //}
}
