using Compiler.message;
using Compiler.message.cool;

namespace Compiler.frontend.cool.tokens
{
    internal class ErrorToken : Token
    {
        public ErrorToken(Source source, ErrorCode errorCode, string tokenText) : base(source)
        {
            Text = tokenText;
            Type = new CoolTokenType(TokenType.Error);
            Value = errorCode;
        }

        protected override void Extract()
        {
            
        }
    }
}