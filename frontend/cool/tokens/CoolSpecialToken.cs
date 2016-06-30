using System.Text;

namespace Compiler.frontend.cool.tokens
{
    public class CoolSpecialToken : CoolToken
    {
        public CoolSpecialToken(Source source) : base(source)
        {
        }

        protected override void Extract()
        {
            char currentChar = CurrentChar();
            char nextChar;
            #region process special symbols
            switch (currentChar)
            {
                case '/':
                    Type = new CoolTokenType(TokenType.Slash); goto default;
                case '+':
                    Type = new CoolTokenType(TokenType.Add); goto default;
                case '-':
                    nextChar = NextChar();
                    if (nextChar != '-')
                    {
                        Type = new CoolTokenType(TokenType.Minus);
                        break;
                    }
                    while (nextChar != Source.Eol) { nextChar = NextChar(); }
                    Type = new CoolTokenType(TokenType.Comment);
                    goto default;
                case ')':
                    Type = new CoolTokenType(TokenType.RightParen); goto default;
                case '.':
                    Type = new CoolTokenType(TokenType.Dispatch); goto default;
                case '~':
                    Type = new CoolTokenType(TokenType.Anti); goto default;
                case ',':
                    Type = new CoolTokenType(TokenType.Comma); goto default;
                case ';':
                    Type = new CoolTokenType(TokenType.Semic); goto default;
                case ':':
                    Type = new CoolTokenType(TokenType.Colon); goto default;
                case '@':
                    Type = new CoolTokenType(TokenType.At); goto default;
                case '{':
                    Type = new CoolTokenType(TokenType.LeftBracket); goto default;
                case '}':
                    Type = new CoolTokenType(TokenType.RightBracket); goto default;
                case '=':
                    nextChar = NextChar();
                    if (nextChar == '>') {
                        Type = new CoolTokenType(TokenType.DArraw);
                        goto default;
                    }
                    Type = new CoolTokenType(TokenType.Equal);
                    break;
                case '<':
                    nextChar = NextChar();
                    if (nextChar == '-') {
                        Type = new CoolTokenType(TokenType.Assign);
                        goto default;
                    }
                    if (nextChar == '=') {
                        Type = new CoolTokenType(TokenType.LessEqual);
                        goto default;
                    }
                    Type = new CoolTokenType(TokenType.LessThan);
                    break;
                case '>':
                    nextChar = NextChar();
                    if (nextChar == '=') {
                        Type = new CoolTokenType(TokenType.GreatEqual);
                        goto default;
                    }
                    Type = new CoolTokenType(TokenType.GreatThan);
                    break;
                case '*':
                    Type = new CoolTokenType(TokenType.Star);
                    goto default;
                case '(':
                    nextChar = NextChar();
                    if (nextChar != '*') {
                        Type = new CoolTokenType(TokenType.LeftParen);
                        break;
                    }
                    nextChar = '\0';
                    do
                    {
                        var prev = nextChar;
                        nextChar = NextChar();
                        
                        if (nextChar == Source.Eof || nextChar == '\0')
                        {
                            Type = new CoolTokenType(TokenType.Error);
                            Value = "EOF during comments";
                            break;
                        }
                        if (prev == '*' && nextChar == ')')
                        {
                            Type = new CoolTokenType(TokenType.Comment);
                            break;
                        }
                    } while (true);
                    goto default;
                default:
                    NextChar(); break;
            }
            #endregion

            Text = "";
            Value = null;
        }
    }
}