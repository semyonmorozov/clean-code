using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	public class Md
	{
		public string RenderToHtml(string markdown)
		{
		    
		    markdown = RenderMarkToTag(markdown,"__","strong");
		    markdown = RenderMarkToTag(markdown, "_", "em");

            return markdown;
		}

	    private string RenderMarkToTag(string markdown, string mark, string tagName)
	    {
	        string result;
	        if (markdown.Contains(mark))
	        {
	            int i = markdown.IndexOf(mark);
	            result = markdown.Remove(i, mark.Length).Insert(i, "<"+ tagName + ">");
	            if (result.Contains(mark))
	            {
	                i = result.IndexOf(mark);
	                result = result.Remove(i, mark.Length).Insert(i, "</"+ tagName + ">");
	                return result;
	            }
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
	        string inputStr = "Текст _окруженный с двух сторон_  одинарными символами подчерка";
	        string expectedStr = "Текст <em>окруженный с двух сторон</em>  одинарными символами подчерка";

	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
	    }

	    [Test]
	    public void RenderImportantText()
	    {
	        string inputStr = "__Двумя символами__ — должен становиться жирным";
	        string expectedStr = "<strong>Двумя символами</strong> — должен становиться жирным";

	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
        }

	    [Test]
	    public void NotRenderUnpairedMakr()
	    {
	        string inputStr = "__непарные _символы не считаются выделением";
	        string expectedStr = "__непарные _символы не считаются выделением";

	        md.RenderToHtml(inputStr).Should().Be(expectedStr);
        }

	    
    }
}