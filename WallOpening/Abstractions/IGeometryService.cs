using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallOpening.Models;

namespace WallOpening.Abstractions
{
    public interface IGeometryService
    {
        OpeningInfo GetOpeningInfo(FamilyInstance opening, double limit);
    }
}
