using System.Text;

using static Compiler.frontend.Source;

namespace Compiler.frontend.cool.tokens
{
    public class CoolStringToken : CoolToken
    {
        public CoolStringToken(Source source) : base(source)
        {
        }

        protected override void Extract()
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
                    char prevChar = currentChar;
                    currentChar = NextChar();
                    switch (currentChar)
                    {
                        case 't':
                            valueBuffer.Append('\t');
                            break;
                        case 'b':
                            valueBuffer.Append('\b');
                            break;
                        case 'n':
                            valueBuffer.Append('\n');
                            break;
                        case 'f':
                            valueBuffer.Append('\f');
                            break;
                        case '\n':
                            break;
                    }

                    textBuffer.Append(currentChar);
                }

                currentChar = NextChar();
            }

            textBuffer.Append('\"');
            NextChar();
            Type = new CoolTokenType(TokenType.StringConst);
            Text = textBuffer.ToString();
            Value = valueBuffer.ToString();
        }
    }
}