using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace WalkBuddy.Utilities
{
    public class CoordinatesDto
    {
        public double? Longitude { get; set; } = null;
        public double? Latitude { get; set; } = null;

        public string? Context { get; set; } = string.Empty;

        public CoordinatesDto(double? longitude = null, double? latitude = null, string? context = null)
        {
            Longitude = longitude;
            Latitude = latitude;
            Context = context;
        }
    }
}
