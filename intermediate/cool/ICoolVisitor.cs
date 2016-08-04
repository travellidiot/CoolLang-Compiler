using Compiler.intermediate.cool.ast;

namespace Compiler.intermediate.cool
{
    public interface ICoolVisitor
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
        void Visit(CoolIdNode node);
        void Visit(CoolIntNode node);
        void Visit(CoolStringNode node);
        void Visit(CoolBoolNode node);
        void Visit(CoolParenExprNode node);
        void Visit(CoolCallNode node);
        void Visit(CoolIfNode node);
        void Visit(CoolWhileNode node);
        void Visit(CoolLetNode node);
        void Visit(CoolPatternNode node);
        void Visit(CoolNewObjNode node);
        void Visit(CoolBlockNode node);
    }
}