using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallOpening.Abstractions
{
    public interface ISelectionService
    {
        FamilyInstance PickOpening();
    }
}
