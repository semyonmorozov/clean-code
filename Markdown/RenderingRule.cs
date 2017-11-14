
namespace Markdown
{
    class RenderingRule
    {
        public readonly string Mark;
        public readonly string OpeningTag;
        public readonly string ClosingTag;
        public readonly int PriorityLevel;
        public StringSelector Selector;

        public RenderingRule(string mark, string openingTag, string closingTag, int priorotyLevel)
        {
            Mark = mark;
            OpeningTag = openingTag;
            ClosingTag = closingTag;
            PriorityLevel = priorotyLevel;
            Selector = new StringSelector();
        }

        public string GetSelection(string rawText)
        {
            var selection = rawText.Substring(Selector.Start, Selector.GetLength());
            selection = selection.Substring(Mark.Length);
            selection = selection.Substring(0, selection.Length - Mark.Length);
            return selection;
        }

        public bool TrySetStartMark(MarkValidator validator, string rawText, int numOfChar)
        {
            if (!validator.IsValidStartMark(Mark, rawText, numOfChar)) return false;
            Selector.Start = numOfChar;
            return true;
        }

        public bool TrySetEndMark(MarkValidator validator, string rawText, int numOfChar)
        {
            if (Selector.Start == -1) return false;
            if (!validator.IsValidEndMark(Mark, rawText, numOfChar)) return false;
            Selector.End = numOfChar + Mark.Length;
            return true;
        }


    }

    
}
