using System.Collections.Generic;
using Compiler.intermediate;
using Compiler.intermediate.coolast;

namespace Compiler.frontend.cool.parsers
{
    public class CoolProgramParser : CoolTdParser
    {
        public CoolProgramParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolProgramParser(CoolTdParser parent) : base(parent)
        {
        }

        // program ::= 【class;】+
        public override IAstNode Parse()
        {
            var classes = new List<IAstNode>();

            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Class});
            var parser = new CoolClassParser(this);
            var clsNode = parser.Parse();
            classes.Add(clsNode);

            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Semic});
            NextToken(); // eat ";"

            while (CurrentToken().GetType() != typeof (EofToken))
            {
                if (Equals(CurrentToken().Type, CoolTokenType.Comment))
                {
                    NextToken();
                    continue;
                }

                var node = parser.Parse();
                classes.Add(node);

                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Semic });
                NextToken(); // eat ";"
            }

            return new CoolProgramNode(classes);
        }
    }
}