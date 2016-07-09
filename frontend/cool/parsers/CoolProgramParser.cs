using System.Collections.Generic;
using Compiler.intermediate;
using System.Diagnostics;
using Compiler.intermediate.cool.ast;
using Compiler.intermediate.cool.symtab;
using static Compiler.frontend.cool.CoolErrorHandler;

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
            // progcessing symbol table
            Debug.Assert(ScopeStack.Count != 0, "Can\'t find global symbol scope!!!");

            var gScope = ScopeStack.Peek() as SymbolScope;
            Debug.Assert(gScope?.SymName == "Global",
                         $"Program \"{Scanner.FileName}\" is not in the Global namespace.");

            var programScope = new SymbolScope(Scanner.FileName, gScope);
            gScope.Enter(programScope.SymName, programScope);
            ScopeStack.Push(programScope);

            // parsing
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

            ScopeStack.Pop();

            return new CoolProgramNode(classes);
        }
    }
}