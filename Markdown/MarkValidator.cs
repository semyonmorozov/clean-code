using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    class MarkValidator
    {
        private readonly List<RenderingRule> renderingRules;
        private readonly List<String> marks;
        public MarkValidator(List<RenderingRule> renderingRules)
        {
            this.renderingRules = renderingRules;
            marks = renderingRules.Select(r => r.Mark).ToList();
        }


        public bool TrySetStartMark(string rawText, int numOfChar)
        {
            foreach (var rule in renderingRules)
            {
                if (!IsValidStartMark(rule.Mark, rawText, numOfChar)) return false;
                rule.StartOfSelection = numOfChar;
                return true;
            }
            return false;
        }

        public bool TrySetEndMark( string rawText, int numOfChar)
        {
            foreach (var rule in renderingRules)
            {
                if (rule.StartOfSelection == -1) return false;
                if (!IsValidEndMark(rule.Mark, rawText, numOfChar)) return false;
                rule.EndOfSelection = numOfChar + rule.Mark.Length;
                return true;
            }
            return false;
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
            if (!IsEndMark(mark, rawText, numOfChar)) return false;
            if (!IsValidEndEnvironment(mark, rawText, numOfChar)) return false;
            return true;
        }

        private bool IsEndMark(string mark, string rawText, int numOfChar)
        {
            if (rawText.Length < numOfChar + mark.Length + 1) return false;
            if (!rawText.Substring(numOfChar, mark.Length).Equals(mark)) return false;
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
