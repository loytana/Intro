using Autodesk.Revit.DB;

namespace CreateSection.Abstractions
{
    public interface ISectionService
    {
        bool CreateThreeSections(
            FamilyInstance familyInsctance, 
            double widthOffsetMm, 
            double depthOffsetMm, 
            double heightOffsetMm, 
            string sectionName);
    }
}
