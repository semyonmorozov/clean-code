using System;

namespace Markdown
{
    public class Renderer
    {
        public readonly char EscapeChar;
        public readonly string Mark;
        public readonly string OpeningTag;
        public readonly string ClosingTag;
        public int StartOfSelection { get; protected set; }
        public int EndOfSelection { get; protected set; }

        public Renderer(string mark,string openingTag, string closingTag, char escapeChar='\\')
        {
            StartOfSelection = -1;
            EndOfSelection = -1;
            EscapeChar = escapeChar;
            Mark = mark;
            OpeningTag = openingTag;
            ClosingTag = closingTag;
        }

        public string Render(string rawText)
        {
            var selection = GetSelection(rawText);
            var tagedSelection = ReplaceMrarksToTags(selection);
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

        private string ReplaceMrarksToTags(string selection)
        {
            selection = selection.Substring(Mark.Length);
            selection = selection.Substring(0, selection.Length - Mark.Length);
            var tagedSelection = String.Concat(OpeningTag, selection, ClosingTag);
            return tagedSelection;
        }

        public bool TrySetStartOfSelection(string rawText, int numOfChar)
        {
            if (rawText.Length < numOfChar + Mark.Length + 1) return false;
            if (!rawText.Substring(numOfChar, Mark.Length).Equals(Mark)) return false;
            if (numOfChar != 0)
                if(!Char.IsWhiteSpace(rawText[numOfChar - 1])) return false;
            if (Char.IsWhiteSpace(rawText[numOfChar + Mark.Length])) return false;
            if (rawText[numOfChar + Mark.Length] == '_') return false;
            StartOfSelection = numOfChar;
            return true;
        }

        public bool TrySetEndOfSelection(string rawText, int numOfChar)
        {
            if (StartOfSelection == -1) return false;
            if (rawText.Length < numOfChar + Mark.Length + 1) return false;
            if (!rawText.Substring(numOfChar, Mark.Length).Equals(Mark)) return false;
            if (numOfChar != rawText.Length-1)
                if(!Char.IsWhiteSpace(rawText[numOfChar + Mark.Length])) return false;
            if (Char.IsWhiteSpace(rawText[numOfChar - 1])) return false;
            if (rawText[numOfChar -1] == '_') return false;
            EndOfSelection = numOfChar+Mark.Length;
            return true;
        }
    }
}
