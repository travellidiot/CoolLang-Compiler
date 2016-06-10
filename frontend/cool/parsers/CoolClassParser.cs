using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolClassParser : CoolTDParser
    {
        public CoolClassParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolClassParser(CoolTDParser parent) : base(parent)
        {
        }

        // class TYPE [inherits TYPE] { 【feature;】* }
        public override IAstNode Parse()
        {
            Synchronize(new SortedSet<TokenType>() {TokenType.Class});
            var typeSet = new SortedSet<TokenType>() {TokenType.TypeId};

            NextToken();
            var clsName = Synchronize(typeSet);
            CoolToken prtName = null;

            NextToken();
            var token = Synchronize(new SortedSet<TokenType>() {TokenType.Inherits, TokenType.LeftBracket});
            if (token.Type.Is(TokenType.Inherits))
            {
                NextToken();
                prtName = Synchronize(typeSet);
                NextToken();
                token = Synchronize(new SortedSet<TokenType>() {TokenType.LeftBracket});
            }

            var features = new List<IAstNode>();
            NextToken();
            while (!CurrentToken().Type.Is(TokenType.RightBracket))
            {
                var parser = new CoolFeatureParser(this);
                var feature = parser.Parse();
                features.Add(feature);

                Synchronize(new SortedSet<TokenType>() {TokenType.Semic});
                NextToken();
            }

            NextToken();
            return new CoolClassNode(clsName as CoolWordToken, prtName as CoolWordToken, features);
        }
    }
}