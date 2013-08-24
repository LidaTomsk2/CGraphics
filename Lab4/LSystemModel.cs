using System.Collections.Generic;

namespace Lab4
{
    public class LSystemModel
    {
        public string Name { get; set; }
        public int StepSize { get; set; }
        public double Angle { get; set; }
        public int DeepValue { get; set; }
        public List<LSystemRule> Rules { get; set; }
    }

    public class LSystemRule
    {
        public char Name { get; set; }
        public string Value { get; set; }
    }
}
