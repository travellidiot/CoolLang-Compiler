using System;
using Compiler.frontend.cool.tokens;
using Compiler.message;
using Compiler.message.cool;
using static Compiler.frontend.Source;

namespace Compiler.frontend.cool
{
    public class CoolScanner : Scanner
    {
        public CoolScanner(Source source, int k) : base(source, k)
        {
        }

        protected override frontend.Token ExtractToken()
        {
            SkipWhiteSpaces();

            char currentChar = CurrentChar();

            frontend.Token token;
            if (currentChar == Eof)
            {
                token = new EofToken(Source);
            }
            else if (char.IsLetter(currentChar))
            {
                // reserved words or objectId
                token = new WordToken(Source);
            }
            else if (char.IsDigit(currentChar))
            {
                token = new NumberToken(Source);
            }
            else if (currentChar == '\"')
            {
                token = new StringToken(Source);
            }
            // Some keys have more than one character, but the first character is also in the keys
            else if (CoolTokenType.SpecialWords.ContainsKey(currentChar.ToString().ToLower()))
            {
                token = new SpecialToken(Source);
            }
            else
            {
                token = new ErrorToken(Source, ErrorCode.InvalidCharacter, currentChar.ToString());
                NextChar();
            }

            return token;
        }

        private void SkipWhiteSpaces()
        {
            char currentChar = CurrentChar();
            while (char.IsWhiteSpace(currentChar))
            {
                currentChar = NextChar();
            }
        }

        public class NotCoolTokenException : Exception
        {
            private readonly frontend.Token _token;
            public NotCoolTokenException(frontend.Token token)
            {
                _token = token;
            }

            public override string ToString()
            {
                var text = _token.Text;
                var line = _token.LineNumber;
                var col = _token.Position;
                return $"Token is not an CoolToken \"{text}\": line {line}, col {col}";
            }
        }
    }
}
