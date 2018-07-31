using System;

namespace CrossSolar.Domain
{
    public class DayResult
    {
        public DateTime Day { get; set; }

        public string  Serial { get; set; }

        public double Sum { get; set; }

        public double Count { get; set; }

        public double Min { get; set; }

        public double Max { get; set; }

        public double Avg { get; set; }
    }
}