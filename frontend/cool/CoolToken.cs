using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.frontend.cool
{
    public class CoolToken : Token
    {
        protected CoolToken(Source source) : base(source)
        {
        }
    }
}
