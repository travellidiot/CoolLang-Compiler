using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;

namespace Compiler.frontend.cool.parsers
{
    public class FormalParser : TdParser
    {
        public FormalParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public FormalParser(TdParser parent) : base(parent)
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
            return new FormalNode(token as WordToken, typeToken as WordToken);
        }
    }
}