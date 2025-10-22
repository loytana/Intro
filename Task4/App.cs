using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4
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
                "Task4.WallStatisticsCommand"
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
