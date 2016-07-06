﻿using Compiler.frontend.cool.tokens;
using Compiler.intermediate.coolsymtab;

namespace Compiler.intermediate.coolast
{
    public class CoolSimpleExprNode : CoolAstNode
    {
        // CoolSimpleExprNode
        public IAstNode SimpleExpr { get; }
        public CoolSpecialToken Op { get; }
        // CoolFactorNode
        public IAstNode Factor { get; }

        public CoolSimpleExprNode(IAstNode simpleExpr, CoolSpecialToken op, IAstNode factor)
        {
            SimpleExpr = simpleExpr;
            SimpleExpr.ParentNode = this;
            Op = op;
            Factor = factor;
            if (Factor != null)
                Factor.ParentNode = this;
        }

        public override IAstNode Copy()
        {
            return new CoolSimpleExprNode(SimpleExpr, Op, Factor);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}