
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
    }

    
}
