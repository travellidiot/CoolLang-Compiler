using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.intermediate.symtabimpl
{
    public class SymTabKeyImpl : ISymTabKey
    {
        public enum Keys
        {
            ConstantValue,

            RoutineCode,
            RoutineSymTab,
            RoutineICode,
            RoutineParms,
            RoutineRoutines,

            DataValue
        }
        public Keys Key { get; private set; }

        public SymTabKeyImpl(SymTabKeyImpl.Keys key)
        {
            this.Key = key;
        }
    }
}
