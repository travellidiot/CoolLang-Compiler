﻿using System;
using System.Collections.Generic;
using System.Text;
using Compiler.intermediate.cool.ast;

namespace Compiler.intermediate
{
    public class AstFactory
    {
        public static IAst CreateAst()
        {
            return new Ast();
        }

        //public static IAstNode CreateAstNode(string type)
        //{
        //    switch (type)
        //    {
        //        case "Program":
        //            return new ProgramNode(new List<IAstNode>());
        //        default:
        //            throw new Exception($"No such a type: {type}");
        //    }
        //}
    }
}