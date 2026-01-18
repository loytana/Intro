using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using WallAnalysis.Models;
using System;
using WallAnalysis.Abstractions;

namespace WallAnalysis.Services
{
    public class RevitService : IRevitService
    {
        public Wall GetSelectedWall(UIDocument uiDoc)
        {
            try
            {
                var reference = uiDoc.Selection.PickObject(
                    Autodesk.Revit.UI.Selection.ObjectType.Element,
                    new WallSelectionFilter(),
                    "Выберите стену для анализа");

                return uiDoc.Document.GetElement(reference) as Wall;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public WallData GetWallData(Wall wall)
        {
            if (wall == null) return null;

            var wallData = new WallData
            {
                WallName = wall.Name,
                WallType = wall.WallType.Name
            };

            // Получение геометрических параметров
            var options = new Options();
            var geometry = wall.get_Geometry(options);

            foreach (GeometryObject geomObj in geometry)
            {
                if (geomObj is Solid solid)
                {
                    wallData.Volume = solid.Volume * 0.0283168; // Конвертация из кубических футов в м³

                    // Расчет площади поверхности
                    double totalArea = 0;
                    foreach (Face face in solid.Faces)
                    {
                        totalArea += face.Area;
                    }
                    wallData.Area = totalArea * 0.092903; // Конвертация из квадратных футов в м²
                }
            }

            // Получение параметров из элемента
            wallData.Length = GetParameterValue(wall, BuiltInParameter.CURVE_ELEM_LENGTH) * 304.8; // футы в мм
            wallData.Height = GetParameterValue(wall, BuiltInParameter.WALL_USER_HEIGHT_PARAM) * 304.8;
            wallData.Thickness = GetParameterValue(wall, BuiltInParameter.WALL_ATTR_WIDTH_PARAM) * 304.8;

            return wallData;
        }

        public ValidationResult ValidateWallThickness(WallData wallData, double maxThickness = 500)
        {
            if (wallData == null)
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Нет данных о стене",
                    Status = ValidationStatus.Error
                };

            if (wallData.Thickness > maxThickness)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = $"Превышение! Толщина {wallData.Thickness:F0} мм > {maxThickness} мм",
                    Status = ValidationStatus.Exceeded
                };
            }

            return new ValidationResult
            {
                IsValid = true,
                Message = $"Норма. Толщина {wallData.Thickness:F0} мм ≤ {maxThickness} мм",
                Status = ValidationStatus.Normal
            };
        }

        public ValidationResult ValidateWallThickness(WallData wallData)
        {
            throw new NotImplementedException();
        }

        private double GetParameterValue(Element element, BuiltInParameter parameter)
        {
            var param = element.get_Parameter(parameter);
            return param?.AsDouble() ?? 0;
        }
    }

    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is Wall;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
