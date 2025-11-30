using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task6
{
    [Transaction(TransactionMode.Manual)]
    public class WallDistanceCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                IList<Reference> selectedReferences;
                try
                {
                    selectedReferences = uiDoc.Selection.PickObjects(
                        ObjectType.Element,
                        new WallSelectionFilter(),
                        "Выберите две параллельные стены");
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    TaskDialog.Show("Отмена", "Выбор отменен пользователем.");
                    return Result.Cancelled;
                }

                if (selectedReferences.Count != 2)
                {
                    TaskDialog.Show("Ошибка", "Пожалуйста, выберите ровно две стены.");
                    return Result.Failed;
                }

                Wall wall1 = doc.GetElement(selectedReferences[0]) as Wall;
                Wall wall2 = doc.GetElement(selectedReferences[1]) as Wall;

                if (wall1 == null || wall2 == null)
                {
                    TaskDialog.Show("Ошибка", "Выбранные элементы не являются стенами.");
                    return Result.Failed;
                }

                double distance = CalculateDistanceBetweenWalls(wall1, wall2);

                if (distance < 0)
                {
                    TaskDialog.Show("Ошибка", "Стены не параллельны или невозможно вычислить расстояние.");
                    return Result.Failed;
                }

                double distanceInMm = UnitUtils.ConvertFromInternalUnits(distance, DisplayUnitType.DUT_MILLIMETERS);

                string result = $"Расстояние между стенами: {distanceInMm:F1} мм\n" +
                               $"({distance * 304.8:F1} мм через коэффициент)";

                TaskDialog.Show("Результат", result);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = $"Ошибка: {ex.Message}";
                TaskDialog.Show("Ошибка", $"Не удалось выполнить команду: {ex.Message}");
                return Result.Failed;
            }
        }

        private double CalculateDistanceBetweenWalls(Wall wall1, Wall wall2)
        {
            XYZ normal1 = GetWallNormal(wall1);
            XYZ normal2 = GetWallNormal(wall2);

            if (normal1 == null || normal2 == null)
            {
                return -1;
            }

            double dotProduct = Math.Abs(normal1.DotProduct(normal2));

            if (Math.Abs(dotProduct - 1.0) > 1e-5)
            {
                return -1;
            }

            XYZ midPoint1 = GetWallMidPoint(wall1);
            XYZ midPoint2 = GetWallMidPoint(wall2);

            if (midPoint1 == null || midPoint2 == null)
            {
                return -1;
            }

            XYZ vectorBetween = midPoint2 - midPoint1;

            double distance = Math.Abs(vectorBetween.DotProduct(normal1));

            return distance;
        }

        private XYZ GetWallNormal(Wall wall)
        {
            try
            {
                LocationCurve locationCurve = wall.Location as LocationCurve;
                if (locationCurve == null || locationCurve.Curve == null)
                {
                    return null;
                }

                Curve curve = locationCurve.Curve;

                XYZ direction = curve.GetEndPoint(1) - curve.GetEndPoint(0);
                direction = direction.Normalize();

                XYZ normal = new XYZ(-direction.Y, direction.X, 0).Normalize();

                return normal;
            }
            catch
            {
                return null;
            }
        }

        private XYZ GetWallMidPoint(Wall wall)
        {
            try
            {
                LocationCurve locationCurve = wall.Location as LocationCurve;
                if (locationCurve == null || locationCurve.Curve == null)
                {
                    return null;
                }

                Curve curve = locationCurve.Curve;

                XYZ startPoint = curve.GetEndPoint(0);
                XYZ endPoint = curve.GetEndPoint(1);

                return (startPoint + endPoint) / 2.0;
            }
            catch
            {
                return null;
            }
        }
    }
}
