namespace Compiler.frontend.cool.tokens
{
    internal class CoolErrorToken : CoolToken
    {
        public CoolErrorToken(Source source, CoolErrorCode errorCode, string tokenText) : base(source)
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