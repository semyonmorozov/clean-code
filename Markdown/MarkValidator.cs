using System.Collections.Generic;

namespace Markdown
{
    class MarkValidator
    {
        private readonly List<string> marks;
        public MarkValidator(List<string> marks)
        {
            this.marks = marks;
        }

        public bool IsValidStartMark(string mark, string rawText, int numOfChar)
        {
            return IsStartMark(mark, rawText, numOfChar) 
                && IsValidStartEnvironment(mark, rawText, numOfChar);
        }

        private bool IsStartMark(string mark, string rawText, int numOfChar)
        {
            return rawText.Length >= numOfChar + mark.Length + 1 
                && rawText.Substring(numOfChar, mark.Length).Equals(mark);
        }

        private bool IsValidStartEnvironment(string mark, string rawText, int numOfChar)
        {
            if (numOfChar != 0)
            {
                var externalChar = rawText[numOfChar - 1];
                if (!IsAcceptedExternalChar(externalChar)) return false;
            }
            if (numOfChar+mark.Length+1<rawText.Length)
            {
                var internalChar = rawText[numOfChar + mark.Length];
                if (!IsAcceptedInternalChar(internalChar)) return false;
            }

            return true;
        }

        public bool IsValidEndMark(string mark, string rawText, int numOfChar)
        {
            return IsEndMark(mark, rawText, numOfChar) 
                && IsValidEndEnvironment(mark, rawText, numOfChar);
        }

        private bool IsEndMark(string mark, string rawText, int numOfChar)
        {
            return rawText.Length >= numOfChar + mark.Length + 1 
                && rawText.Substring(numOfChar, mark.Length).Equals(mark);
        }

        private bool IsValidEndEnvironment(string mark, string rawText, int numOfChar)
        {
            if (numOfChar + mark.Length + 1 < rawText.Length)
            {
                var externalChar = rawText[numOfChar + mark.Length];
                if (!IsAcceptedExternalChar(externalChar)) return false;
            }

            if (numOfChar != 0)
            {
                var internalChar = rawText[numOfChar - 1];
                if (!IsAcceptedInternalChar(internalChar)) return false;
            }
            
            return true;
        }

        private bool IsAcceptedExternalChar(char externalChar)
        {
            if (marks.Contains(externalChar.ToString())) return false;
            return char.IsWhiteSpace(externalChar) || char.IsPunctuation(externalChar);
        }

        private bool IsAcceptedInternalChar(char internalChar)
        {
            if (marks.Contains(internalChar.ToString())) return false;
            return !char.IsWhiteSpace(internalChar);
        }
    }
}
