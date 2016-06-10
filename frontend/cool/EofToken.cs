using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.frontend.cool
{
    public class EofToken : CoolToken
    {
        public EofToken(Source source) : base(source)
        {
        }

        protected void Extract(Source source)
        {
        }
    }
}
