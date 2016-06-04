using System;
using Compiler.intermediate.coolast;
using System.Text;

namespace Compiler.intermediate
{
    public class AstFactory
    {
        public static IAst CreateAst()
        {
            return new CoolAst();
        }

        public static IAstNode CreateAstNode(string type)
        {
            switch (type)
            {
                case "Program":
                    return new CoolProgramNode();
                default:
                    throw new Exception($"No such a type: {type}");
            }
        }
    }
}