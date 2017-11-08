using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
		public string RenderToHtml(string markdown)
		{
            for (var i=0;i<markdown.Length;i++)
            {
                
            }

            return markdown;
		}

    }

	[TestFixture]
	public class Md_Should
	{
	    private Md md;

        [SetUp]
	    public void Init()
	    {
	        md = new Md();
	    }

        [Test]
	    public void RenderEmphasizedText()
	    {
	        var inputStr = "Текст _окруженный с двух сторон_  одинарными символами подчерка";
	        var expectedStr = "Текст <em>окруженный с двух сторон</em>  одинарными символами подчерка";
	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
	    }

	    [TestCase(@"\_Вот это\_, не должно выделиться тегом <em>")]
	    [TestCase(@"\_\_А это\_\_, не должно выделиться тегом <strong>")]
        public void Render_ShieldedMark_AsCharacter(string inputStr)
	    {
	        md.RenderToHtml(inputStr).Should().Be(inputStr);
	    }

        [Test]
	    public void RenderImportantText()
	    {
	        var inputStr = "__Двумя символами__ — должен становиться жирным";
	        var expectedStr = "<strong>Двумя символами</strong> — должен становиться жирным";

	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
        }

	    [Test]
	    public void Render_EmphasizedText_Inside_ImportantText()
	    {
	        var inputStr = "Внутри __двойного выделения _одинарное_ тоже__ работает.";
	        var expectedStr = "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.";
	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
	    }

	    [Test]
	    public void NotRender_ImportantText_Inside_EmphasizedText()
	    {
	        var inputStr = "Но не наоборот — внутри _одинарного __двойное__ не работает_.";
	        var expectedStr = "Но не наоборот — внутри <em>одинарного двойное не работает</em>.";
	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
	    }

	    [Test]
	    public void NotRenderMakr_InsideTextWithNums()
	    {
	        var inputStr = "Подчерки внутри текста c цифрами_12_3 не считаются выделением";
	        md.RenderToHtml(inputStr).Should().Be(inputStr);
	    }

        [Test]
	    public void NotRender_UnpairedMakr()
	    {
	        var inputStr = "__непарные _символы не считаются выделением";
	        var expectedStr = "__непарные _символы не считаются выделением";
	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
        }
        
	    [TestCase("Это_ простая_ строка для тестирования.")]
	    [TestCase("Это__ простая__ строка для тестирования.")]
        public void NotRender_FinalMark_WithoutWhitespaceAfterIt(string inputStr)
	    {
	        md.RenderToHtml(inputStr).Should().Be(inputStr);
        }
	    
	    [TestCase("Это _простая _строка для тестирования.")]
	    [TestCase("Это __простая __строка для тестирования.")]
	    public void NotRender_InitialMark_WithoutWhitespaceBeforeIt(string inputStr)
	    {
	        md.RenderToHtml(inputStr).Should().Be(inputStr);
        }
	    
	    [TestCase("Это _простая_ строка_ для тестирования.", "Это <em>простая</em> строка_ для тестирования.")]
	    [TestCase("Это __простая__ строка__ для тестирования.", "Это <strong>простая</strong> строка__ для тестирования.")]
        public void Consider_LeftmostFinalMark_AsEndOfSelection(string inputStr,string expectedStr)
	    {
	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
        }
	    
	    [TestCase("Это _простая _строка_, для тестирования.", "Это _простая <em>строка</em>, для тестирования.")]
	    [TestCase("Это __простая __строка__ для тестирования.", "Это __простая <strong>строка</strong> для тестирования.")]
	    public void Consider_RightmostInitialMark_AsStartOfSelection(string inputStr, string expectedStr)
	    {
	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
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
        public void Render_Punctuation_SimilarToSpaces(string inputStr, string expectedStr)
	    {
	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
        }

	    [TestCase("Это __простая _строка__ для_ тестирования.", "Это простая <em>строка для</em> тестирования.")]
	    [TestCase("Это _простая __строка_ для__ тестирования.", "Это <em>простая строка</em> для тестирования.")]
        public void Render_OnlyEmphasizedText_WhenItIntersects_WithImportantText(string inputStr, string expectedStr)//
	    {
	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
        }
        
    }
}