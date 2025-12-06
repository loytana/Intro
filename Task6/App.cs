using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Task6
{
    [Transaction(TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            app.CreateRibbonTab("Анализ");
            var panel = app.CreateRibbonPanel("Анализ", "Стены");
            var button = new PushButtonData(
                "WallDistance",
                "Расстояние между стенами",
                System.Reflection.Assembly.GetExecutingAssembly().Location,
                "Task6.WallDistanceCommand"
                );
            panel.AddItem(button);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
