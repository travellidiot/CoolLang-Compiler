namespace Compiler.frontend
{
    public class Token
    {
        public ITokenType Type { get; protected set; }
        public string Text { get; protected set; }
        public object Value { get; protected set; }
        public int LineNumber { get; protected set; }
        public int Position { get; protected set; }
        protected Source Source;


        public Token(Source source) 
        {
            Source = source;
            LineNumber = source.LineNum;
            Position = source.CurrentPos;

            Extract();
        }

        protected virtual void Extract()
        {
            Text = CurrentChar().ToString();
            Value = null;
            NextChar();
        }

        protected char CurrentChar()
        {
            return Source.CurrentChar();
        }

        protected char NextChar()
        {
            return Source.NextChar();
        }

        protected char PeekChar()
        {
            return Source.PeekChar();
        }
    }
}