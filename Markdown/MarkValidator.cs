using System;
using System.Collections.Generic;

namespace Markdown
{
    class MarkValidator
    {
        private readonly List<String> marks;
        public MarkValidator(List<String> marks)
        {
            this.marks = marks;
        }

        public bool IsValidStartMark(string mark, string rawText, int numOfChar)
        {
            if (!IsStartMark(mark, rawText, numOfChar)) return false;
            if (!IsValidStartEnvironment(mark, rawText, numOfChar)) return false;
            return true;
        }

        private bool IsStartMark(string mark, string rawText, int numOfChar)
        {
            if (rawText.Length < numOfChar + mark.Length + 1) return false;
            if (!rawText.Substring(numOfChar, mark.Length).Equals(mark)) return false;
            return true;
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
            if (rawText.Length < numOfChar + mark.Length + 1) return false;
            if (!rawText.Substring(numOfChar, mark.Length).Equals(mark)) return false;
            if (!IsValidEndEnvironment(mark, rawText, numOfChar)) return false;
            return true;
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

        private bool IsAcceptedExternalChar(char c)
        {
            if (marks.Contains(c.ToString())) return false;
            if (!Char.IsWhiteSpace(c))
                if (!Char.IsPunctuation(c))
                    return false;
            return true;
        }

        private bool IsAcceptedInternalChar(char c)
        {
            if (marks.Contains(c.ToString())) return false;
            if (Char.IsWhiteSpace(c)) return false;
            return true;
        }
    }
}
