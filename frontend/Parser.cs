using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.intermediate;
using Compiler.message;

namespace Compiler.frontend
{
    public abstract class Parser : IMessageProducer
    {
        public static ISymTabStack SymTabStack { get; protected set; } = SymTabFactory.CreateSymTabStack();
        public Scanner Scanner { get; protected set; }
        public ICode ICode { get; protected set; }
        public event EventHandler<Message> MessageHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scanner">The scanner to be used with this parser</param>
        protected Parser(Scanner scanner)
        {
            this.Scanner = scanner;
            this.ICode = null;
        }

        /// <summary>
        /// Parse a Source program and generate the intermediate code and the symbol table.
        /// To be implemented by a language-specific parser subclass.
        /// </summary>
        public abstract void Parse();

        public abstract int GetErrorCount();

        public Token CurrentToken()
        {
            return Scanner.CurrentToken;
        }

        public Token NextToken()
        {
            return Scanner.NextToken();
        }

        public void SendMessage(Message message)
        {
            MessageHandler?.Invoke(this, message);
        }
    }
}
