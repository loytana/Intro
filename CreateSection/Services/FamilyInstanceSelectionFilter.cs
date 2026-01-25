using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace CreateSection.Services
{
    internal class FamilyInstanceSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (!(elem is FamilyInstance fi)) 
                return false;
            return true;
        }

        public bool AllowReference(Reference reference, XYZ position) => false;
    }
}
