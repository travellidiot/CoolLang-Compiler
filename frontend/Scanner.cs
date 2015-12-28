namespace Compiler.frontend
{
    public abstract class Scanner
    {
        protected Source Source;
        public Token CurrentToken { get; private set; }

        public Scanner(Source source)
        {
            Source = source;
        }

        public Token NetToken()
        {
            CurrentToken = ExtractToken();
            return CurrentToken;
        }

        protected abstract Token ExtractToken();

        public char CurrentChar()
        {
            return Source.CurrentChar();
        }

        public char NextChar()
        {
            return Source.NextChar();
        }
    }
}