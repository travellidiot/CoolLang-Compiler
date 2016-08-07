using System.Collections.Generic;
using System.Diagnostics;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.cool.ast;
using Compiler.intermediate.cool.symtab;

namespace Compiler.frontend.cool.parsers
{
    public class ClassParser : TdParser
    {
        public ClassParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public ClassParser(TdParser parent) : base(parent)
        {
        }

        // class TYPE [inherits TYPE] { 【feature;】* }
        public override IAstNode Parse()
        {
            Synchronize(new SortedSet<ITokenType>() {CoolTokenType.Class});
            var typeSet = new SortedSet<ITokenType>() { CoolTokenType.TypeId};

            NextToken(); // eat "class"
            var clsName = Synchronize(typeSet);
            Token prtName = null;

            NextToken(); // eat "TYPE", the class name
            var token = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Inherits, CoolTokenType.LeftBracket});
            if (Equals(token.Type, CoolTokenType.Inherits))
            {
                NextToken(); // eat "inherites"
                prtName = (Token)Synchronize(typeSet);
                NextToken(); // eat "TYPE", the parent class name
                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.LeftBracket});
            }

            var features = new List<FeatureNode>();
            NextToken(); // eat "{"
            while (!Equals(CurrentToken().Type, CoolTokenType.RightBracket))
            {
                var parser = new FeatureParser(this);
                var feature = parser.Parse() as FeatureNode;
                features.Add(feature);

                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Semic});
                NextToken(); // eat ";"
            }

            NextToken(); // eat "}"

            return new ClassNode(clsName as WordToken, prtName as WordToken, features);
        }
    }
}