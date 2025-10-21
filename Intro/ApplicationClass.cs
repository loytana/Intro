using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using System;
using System.Windows.Media.Imaging;

namespace Intro
{
    [Transaction(TransactionMode.Manual)]
    public class ApplicationClass : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab("ПИК-Привет");
            var panel = application.CreateRibbonPanel("ПИК-Привет", "Общее");
            var button = new PushButtonData(
                "Hello",
                "Привет",
                "C:\\Software Development Kit\\Samples\\StructSample\\CS\\bin\\Debug\\StructSample.dll",
                "Revit.SDK.Samples.StructSample.CS.Command"
                );
            BitmapImage bitmapImage = new BitmapImage(new Uri("C:\\Users\\User\\AppData\\Roaming\\Autodesk\\Revit\\Addins\\2019\\Test\\img\\images.png", UriKind.Absolute));
            button.LargeImage = bitmapImage;
            panel.AddItem(button);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
