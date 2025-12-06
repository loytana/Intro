using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Task7
{
    [Transaction(TransactionMode.Manual)]
    public class FamilyGeometryStatisticsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIDocument uiDoc = commandData.Application.ActiveUIDocument;
                Document doc = uiDoc.Document;

                Element elem = doc.GetElement(uiDoc.Selection.PickObject(ObjectType.Element, "Выберите элемент"));

                GeometryElement geom = elem.get_Geometry(new Options());

                double vol = 0;
                double area = 0;
                int faces = 0;
                int edges = 0;
                double edgesLen = 0;

                foreach (GeometryObject obj in geom)
                {
                    if (obj is Solid solid && solid.Volume > 0)
                    {
                        vol += solid.Volume;

                        foreach (Face f in solid.Faces)
                        {
                            area += f.Area;
                            faces++;
                        }

                        foreach (Edge e in solid.Edges)
                        {
                            edges++;
                            edgesLen += e.ApproximateLength;
                        }
                    }
                }

                TaskDialog.Show("Статистика",
                    $"Объем: {UnitUtils.ConvertFromInternalUnits(vol, DisplayUnitType.DUT_CUBIC_METERS):F2} м³\n" +
                    $"Площадь: {UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS):F2} м²\n" +
                    $"Граней: {faces}\n" +
                    $"Ребер: {edges}\n" +
                    $"Длина ребер: {UnitUtils.ConvertFromInternalUnits(edgesLen, DisplayUnitType.DUT_METERS):F2} м");

                return Result.Succeeded;
            }
            catch
            {
                return Result.Cancelled;
            }
        }

    }
}
