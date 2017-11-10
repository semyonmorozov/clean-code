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

        public string Render(string rawText)
        {
            var selection = GetSelection(rawText);
            var tagedSelection = ReplaceMarksToTags(selection);
            var renderedText = rawText.Remove(StartOfSelection, EndOfSelection - StartOfSelection)
                .Insert(StartOfSelection, tagedSelection);

            StartOfSelection = -1;
            EndOfSelection = -1;

            return renderedText;
        }

        private string GetSelection(string rawText)
        {
            var selection = rawText.Substring(StartOfSelection, EndOfSelection - StartOfSelection);
            return selection;
        }

        private string ReplaceMarksToTags(string selection)
        {
            selection = selection.Substring(Mark.Length);
            selection = selection.Substring(0, selection.Length - Mark.Length);
            var tagedSelection = String.Concat(OpeningTag, selection, ClosingTag);
            return tagedSelection;
        }
    }
}
