using Compiler.intermediate.cool.ast;

namespace Compiler.intermediate.cool
{
    public interface ICoolVisitor
    {
        void Visit(ProgramNode node); 
        void Visit(NegNode node);
        void Visit(AssignNode node);
        void Visit(ClassNode node);
        void Visit(FactorNode node);
        void Visit(MethodNode node);
        void Visit(AttrNode node);
        void Visit(FormalNode node);
        void Visit(IsVoidNode node);
        void Visit(NotNode node);
        void Visit(RalExprNode node);
        void Visit(SimpleExprNode node);
        void Visit(TermNode node);
        void Visit(IdNode node);
        void Visit(IntNode node);
        void Visit(StringNode node);
        void Visit(BoolNode node);
        void Visit(ParenExprNode node);
        void Visit(CallNode node);
        void Visit(IfNode node);
        void Visit(WhileNode node);
        void Visit(LetNode node);
        void Visit(PatternNode node);
        void Visit(NewObjNode node);
        void Visit(BlockNode node);
    }
}