using System.Collections.Generic;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolClassParser : CoolTdParser
    {
        public CoolClassParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolClassParser(CoolTdParser parent) : base(parent)
        {
        }

        // class TYPE [inherits TYPE] { 【feature;】* }
        public override IAstNode Parse()
        {
            Synchronize(new SortedSet<ITokenType>() {CoolTokenType.Class});
            var typeSet = new SortedSet<ITokenType>() { CoolTokenType.TypeId};

            NextToken(); // eat "class"
            var clsName = Synchronize(typeSet);
            CoolToken prtName = null;

            NextToken(); // eat "TYPE", the class name
            var token = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Inherits, CoolTokenType.LeftBracket});
            if (Equals(token.Type, CoolTokenType.Inherits))
            {
                NextToken(); // eat "inherites"
                prtName = Synchronize(typeSet) as CoolToken;
                NextToken(); // eat "TYPE", the parent class name
                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.LeftBracket});
            }

            var features = new List<IAstNode>();
            NextToken(); // eat "{"
            while (!Equals(CurrentToken().Type, CoolTokenType.RightBracket))
            {
                var parser = new CoolFeatureParser(this);
                var feature = parser.Parse();
                features.Add(feature);

                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Semic});
                NextToken(); // eat ";"
            }

            NextToken(); // eat "}"
            return new CoolClassNode(clsName as CoolWordToken, prtName as CoolWordToken, features);
        }
    }
}