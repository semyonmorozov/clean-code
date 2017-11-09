using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Renderer
    {
        public readonly char EscapeChar;
        public readonly string Mark;
        public readonly string Tag;
        public int StartOfSelection { get; protected set; }
        public int EndOfSelection { get; protected set; }

        public Renderer(string mark,string tag)
        {
            EscapeChar = '\\';
            Mark = mark;
            Tag = tag;
        }

        public string Render(string rawText)
        {
            return rawText;
        }

        public bool TrySetStartOfSelection(string rawText, int numOfChar)
        {
            StartOfSelection = numOfChar;
            throw new NotImplementedException();
        }

        public bool TrySetEndOfSelection(string rawText, int numOfChar)
        {
            EndOfSelection = numOfChar;
            throw new NotImplementedException();
        }
    }
}
