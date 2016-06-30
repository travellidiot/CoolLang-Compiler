using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
