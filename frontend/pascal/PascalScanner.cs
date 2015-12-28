namespace Compiler.frontend.pascal
{
    public class PascalScanner : Scanner
    {
        public PascalScanner(Source source) : base(source)
        {
        }

        protected override Token ExtractToken()
        {
            var currentChar = CurrentChar();

            var token = currentChar == Source.Eof ? new EofToken(Source) : new Token(Source);

            return token;
        }
    }
}