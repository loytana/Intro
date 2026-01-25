using Autodesk.Revit.DB;

namespace CreateSection.Abstractions
{
    public interface ISelectionService
    {
        FamilyInstance PickFamilyInstance();
    }
}
