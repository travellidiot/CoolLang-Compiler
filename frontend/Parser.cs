using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.intermediate;

namespace Compiler.frontend
{
    public abstract class Parser
    {
        protected static ISymbolTable SymbolTable = null;
        protected Scanner scanner;
        protected ICode iCode;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scanner">The scanner to be used with this parser</param>
        protected Parser(Scanner scanner)
        {
            this.scanner = scanner;
            this.iCode = null;
        }

        /// <summary>
        /// Parse a source program and generate the intermediate code and the symbol table.
        /// To be implemented by a language-specific parser subclass.
        /// </summary>
        public abstract void Parse();

        public abstract int GetErrorCount();

        public Token CurrentToken()
        {
            return scanner.CurrentToken();
        }

        public Token NextToken()
        {
            return scanner.NetToken();
        }
    }
}
