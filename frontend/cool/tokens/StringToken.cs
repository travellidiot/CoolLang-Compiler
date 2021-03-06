﻿using System.Text;

using static Compiler.frontend.Source;

namespace Compiler.frontend.cool.tokens
{
    public class StringToken : Token
    {
        public StringToken(Source source) : base(source)
        {
        }

        protected override void Extract()
        {
            var textBuffer = new StringBuilder();
            var valueBuffer = new StringBuilder();

            textBuffer.Append('\"');
            var currentChar = NextChar();

            while (currentChar != '\"')
            {
                if (currentChar == Eol || currentChar == Eof)
                {
                    Type = new CoolTokenType(TokenType.Error);
                    Text = "EOL or EOF appears in string: " + textBuffer;
                    Value = null;
                    return;
                }
                
                if (currentChar == '\\')
                {
                    textBuffer.Append('\\');
                    var prevChar = currentChar;
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