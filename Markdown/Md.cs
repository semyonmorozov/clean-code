using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	class Md
	{
        private readonly MarkValidator markValidator;
        private readonly Renderer renderer;
        
        public Md(List<RenderingRule> renderingRules)
	    {
            markValidator = new MarkValidator(renderingRules.Select(r => r.Mark).ToList());
            renderer = new Renderer(renderingRules);
	    }

	    public string Render(string markdown)
		{
            for (var numOfChar=0; numOfChar < markdown.Length; numOfChar++)
            {
                foreach (var rule in renderer.RenderingRules)
                {
                    if (rule.TrySetStartMark(markValidator, markdown, numOfChar)) break;
                    
                    if (rule.TrySetEndMark(markValidator, markdown, numOfChar))
                    {
                        markdown = renderer.RenderWithPriority(rule, markdown);
                        break;
                    }
                }
            }
            return markdown;
		}
    }
}