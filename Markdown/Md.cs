﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
	    private readonly List<RenderingRule> renderingRules;
        private readonly MarkValidator markValidator;

        public static List<RenderingRule> HtmlRules = new List<RenderingRule>
        {
            new RenderingRule("_","<em>","</em>",1),
            new RenderingRule("__","<strong>","</strong>",2)
        };

	    public static List<RenderingRule> CreoleRules = new List<RenderingRule>
	    {
	        new RenderingRule("_","//","//",1),
	        new RenderingRule("__","**","**",1)
	    };

        public Md(List<RenderingRule> renderingRules)
	    {
	        this.renderingRules = renderingRules;
	        var marks = renderingRules.Select(r => r.Mark).ToList();
            markValidator = new MarkValidator(marks);
	    }

	    public string Render(string markdown)
		{
            for (var numOfChar=0;numOfChar<markdown.Length;numOfChar++)
            {
                foreach (var rule in renderingRules)
                {
                    if (TrySetStartMark(rule, markdown, numOfChar))
                    {
                        numOfChar += rule.Mark.Length-1;
                        break;
                    }
                    if (TrySetEndMark(rule, markdown, numOfChar))
                    {
                        var partiallyRenderedText = rule.Render(markdown);
                        numOfChar += partiallyRenderedText.Length-markdown.Length-1;
                        markdown = partiallyRenderedText;
                        break;
                    }
                }
            }

            return markdown;
		}

        private bool TrySetStartMark(RenderingRule rule, string rawText, int numOfChar)
        {
            if (!markValidator.IsValidStartMark(rule.Mark, rawText, numOfChar)) return false;
	        rule.StartOfSelection = numOfChar;
	        return true;
	    }

        private bool TrySetEndMark(RenderingRule rule, string rawText, int numOfChar)
	    {
	        if (rule.StartOfSelection == -1) return false;
            if (!markValidator.IsValidEndMark(rule.Mark, rawText, numOfChar)) return false;
            rule.EndOfSelection = numOfChar + rule.Mark.Length;
            return true;
	    }
    }

    [TestFixture]
	public class Md_Should
	{
	    private Md md;

        [SetUp]
	    public void Init()
	    {
	        md = new Md(Md.HtmlRules);
	    }

        [Test]
	    public void RenderEmphasizedText()
	    {
	        var rawText = "Текст _окруженный с двух сторон_  одинарными символами подчерка";
	        var expectedStr = "Текст <em>окруженный с двух сторон</em>  одинарными символами подчерка";
	        md.Render(rawText).Should().Be(expectedStr);
	    }

	    [TestCase(@"\_Вот это\_, не должно выделиться тегом <em>")]
	    [TestCase(@"\_\_А это\_\_, не должно выделиться тегом <strong>")]
        public void Render_EscapedMark_AsCharacter(string rawText)
	    {
	        md.Render(rawText).Should().Be(rawText);
	    }

        [Test]
	    public void RenderImportantText()
	    {
	        var rawText = "__Двумя символами__ — должен становиться жирным";
	        var expectedStr = "<strong>Двумя символами</strong> — должен становиться жирным";

	        md.Render(rawText).Should().Be(expectedStr);
        }

	    [Test]
	    public void Render_EmphasizedText_Inside_ImportantText()
	    {
	        var rawText = "Внутри __двойного выделения _одинарное_ тоже__ работает.";
	        var expectedStr = "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.";
	        md.Render(rawText).Should().Be(expectedStr);
	    }

	    [Test]
	    public void NotRender_ImportantText_Inside_EmphasizedText()
	    {
	        var rawText = "Но не наоборот — внутри _одинарного __двойное__ не работает_.";
	        var expectedStr = "Но не наоборот — внутри <em>одинарного двойное не работает</em>.";
	        md.Render(rawText).Should().Be(expectedStr);
	    }

	    [Test]
	    public void NotRenderMakr_InsideTextWithNums()
	    {
	        var rawText = "Подчерки внутри текста c цифрами_12_3 не считаются выделением";
	        md.Render(rawText).Should().Be(rawText);
	    }

        [Test]
	    public void NotRender_UnpairedMark()
	    {
	        var rawText = "__непарные _символы не считаются выделением";
	        var expectedStr = "__непарные _символы не считаются выделением";
	        md.Render(rawText).Should().Be(expectedStr);
        }
        
	    [TestCase("Это_ простая_ строка для тестирования.")]
	    [TestCase("Это__ простая__ строка для тестирования.")]
        public void NotRender_FinalMark_WithoutWhitespaceAfterIt(string rawText)
	    {
	        md.Render(rawText).Should().Be(rawText);
        }
	    
	    [TestCase("Это _простая _строка для тестирования.")]
	    [TestCase("Это __простая __строка для тестирования.")]
	    public void NotRender_InitialMark_WithoutWhitespaceBeforeIt(string rawText)
	    {
	        md.Render(rawText).Should().Be(rawText);
        }
	    
	    [TestCase("Это _простая_ строка_ для тестирования.", "Это <em>простая</em> строка_ для тестирования.")]
	    [TestCase("Это __простая__ строка__ для тестирования.", "Это <strong>простая</strong> строка__ для тестирования.")]
        public void Consider_LeftmostFinalMark_AsEndOfSelection(string rawText,string expectedStr)
	    {
	        md.Render(rawText).Should().Be(expectedStr);
        }
	    
	    [TestCase("Это _простая _строка_, для тестирования.", "Это _простая <em>строка</em>, для тестирования.")]
	    [TestCase("Это __простая __строка__ для тестирования.", "Это __простая <strong>строка</strong> для тестирования.")]
	    public void Consider_RightmostInitialMark_AsStartOfSelection(string rawText, string expectedStr)
	    {
	        md.Render(rawText).Should().Be(expectedStr);
	    }

	    [TestCase("__запятая__,", "<strong>запятая</strong>,")]
	    [TestCase("_двоеточие_:", "<em>двоеточие</em>:")]
	    [TestCase("__точка с запятой__;", "<strong>точка с запятой</strong>;")]
	    [TestCase("(_обычная скобка_)", "(<em>обычная скобка</em>)")]
	    [TestCase("{__фигурная скобка__}", "{<strong>фигурная скобка</strong>}")]
	    [TestCase("\"_кавычки_\"", "\"<em>кавычки</em>\"")]
	    [TestCase("__восклицание__!", "<strong>восклицание</strong>!")]
	    [TestCase("_знак вопроса_?", "<em>знак вопроса</em>?")]
	    [TestCase("__точка__.", "<strong>точка</strong>.")]
	    [TestCase(@"\__escape-символом_\_", @"\_<em>escape-символом</em>\_")]
        public void Render_Punctuation_SimilarToSpaces(string rawText, string expectedStr)
	    {
	        md.Render(rawText).Should().Be(expectedStr);
        }

	    [TestCase("Это __простая _строка__ для_ тестирования.", "Это простая <em>строка для</em> тестирования.")]
	    [TestCase("Это _простая __строка_ для__ тестирования.", "Это <em>простая строка</em> для тестирования.")]
        public void Render_OnlyEmphasizedText_WhenItIntersects_WithImportantText(string rawText, string expectedStr)
	    {
	        md.Render(rawText).Should().Be(expectedStr);
        }
        
    }
}