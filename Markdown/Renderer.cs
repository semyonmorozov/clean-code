using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class Renderer
    {
        public readonly List<RenderingRule> RenderingRules;

        public Renderer(List<RenderingRule> renderingRules)
        {
            RenderingRules = renderingRules;
        }

        public string RenderWithPriority(RenderingRule rule, string rawText)
        {
            string renderedText;

            var selection = rule.GetSelection(rawText);
            var intersectedRules = RenderingRules.Where(r => IntersectedWithRenderedRule(selection, r)).ToList();
            if (intersectedRules.Any())
            {
                foreach (var r in intersectedRules)
                    rule.Selector.Start += r.OpeningTag.Length - 1;
                
                return DeleteMarks(rule, rawText);
            }

            if (IntersectedWithHigherPriorityRule(rule))
            {
                var oldEndOfSelection = rule.Selector.End;
                var oldStartOfSelection = rule.Selector.Start;
                renderedText = DeleteMarks(rule, rawText);
                MoveIntersectedStarts(rule, oldStartOfSelection, oldEndOfSelection);
                return renderedText;
            }

            renderedText = Render(rule, rawText);
            rule.Selector.ResetSelection();
            return renderedText;
        }

        private void MoveIntersectedStarts(RenderingRule rule,int oldStartOfSelection, int oldEndOfSelection)
        {
            var intersectedRules = RenderingRules.Where(r => r.Selector.Start < oldEndOfSelection)
                .Where(r => r.Selector.Start > oldStartOfSelection)
                .ToList();
            foreach (var r in intersectedRules)
                r.Selector.Start -= rule.Mark.Length;
        }

        private string Render(RenderingRule rule, string rawText)
        {
            var selection = rule.GetSelection(rawText);
            var tagedSelection = String.Concat(rule.OpeningTag, selection, rule.ClosingTag); 
            var renderedText = rawText.Replace(tagedSelection, rule.Selector.Start, rule.Selector.GetLength());
            return renderedText;
        }

        private string DeleteMarks(RenderingRule rule, string rawText)
        {
            var selection = rule.GetSelection(rawText);
            var renderedText = rawText.Replace(selection, rule.Selector.Start, rule.Selector.GetLength());
            return renderedText;
        }

        private bool IntersectedWithRenderedRule(string selection, RenderingRule rule)
        {
            return selection.Contains(rule.ClosingTag)
                   && !selection.Contains(rule.OpeningTag);
        }

        private bool IntersectedWithHigherPriorityRule(RenderingRule rule)
        {
            return RenderingRules.Where(r => r.Selector.Start != -1).Any(r => r.PriorityLevel < rule.PriorityLevel);
        }
    }

    static class Extension
    {
        public static string Replace(this string rawString, string newPart, int start, int lenght)
        {
            var builder = new StringBuilder(rawString);
            return builder.Remove(start, lenght).Insert(start, newPart).ToString();
        }
    }
}
