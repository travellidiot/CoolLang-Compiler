using System;
using System.Collections.Generic;
using Compiler.intermediate.cool.ast;

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
                    return new CoolProgramNode(new List<IAstNode>());
                default:
                    throw new Exception($"No such a type: {type}");
            }
        }
    }
}