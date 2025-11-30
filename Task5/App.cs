using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Task5
{
    [Transaction(TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            app.CreateRibbonTab("Статистика");
            var panel = app.CreateRibbonPanel("Статистика", "Стены");
            var button = new PushButtonData(
                "WallStats",
                "Статистика стен",
                System.Reflection.Assembly.GetExecutingAssembly().Location,
                "Task5.SelectionStatisticsCommand"
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
