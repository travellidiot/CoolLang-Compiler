using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolFormalParser : CoolTDParser
    {
        public CoolFormalParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolFormalParser(CoolTDParser parent) : base(parent)
        {
        }

        public override CoolAstNode Parse()
        {
            return base.Parse();
        }
    }
}