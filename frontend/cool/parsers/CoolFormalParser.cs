using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
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

        public override IAstNode Parse()
        {
            var token = Synchronize(new SortedSet<TokenType>() {TokenType.ObjectId});
            NextToken();
            Synchronize(new SortedSet<TokenType>() {TokenType.Colon});
            NextToken();
            var typeToken = Synchronize(new SortedSet<TokenType>() {TokenType.TypeId});

            NextToken();
            return new CoolFormalNode(token as CoolWordToken, typeToken as CoolWordToken);
        }
    }
}