using Autodesk.Revit.DB;
using System;
using WallOpening.Abstractions;
using WallOpening.Models;

namespace WallOpening
{
    internal class GeometryService : IGeometryService
    {
        public OpeningInfo GetOpeningInfo(FamilyInstance opening, double limit)
        {
            double currentDistance = GetCurrentDistance(opening);
            return new OpeningInfo()
            {
                Name = opening.Name,
                Distance = currentDistance,
                IsCorrect = currentDistance < limit
            };
        }

        private double GetCurrentDistance(FamilyInstance opening)
        {
            double maxZOpening = GetMaxZ(opening);
            double maxZHost = GetMaxZ(opening.Host);

            return UnitUtils.ConvertFromInternalUnits( maxZHost - maxZOpening, DisplayUnitType.DUT_MILLIMETERS);
            
        }

        private double GetMaxZ(Element element)
        {
            var box = element.get_BoundingBox(null);
            return box.Max.Z;
        }
    }
}
