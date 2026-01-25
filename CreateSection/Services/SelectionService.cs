using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using CreateSection.Abstractions;

namespace CreateSection.Services
{
    public class SelectionService : ISelectionService
    {
        private readonly ExternalCommandData _commandData;

        public SelectionService(ExternalCommandData commandData)
        {
            _commandData = commandData;
        }
        public FamilyInstance PickFamilyInstance()
        {
            try
            {
                Reference reference = _commandData.Application.ActiveUIDocument.Selection.PickObject(ObjectType.Element, new FamilyInstanceSelectionFilter());
                FamilyInstance familyInstance = _commandData.Application.ActiveUIDocument.Document.GetElement(reference) as FamilyInstance;
                return familyInstance;
            }
            catch
            {
                return null;
            }
        }
    }
}
