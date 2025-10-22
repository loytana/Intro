using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4
{
    [Transaction(TransactionMode.Manual)]
    public class WallStatisticsCommandWalls : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            int totalWalls = 0;
            double maxLength = 0;
            double minLength = 0;
            double averageLength = 0;
            Wall longestWall;
            Wall shortestWall;

            var walls = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .OfType<Wall>()
                .ToList();

            if (walls.Count == 0)
            {
                TaskDialog.Show("Статистика стен", "В проекте нет стен.");
                return Result.Succeeded;
            }

            minLength = walls[0].get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble();
            shortestWall = walls[0];

            using (Transaction t = new Transaction(doc, "Статистика стен"))
            {
                t.Start();
                foreach (Wall wall in walls)
                {
                    totalWalls++;
                    Parameter length = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                    averageLength += length.AsDouble();

                    if (length.AsDouble() > maxLength)
                    {
                        longestWall = wall;
                        maxLength = length.AsDouble();
                        
                    }
                    if (length.AsDouble() < minLength)
                    {
                        shortestWall = wall;
                        minLength = length.AsDouble();
                    }
                }
                averageLength = averageLength / totalWalls;

                foreach (Wall wall in walls)
                {
                    if (maxLength == wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble())
                    {
                        Parameter commentParamMax = wall.LookupParameter("Комментарии");
                        commentParamMax.Set($"Самая длинная стена");
                    }
                    if (minLength == wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble())
                    {
                        Parameter commentParamMin = wall.LookupParameter("Комментарии");
                        commentParamMin.Set($"Самая короткая стена");
                    }
                }
                t.Commit();
                TaskDialog.Show("Готово", $"Общее количество стен: {totalWalls}\n Самая большая длина стены: {maxLength}\n Самая маленькая длина стены: {minLength}\n Средняя длина стен: {averageLength}");
            }
            return Result.Succeeded;
        }
    }
}
