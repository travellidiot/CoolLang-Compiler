using System.Collections.Generic;
using Compiler.intermediate;
using System.Diagnostics;
using Compiler.intermediate.cool.ast;
using Compiler.intermediate.cool.symtab;

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
            // parsing
            var classes = new List<CoolClassNode>();

            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Class});
            var parser = new CoolClassParser(this);
            var clsNode = parser.Parse() as CoolClassNode;
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

                var node = parser.Parse() as CoolClassNode;
                classes.Add(node);

                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Semic });
                NextToken(); // eat ";"
            }

            return new CoolProgramNode(Scanner.FileName, classes);
        }
    }
}