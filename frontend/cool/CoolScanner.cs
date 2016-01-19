using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.frontend.cool.tokens;
using static Compiler.frontend.Source;

namespace Compiler.frontend.cool
{
    public class CoolScanner : Scanner
    {
        public CoolScanner(Source source) : base(source)
        {
        }

        protected override Token ExtractToken()
        {
            skipWhiteSpaces();

            char currentChar = CurrentChar();

            Token token;
            if (currentChar == Eof)
            {
                token = new EofToken(Source);
            }
            else if (char.IsLetter(currentChar))
            {
                // reserved words or objectId
                token = new CoolWordToken(Source);
            }
            else if (char.IsDigit(currentChar))
            {
                token = new CoolNumberToken(Source);
            }
            else if (currentChar == '\"')
            {
                token = new CoolStringToken(Source);
            }
            else if (CoolTokenType.SpecialWords.ContainsKey(currentChar.ToString().ToLower()))
            {
                token = new CoolSpecialToken(Source);
            }
            else
            {
                token = new CoolErrorToken(Source);
                NextChar();
            }

            return token;
        }

        private void skipWhiteSpaces()
        {
            char currentChar = CurrentChar();
            while (char.IsWhiteSpace(currentChar))
            {
                currentChar = NextChar();
            }
        }
    }
}
