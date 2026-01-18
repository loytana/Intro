using System;
using System.Collections.Generic;
using System.Text;

namespace WallAnalysis.Models
{
    public class WallData
    {
        public string WallName { get; set; }
        public string WallType { get; set; }
        public double Length { get; set; } // мм
        public double Height { get; set; } // мм
        public double Thickness { get; set; } // мм
        public double Volume { get; set; } // м³
        public double Area { get; set; } // м²
        public ValidationStatus Status { get; set; }
    }

    public enum ValidationStatus
    {
        Unknown,
        Normal,
        Exceeded,
        Error
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public ValidationStatus Status { get; set; }
    }
}
