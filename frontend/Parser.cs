using System.Collections.Generic;
using Compiler.intermediate;
using Compiler.message;

namespace Compiler.frontend
{
    public abstract class Parser : IMessageProducer
    {
        public Scanner Scanner { get; protected set; }
        public IAst AstRoot { get; protected set; }
        protected List<IMessageListener> Listeners = new List<IMessageListener>();
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scanner">The scanner to be used with this parser</param>
        protected Parser(Scanner scanner)
        {
            Scanner = scanner;
            AstRoot = null;
        }

        /// <summary>
        /// Parse a Source program and generate the intermediate code and the symbol table.
        /// To be implemented by a language-specific parser subclass.
        /// </summary>
        public abstract IAstNode Parse();

        public abstract int GetErrorCount();

        public Token CurrentToken()
        {
            return Scanner.CurrentToken;
        }

        public Token NextToken()
        {
            return Scanner.NextToken();
        }

        public void AddListener(IMessageListener listener)
        {
            Listeners.Add(listener);
        }

        public void RemoveListener(IMessageListener listener)
        {
            Listeners.Remove(listener);
        }

        public void SendMessage(Message message)
        {
            foreach (var listener in Listeners)
            {
                listener.MessageReceived(message);
            }
        }
    }
}
