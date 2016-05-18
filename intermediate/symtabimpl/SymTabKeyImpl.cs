using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.intermediate.symtabimpl
{
    public class SymTabKeyImpl : ISymTabKey
    {
        public enum SymKey
        {
            ConstantValue,

            RoutineCode,
            RoutineSymTab,
            RoutineICode,
            RoutineParms,
            RoutineRoutines,

            DataValue
        }
        public SymKey Key { get; private set; }

        public SymTabKeyImpl(SymTabKeyImpl.SymKey symKey)
        {
            this.Key = symKey;
        }
    }
}
