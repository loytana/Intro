using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using WallAnalysis.Models;

namespace WallAnalysis.Abstractions
{
    public interface IRevitService
    {
        Wall GetSelectedWall(UIDocument uiDoc);
        WallData GetWallData(Wall wall);
        ValidationResult ValidateWallThickness(WallData wallData, double maxThickness);
        ValidationResult ValidateWallThickness(WallData wallData);
    }
}
