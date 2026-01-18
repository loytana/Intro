using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallOpening.Services
{
    internal class OpeningSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (!(elem is FamilyInstance fi)) return false;

            return fi.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Doors ||
                fi.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Windows;
        }

        public bool AllowReference(Reference reference, XYZ position) => false;
    }
}
