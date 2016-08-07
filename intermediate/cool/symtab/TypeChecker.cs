using System.Collections.Generic;
using System.Linq;
using Compiler.intermediate.cool.ast;
using Compiler.message;
using Compiler.message.cool;
using static Compiler.message.cool.ErrorHandler;

namespace Compiler.intermediate.cool.symtab
{
    public class TypeChecker : ICoolVisitor, IMessageProducer
    {
        private readonly SymbolTableStack _stack = new SymbolTableStack();
        private readonly List<IMessageListener> _listeners = new List<IMessageListener>();
        public TypeChecker(IScope gScope)
        {
            _stack.Push(gScope);
        }

        /// <summary>
        /// Check whether the 'cls' is subclass of parent
        /// </summary>
        /// <param name="cls"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool IsSubClass(ClassSymbol cls, ClassSymbol parent)
        {
            var sub = cls;
            while (sub.Parent != null)
            {
                if (sub.Parent == parent)
                    return true;
                sub = sub.Parent;
            }
            return false;
        }



        public void Visit(ProgramNode node)
        {
            var gScope = _stack.Peek();
            var pScope = new SymbolScope(node.Name, gScope);
            gScope.Enter(node.Name, pScope);

            _stack.Push(pScope);

            foreach (var cls in node.Classes)
            {
                cls.Accept(this);
            }

            _stack.Pop();
        }

        public void Visit(AntiNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(AssignNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ClassNode node)
        {
            var pScope = _stack.Peek();
            var clsScope = pScope.LookupForType(node.ClassName.Text);
            AssertTypeDefined(clsScope, node.ClassName, this);

            _stack.Push(clsScope);

            foreach (var feature in node.Features.Where(feature => feature is MethodNode))
            {
                feature.Accept(this);
            }

            _stack.Pop();
        }

        public void Visit(FactorNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(MethodNode node)
        {
            var clsScope = _stack.Peek();
            var mScope = clsScope.Lookup(node.MangledName);
        }

        public void Visit(AttrNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(FormalNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(IsVoidNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(NotNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(RalExprNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(SimpleExprNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(TermNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(IdNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(IntNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(StringNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(BoolNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ParenExprNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CallNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(IfNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(WhileNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(LetNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(PatternNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(NewObjNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(BlockNode node)
        {
            throw new System.NotImplementedException();
        }

        public void AddListener(IMessageListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(IMessageListener listener)
        {
            _listeners.Remove(listener);
        }

        public void SendMessage(Message message)
        {
            foreach (var listener in _listeners)
            {
                listener.MessageReceived(message);
            }
        }
    }
}