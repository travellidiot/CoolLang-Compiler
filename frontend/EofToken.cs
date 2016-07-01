namespace Compiler.frontend
{
    public class EofToken : Token
    {
        public EofToken(Source source) : base(source)
        {
        }

        protected void Extract(Source source)
        {
        }
    }
}
