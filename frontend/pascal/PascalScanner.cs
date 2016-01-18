using static Compiler.frontend.Source;

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

            var token = currentChar == Eof ? new EofToken(Source) : new Token(Source);
            
            return token;
        }
    }
}