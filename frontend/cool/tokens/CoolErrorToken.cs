namespace Compiler.frontend.cool.tokens
{
    internal class CoolErrorToken : CoolToken
    {
        public CoolErrorToken(Source source, CoolErrorCode errorCode, string tokenText) : base(source)
        {
            this.Text = tokenText;
            this.Type = new CoolTokenType(TokenType.Error);
            this.Value = errorCode;
        }

        protected override void Extract()
        {
            
        }
    }
}