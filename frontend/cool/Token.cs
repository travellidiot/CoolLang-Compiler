namespace Compiler.frontend.cool
{
    public class Token : frontend.Token
    {
        protected Token(Source source) : base(source)
        {
        }

        public new CoolTokenType Type
        {
            get { return base.Type as CoolTokenType; }
            set { base.Type = value; }
        }
    }
}
