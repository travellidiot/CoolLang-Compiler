﻿using Compiler.intermediate.coolast;

namespace Compiler.intermediate.coolsymtab
{
    public interface IVisitor
    {
        void Visit(CoolProgramNode node);
        void Visit(CoolAntiNode node);
        void Visit(CoolAssignNode node);
        void Visit(CoolClassNode node);
        void Visit(CoolFactorNode node);
        void Visit(CoolMethodNode node);
        void Visit(CoolAttrNode node);
        void Visit(CoolFormalNode node);
        void Visit(CoolIsVoidNode node);
        void Visit(CoolNotNode node);
        void Visit(CoolRalExprNode node);
        void Visit(CoolSimpleExprNode node);
        void Visit(CoolTermNode node);
        void Visit(CoolValueNode node);
    }
}