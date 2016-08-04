using System.Diagnostics;
using Compiler.intermediate.cool.ast;

namespace Compiler.intermediate.cool.symtab
{
    internal class TypeGather
    {
        internal TypeGather() { }

        internal void Gather(CoolProgramNode node, IScope pScope)
        {
            foreach (var cls in node.Classes)
            {
                var clsScope = new ClassSymbol(cls.ClassName.Text, pScope);
                pScope.Enter(cls.ClassName.Text, clsScope);
            }
        }
    }
}