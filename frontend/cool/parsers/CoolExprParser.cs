using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolExprParser : CoolTDParser
    {
        public CoolExprParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolExprParser(CoolTDParser parent) : base(parent)
        {
        }

        public override CoolAstNode Parse()
        {
            throw new System.NotImplementedException();
        }
    }
}