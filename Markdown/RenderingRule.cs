using System;

namespace Markdown
{
    public class RenderingRule
    {
        public readonly string Mark;
        public readonly string OpeningTag;
        public readonly string ClosingTag;
        public readonly int PriorityLevel;
        public int StartOfSelection { get; set; }
        public int EndOfSelection { get; set; }

        public RenderingRule(string mark, string openingTag, string closingTag, int priorotyLevel)
        {
            StartOfSelection = -1;
            EndOfSelection = -1;
            Mark = mark;
            OpeningTag = openingTag;
            ClosingTag = closingTag;
            PriorityLevel = priorotyLevel;
        }

        public void ResetSelection()
        {
            StartOfSelection = -1;
            EndOfSelection = -1;
        }
    }

    
}
