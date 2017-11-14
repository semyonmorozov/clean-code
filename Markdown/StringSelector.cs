
namespace Markdown
{
    class StringSelector
    {
        public int Start { get; set; }
        public int End { get; set; }

        public StringSelector()
        {
            ResetSelection();
        }

        public void ResetSelection()
        {
            Start = -1;
            End = -1;
        }

        public int GetLength()
        {
            return End - Start;
        }
    }
}
