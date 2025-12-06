using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Task7
{
    [Transaction(TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            app.CreateRibbonTab("Анализ");
            var panel = app.CreateRibbonPanel("Анализ", "Стены");
            var button = new PushButtonData(
                "WallStat",
                "Статистика элемента",
                System.Reflection.Assembly.GetExecutingAssembly().Location,
                "Task7.FamilyGeometryStatisticsCommand"
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
