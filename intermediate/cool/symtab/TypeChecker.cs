using System.Collections.Generic;
using Compiler.intermediate.cool.ast;
using Compiler.message;
using Compiler.message.cool;
using static Compiler.message.cool.CoolErrorHandler;

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

        public void Visit(CoolProgramNode node)
        {
            var gScope = _stack.Peek();
            var programScope = new SymbolScope(node.Name, gScope);
            gScope.Enter(node.Name, programScope);

            var gather = new TypeGather();
            gather.Gather(node, programScope);

            _stack.Push(programScope);

            foreach (var cls in node.Classes)
            {
                cls.Accept(this);
            }

            _stack.Pop();
        }

        public void Visit(CoolAntiNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolAssignNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolClassNode node)
        {
            var programScope = _stack.Peek();
            var clsSymbol = programScope.LookupForType(node.ClassName.Text);
            AssertTypeDefined(clsSymbol, node.ClassName, this);

            foreach (var feature in node.Features)
            {
                if (feature is CoolAttrNode)
                {
                    var attr = feature as CoolAttrNode;
                    var attrTypeSymbol = programScope.LookupForType(attr.TypeName.Text);

                    AssertTypeDefined(attrTypeSymbol, attr.TypeName, this);

                    var variable = new VariableSymbol(attr.AttrName.Text, attrTypeSymbol);
                    try
                    {
                        clsSymbol.Enter(variable.SymName, variable);
                    }
                    catch (System.ArgumentException)
                    {
                        FlagSemanticsError(attr.AttrName, CoolErrorCode.RedefineVariable, this);
                    }
                }
                else if (feature is CoolMethodNode)
                {
                    var method = feature as CoolMethodNode;
                    var rtTypeSymbol = programScope.LookupForType(method.RetType.Text);

                    AssertTypeDefined(rtTypeSymbol, method.RetType, this);

                    var methodSymbol = new MethodSymbol(method.MethodName.Text, clsSymbol) {RTType = rtTypeSymbol};
                    foreach (var formal in method.Formals)
                    {
                        var formalType = programScope.LookupForType(formal.TypeName.Text);
                        AssertTypeDefined(formalType, formal.TypeName, this);
                        methodSymbol.Formals.Add(formalType);
                    }

                    var mName = methodSymbol.MangleName();
                    try
                    {
                        clsSymbol.Enter(mName, methodSymbol);
                    }
                    catch (System.ArgumentException)
                    {
                        FlagSemanticsError(method.MethodName, CoolErrorCode.RedefineMethod, this);
                    }
                }
            }
        }

        public void Visit(CoolFactorNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolMethodNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolAttrNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolFormalNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolIsVoidNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolNotNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolRalExprNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolSimpleExprNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolTermNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolIdNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolIntNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolStringNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolBoolNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolParenExprNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolCallNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolIfNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolWhileNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolLetNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolPatternNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolNewObjNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolBlockNode node)
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