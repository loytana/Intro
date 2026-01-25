using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;
using CreateSection.Abstractions;

namespace CreateSection.Services
{
    public class SectionService : ISectionService
    {
        private readonly ExternalCommandData _commandData;

        public SectionService(ExternalCommandData commandData)
        {
            _commandData = commandData;
        }
        public bool CreateThreeSections(
            FamilyInstance instance, 
            double widthOffsetMm, 
            double depthOffsetMm, 
            double heightOffsetMm, 
            string sectionName)
        {
            var doc = _commandData.Application.ActiveUIDocument.Document;
            var bbox = instance.get_BoundingBox(null);

            if (bbox == null)
            {
                return false;
            }

            var center = (bbox.Min + bbox.Max) / 2;
            var size = bbox.Max - bbox.Min;

            Transform transform = Transform.CreateTranslation(XYZ.Zero);
            transform.Origin = XYZ.Zero;
            transform.BasisX = (XYZ.BasisZ.CrossProduct(XYZ.BasisY)).Normalize();
            transform.BasisY = XYZ.BasisZ;
            transform.BasisZ = XYZ.BasisY;

            var widthOffset = UnitUtils.ConvertToInternalUnits(widthOffsetMm, DisplayUnitType.DUT_MILLIMETERS);
            var depthOffset = UnitUtils.ConvertToInternalUnits(depthOffsetMm, DisplayUnitType.DUT_MILLIMETERS);
            var heightOffset = UnitUtils.ConvertToInternalUnits(heightOffsetMm, DisplayUnitType.DUT_MILLIMETERS);

            var viewType = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .OfType<ViewFamilyType>()
                .FirstOrDefault(x => x.ViewFamily == ViewFamily.Section);
            if (viewType == null)
            {
                return false;
            }

            try
            {
                using (var transaction = new Transaction(doc, "VisulalizaTransform"))
                {
                    transaction.Start();
                    // 1. ГОРИЗОНТАЛЬНЫЙ РАЗРЕЗ (XY плоскость, ПЛАН)
                    var sectionBoxXY = CreateSectionBox(
                        center: center,
                        basisX: XYZ.BasisX,
                        basisY: XYZ.BasisY,
                        basisZ: XYZ.BasisZ,
                        size: size,
                        widthOffset: widthOffset,
                        depthOffset: depthOffset,
                        heightOffset: heightOffset
                    );

                    var viewSectionXY = ViewSection.CreateSection(doc, viewType.Id, sectionBoxXY);
                    viewSectionXY.Name = $"{sectionName}_План";

                    // 2. ФРОНТАЛЬНЫЙ РАЗРЕЗ (XZ плоскость, ФРОНТ)
                    var sectionBoxXZ = CreateSectionBox(
                        center: center,
                        basisX: XYZ.BasisX,
                        basisY: XYZ.BasisZ,
                        basisZ: XYZ.BasisY,
                        size: size,
                        widthOffset: widthOffset,
                        depthOffset: heightOffset,    // меняем местами для этой плоскости
                        heightOffset: depthOffset     // меняем местами для этой плоскости
                    );

                    var viewSectionXZ = ViewSection.CreateSection(doc, viewType.Id, sectionBoxXZ);
                    viewSectionXZ.Name = $"{sectionName}_Фронт";

                    // 3. ПРОФИЛЬНЫЙ РАЗРЕЗ (YZ плоскость, ПРОФИЛЬ)
                    var sectionBoxYZ = CreateSectionBox(
                        center: center,
                        basisX: XYZ.BasisY,
                        basisY: XYZ.BasisZ,
                        basisZ: XYZ.BasisX,
                        size: size,
                        widthOffset: depthOffset,     // меняем местами для этой плоскости
                        depthOffset: heightOffset,    // меняем местами для этой плоскости
                        heightOffset: widthOffset     // меняем местами для этой плоскости
                    );

                    var viewSectionYZ = ViewSection.CreateSection(doc, viewType.Id, sectionBoxYZ);
                    viewSectionYZ.Name = $"{sectionName}_Профиль";
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }    
    private BoundingBoxXYZ CreateSectionBox(
            XYZ center,
            XYZ basisX,
            XYZ basisY,
            XYZ basisZ,
            XYZ size,
            double widthOffset,
            double depthOffset,
            double heightOffset)
        {
            var transform = Transform.Identity;
            transform.Origin = center;
            transform.BasisX = basisX;
            transform.BasisY = basisY;
            transform.BasisZ = basisZ;

            return new BoundingBoxXYZ
            {
                Transform = transform,
                Min = new XYZ(
                    -size.X / 2 - widthOffset,
                    -size.Y / 2 - depthOffset,
                    -size.Z / 2 - heightOffset
                ),
                Max = new XYZ(
                    size.X / 2 + widthOffset,
                    size.Y / 2 + depthOffset,
                    size.Z / 2 + heightOffset
                )
            };
        }
    }
}
