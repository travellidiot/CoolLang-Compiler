using System.Text;

namespace Compiler.frontend.cool.tokens
{
    public class CoolWordToken : CoolToken
    {
        public CoolWordToken(Source source) : base(source)
        {
        }

        protected override void Extract()
        {
            var currentChar = CurrentChar();
            var firstChar = currentChar;
            var stringBuffer = new StringBuilder();

            while (char.IsLetterOrDigit(currentChar) || currentChar == '_')
            {
                stringBuffer.Append(currentChar);
                currentChar = NextChar();
            }

            Text = stringBuffer.ToString();

            if (char.IsUpper(firstChar))
            {
                Type = new CoolTokenType(TokenType.TypeId);
            }
            else
            {
                Type = CoolTokenType.KeyWords.ContainsKey(Text) ?
                    new CoolTokenType(CoolTokenType.KeyWords[Text])
                    : new CoolTokenType(TokenType.ObjectId);
            }
        }
    }
}