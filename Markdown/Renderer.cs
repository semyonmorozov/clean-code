using System;

namespace Markdown
{
    class Renderer
    {
        public readonly int Start;
        public readonly int End;
        public readonly string Mark;
        public readonly string OpeningTag;
        public readonly string ClosingTag;
        public readonly string RawText;

        public Renderer(RenderingRule renderingRule, string rawText)
        {
            Start = renderingRule.StartOfSelection;
            End = renderingRule.EndOfSelection;
            Mark = renderingRule.Mark;
            OpeningTag = renderingRule.OpeningTag;
            ClosingTag = renderingRule.ClosingTag;
            RawText = rawText;
        }

        public string Render()
        {
            var selection = GetSelection();
            var tagedSelection = String.Concat(OpeningTag, selection, ClosingTag); 
            var renderedText = RawText.Replace(tagedSelection, Start, End - Start);
            return renderedText;
        }

        public string DeleteMarks()
        {
            var selection = GetSelection();
            var renderedText = RawText.Replace(selection, Start, End - Start);
            return renderedText;
        }

        public string GetSelection()
        {
            var selection = RawText.Substring(Start, End - Start);
            selection = selection.Substring(Mark.Length);
            selection = selection.Substring(0, selection.Length - Mark.Length);
            return selection;
        }
    }

    static class Extension
    {
        public static string Replace(this string rawString, string newPart, int start, int lenght)
        {
            return rawString.Remove(start, lenght)
                .Insert(start, newPart);
        }
    }
}
