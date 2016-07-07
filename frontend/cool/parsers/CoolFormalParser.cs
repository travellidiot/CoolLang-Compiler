using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolFormalParser : CoolTdParser
    {
        public CoolFormalParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolFormalParser(CoolTdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var token = Synchronize(new SortedSet<ITokenType>() {CoolTokenType.ObjectId});
            NextToken(); // eat ID
            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Colon});
            NextToken(); // eat ":"
            var typeToken = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.TypeId});

            NextToken(); // eat "TYPE"
            return new CoolFormalNode(token as CoolWordToken, typeToken as CoolWordToken);
        }
    }
}