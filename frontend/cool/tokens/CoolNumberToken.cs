using System.Text;

namespace Compiler.frontend.cool.tokens
{
    internal class CoolNumberToken : CoolToken
    {
        public CoolNumberToken(Source source) : base(source)
        {
        }

        protected new void Extract()
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
        }
    }
}