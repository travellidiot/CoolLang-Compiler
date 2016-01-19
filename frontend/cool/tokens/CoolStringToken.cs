using System;
using System.Text;

using static Compiler.frontend.Source;

namespace Compiler.frontend.cool.tokens
{
    internal class CoolStringToken : CoolToken
    {
        public CoolStringToken(Source source) : base(source)
        {
        }

        protected new void Extract()
        {
            StringBuilder textBuffer = new StringBuilder();
            StringBuilder valueBuffer = new StringBuilder();

            textBuffer.Append('\"');
            char currentChar = NextChar();

            while (currentChar != '\"')
            {
                if (currentChar == Eol || currentChar == Eof)
                {
                    Type = new CoolTokenType(TokenType.Error);
                    Text = "EOL or EOF appears in string: " + textBuffer.ToString();
                    Value = null;
                    return;
                }
                
                if (currentChar == '\\')
                {
                    textBuffer.Append('\\');
                    currentChar = NextChar();
                    switch (currentChar)
                    {
                        case 't':
                            currentChar = '\t'; break;
                        case 'b':
                            currentChar = '\b'; break;
                        case 'n':
                            currentChar = '\n'; break;
                        case 'f':
                            currentChar = '\f'; break;
                        case '\n':
                            currentChar = NextChar();
                            textBuffer.Append('\n');
                            break;
                    }
                }

                textBuffer.Append(currentChar);
                valueBuffer.Append(currentChar);

                currentChar = NextChar();
            }
        }
    }
}