using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Compiler.intermediate.cool.ast;
using Compiler.message;
using Compiler.message.cool;
using static Compiler.message.cool.ErrorHandler;

namespace Compiler.intermediate.cool.symtab
{
    public class SymbolBuilder : ICoolVisitor, IMessageProducer
    {
        private readonly List<IMessageListener> _listeners = new List<IMessageListener>();
        private readonly SymbolTableStack _symbolStack = new SymbolTableStack();

        public SymbolBuilder(IScope gScope)
        {
            _symbolStack.Push(gScope);
        }

        public void Visit(ProgramNode node)
        {
            var gScope = _symbolStack.Peek();
            var pScope = new SymbolScope(node.Name, gScope);
            gScope.Enter(pScope.SymName, pScope);

            foreach (var cls in node.Classes)
            {
                var clsScope = new ClassSymbol(cls.ClassName.Text, pScope);
                try
                {
                    pScope.Enter(cls.ClassName.Text, clsScope);
                }
                catch (System.ArgumentException)
                {
                    FlagSemanticsError(cls.ClassName, ErrorCode.RedefineType, this);
                }
            }

            _symbolStack.Push(pScope);
            foreach (var cls in node.Classes)
            {
                cls.Accept(this);
            }
        }

       
        public void Visit(ClassNode node)
        {
            var pScope = _symbolStack.Peek();
            var clsSymbol = pScope.LookupForType(node.ClassName.Text);
            AssertTypeDefined(clsSymbol, node.ClassName, this);

            _symbolStack.Push(clsSymbol);
            foreach (var feature in node.Features)
            {
                if (feature is AttrNode)
                {
                    var attr = feature as AttrNode;
                    var attrTypeSymbol = pScope.LookupForType(attr.TypeName.Text);

                    AssertTypeDefined(attrTypeSymbol, attr.TypeName, this);

                    var variable = new VariableSymbol(attr.AttrName.Text, attrTypeSymbol);
                    try
                    {
                        clsSymbol.Enter(variable.SymName, variable);
                    }
                    catch (System.ArgumentException)
                    {
                        FlagSemanticsError(attr.AttrName, ErrorCode.RedefineVariable, this);
                    }
                }
                else if (feature is MethodNode)
                {
                    var method = feature as MethodNode;
                    var rtTypeSymbol = pScope.LookupForType(method.RetType.Text);

                    AssertTypeDefined(rtTypeSymbol, method.RetType, this);

                    var methodSymbol = new MethodSymbol(method.MethodName.Text, clsSymbol) { RTType = rtTypeSymbol };
                    foreach (var formal in method.Formals)
                    {
                        var formalType = pScope.LookupForType(formal.TypeName.Text);
                        AssertTypeDefined(formalType, formal.TypeName, this);
                        methodSymbol.Formals.Add(formalType);
                    }

                    var mName = method.MangledName;
                    try
                    {
                        clsSymbol.Enter(mName, methodSymbol);
                    }
                    catch (System.ArgumentException)
                    {
                        FlagSemanticsError(method.MethodName, ErrorCode.RedefineMethod, this);
                    }
                }
            }

            foreach (var feature in node.Features.OfType<MethodNode>())
            {
                feature.Accept(this);
            }
        }

        public void Visit(MethodNode node)
        {
            var clsScope = _symbolStack.Peek();
            var methodScope = clsScope.Lookup(node.MangledName) as SymbolScope;
            Debug.Assert(methodScope != null);

            foreach (var variable in
                        from formal in node.Formals
                        let typeSymbol = clsScope.LookupForType(formal.TypeName.Text)
                        select new VariableSymbol(formal.IdName.Text, typeSymbol))
            {
                methodScope.Enter(variable.SymName, variable);
            }

            _symbolStack.Push(methodScope);

            node.Expr.Accept(this);
        }

        public void Visit(AntiNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(FactorNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(AttrNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(FormalNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IsVoidNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(NotNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(RalExprNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(SimpleExprNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(TermNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IdNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IntNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(StringNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(BoolNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(ParenExprNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(CallNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IfNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(WhileNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(LetNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(PatternNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(NewObjNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(BlockNode node)
        {
            throw new NotImplementedException();
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