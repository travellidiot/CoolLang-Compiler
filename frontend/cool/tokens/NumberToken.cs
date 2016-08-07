using System.Text;

namespace Compiler.frontend.cool.tokens
{
    public class NumberToken : Token
    {
        public NumberToken(Source source) : base(source)
        {
        }

        protected override void Extract()
        {
            StringBuilder stringBuffer = new StringBuilder();
            char currentChar = CurrentChar();
            while (char.IsDigit(currentChar))
            {
                stringBuffer.Append(currentChar);
                currentChar = NextChar();
            }

            Text = stringBuffer.ToString();
            Value = int.Parse(Text);
            Type = new CoolTokenType(TokenType.IntConst);
        }
    }
}