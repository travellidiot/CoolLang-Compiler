using System.Collections.Generic;
using Compiler.intermediate;
using System.Diagnostics;
using Compiler.intermediate.cool.ast;
using Compiler.intermediate.cool.symtab;

namespace Compiler.frontend.cool.parsers
{
    public class ProgramParser : TdParser
    {
        public ProgramParser(frontend.Scanner scanner) : base(scanner)
        {
        }

        public ProgramParser(TdParser parent) : base(parent)
        {
        }

        // program ::= 【class;】+
        public override IAstNode Parse()
        {
            // parsing
            var classes = new List<ClassNode>();

            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Class});
            var parser = new ClassParser(this);
            var clsNode = parser.Parse() as ClassNode;
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

                var node = parser.Parse() as ClassNode;
                classes.Add(node);

                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Semic });
                NextToken(); // eat ";"
            }

            return new ProgramNode(Scanner.FileName, classes) {LineNumber = 0, Position = 0};
        }
    }
}