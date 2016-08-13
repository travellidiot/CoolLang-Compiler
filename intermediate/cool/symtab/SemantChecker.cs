using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool;
using Compiler.intermediate.cool.ast;
using Compiler.message;
using Compiler.message.cool;
using static Compiler.message.cool.ErrorHandler;

namespace Compiler.intermediate.cool.symtab
{
    public class SemantChecker : ICoolVisitor, IMessageProducer
    {
        private readonly SymbolTableStack _stack = new SymbolTableStack();
        private readonly List<IMessageListener> _listeners = new List<IMessageListener>();
        private GlobalSymbolScope GlobalEnv { get; } = GlobalSymbolScope.Instance;
        private ClassSymbol ObjEnv { get; set; }

        public SemantChecker(IScope gScope)
        {
            _stack.Push(gScope);
        }

        /// <summary>
        /// Check whether the 'cls' is subtype of 'parent'
        /// </summary>
        /// <param name="cls"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool IsSubType(ClassSymbol cls, ClassSymbol parent)
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

        /// <summary>
        /// Return the "Least Upper Bound" of "lhs" and "rhs"
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns>The least upper bound of the two given class</returns>
        public ClassSymbol LUB(ClassSymbol lhs, ClassSymbol rhs)
        {
            if (lhs == rhs)
                return lhs;

            while (true)
            {
                if (lhs == GlobalEnv.SelfType)
                {
                    lhs = ObjEnv;
                    continue;
                }
                if (rhs == GlobalEnv.SelfType)
                {
                    var lhs1 = lhs;
                    lhs = rhs;
                    rhs = lhs1;
                    continue;
                }

                if (IsSubType(lhs, rhs))
                    return rhs;
                if (IsSubType(rhs, lhs))
                    return lhs;

                var cls = lhs;
                while (cls.Parent != null)
                {
                    if (IsSubType(rhs, cls.Parent))
                    {
                        return cls.Parent;
                    }
                    cls = cls.Parent;
                }
                return cls;
            }
        }


        public void Visit(ProgramNode node)
        {
            var gScope = _stack.Peek();
            var pScope = gScope.Lookup(node.Name);
            _stack.Push(pScope as SymbolScope);

            foreach (var cls in node.Classes)
            {
                cls.Accept(this);
            }

            _stack.Pop();
        }
        public void Visit(ClassNode node)
        {
            var pScope = _stack.Peek();
            var clsScope = pScope.LookupForType(node.ClassName.Text);
            AssertTypeDefined(clsScope, node.ClassName, this);

            _stack.Push(clsScope);
            ObjEnv = clsScope;

            var selfVar = new VariableSymbol("self", clsScope);
            ObjEnv.Enter(selfVar.SymName, selfVar);

            foreach (var feature in node.Features.Where(feature => feature is MethodNode))
            {
                feature.Accept(this);
            }

            _stack.Pop();
        }

        public void Visit(MethodNode node)
        {
            var clsScope = _stack.Peek();
            var mScope = clsScope.Lookup(node.MethodNameToken.GenMethodName());

            _stack.Push(mScope as SymbolScope);
            node.Expr.Accept(this);
            _stack.Pop();
        }

        public void Visit(NegNode node)
        {
            node.Expr.Accept(this);
            node.ValType = node.Expr.ValType == GlobalEnv.IntType
                ? GlobalEnv.IntType
                : GlobalEnv.ObjectType;

            if (node.Expr.ValType == GlobalEnv.IntType)
            {
                node.ValType = GlobalEnv.IntType;
            }
            else
            {
                FlagSemanticsError(node.Neg, ErrorCode.BadNegation, this);
                node.ValType = GlobalEnv.ObjectType;
            }
        }

        public void Visit(AssignNode node)
        {
            node.Id.Accept(this);
            node.Expr.Accept(this);

            var idt = (ClassSymbol)node.Id.ValType;
            var exprt = (ClassSymbol)node.Expr.ValType;
            if (idt == exprt || IsSubType(exprt, idt))
            {
                node.ValType = idt;
            }
            else
            {
                FlagSemanticsError(node.AssignToken, ErrorCode.BadAssignment, this);
                node.ValType = GlobalEnv.ObjectType;
            }
        }

        public void Visit(FactorNode node)
        {
            var factor = node.Factor;
            var term = node.Term;
            factor.Accept(this);
            term.Accept(this);
            if (factor.ValType == GlobalEnv.IntType && term.ValType == GlobalEnv.IntType)
            {
                node.ValType = GlobalEnv.IntType;
            }
            else
            {
                FlagSemanticsError(node.Op, ErrorCode.BadBinaryOperaton, this);
                node.ValType = GlobalEnv.ObjectType;
            }
        }

        

        public void Visit(IsVoidNode node)
        {
            node.ValType = GlobalEnv.BoolType;
        }

        public void Visit(NotNode node)
        {
            node.Expr.Accept(this);
            if (node.Expr.ValType == GlobalEnv.BoolType)
            {
                node.ValType = GlobalEnv.BoolType;
            }
            else
            {
                FlagSemanticsError(node.NotToken, ErrorCode.BadNotExpression, this);
                node.ValType = GlobalEnv.ObjectType;
            }
        }

        public void Visit(RalExprNode node)
        {
            var left = node.LeftSimpleExpr;
            var right = node.RightSimpleExpr;
            left.Accept(this);
            right.Accept(this);
            
            if (left.ValType == GlobalEnv.IntType ||
                left.ValType == GlobalEnv.BoolType ||
                left.ValType == GlobalEnv.StringType)
            {
                if (left.ValType == right.ValType)
                {
                    node.ValType = GlobalEnv.BoolType;
                }
                else
                {
                    FlagSemanticsError(node.Op, ErrorCode.BadComparation, this);
                    node.ValType = GlobalEnv.ObjectType;
                }
            }
            else if (Equals(node.Op.Type, CoolTokenType.Equal))
            {
                node.ValType = GlobalEnv.BoolType;
            }
            else
            {
                FlagSemanticsError(node.Op, ErrorCode.BadComparation, this);
                node.ValType = GlobalEnv.ObjectType;
            }
            
        }

        public void Visit(SimpleExprNode node)
        {
            var simp = node.SimpleExpr;
            var factor = node.Factor;
            simp.Accept(this);
            factor.Accept(this);
            if (simp.ValType == GlobalEnv.IntType && factor.ValType == GlobalEnv.IntType)
            {
                node.ValType = GlobalEnv.IntType;
            }
            else
            {
                FlagSemanticsError(node.Op, ErrorCode.BadBinaryOperaton, this);
                node.ValType = GlobalEnv.ObjectType;
            }
        }

        public void Visit(TermNode node)
        {
            node.Term.Accept(this);
            
            var env = (ClassSymbol) node.Term.ValType;
            if (node.DispatchType != null)
            {
                var dispatchType = env.LookupForType(node.DispatchType.Text);
                if (dispatchType == null || !IsSubType(env, dispatchType))
                {
                    FlagSemanticsError(node.DispatchType, ErrorCode.UndefinedType, this);
                    node.ValType = GlobalEnv.ObjectType;
                    return;
                }

                env = dispatchType;
            }
            
            var methodSym = env.Lookup(node.MethodName.GenMethodName()) as MethodSymbol;
            if (methodSym != null)
            {
                node.ValType = methodSym.RTType == GlobalEnv.SelfType ? env : methodSym.RTType;
            }
            else
            {
                FlagSemanticsError(node.MethodName, ErrorCode.UndefinedMethod, this);
                node.ValType = GlobalEnv.ObjectType;
            }
        }

        public void Visit(IdNode node)
        {
            var scope = _stack.Peek();
            var variable = scope.Lookup(node.IdToken.Text) as VariableSymbol;

            if (variable == null)
            {
                FlagSemanticsError(node.IdToken, ErrorCode.UndefinedVariable, this);
                node.ValType = GlobalEnv.ObjectType;
            }
            else
            {
                node.ValType = variable.SymType;
            }
        }

        public void Visit(IntNode node)
        {
            node.ValType = GlobalEnv.IntType;
        }

        public void Visit(StringNode node)
        {
            node.ValType = GlobalEnv.StringType;
        }

        public void Visit(BoolNode node)
        {
            node.ValType = GlobalEnv.BoolType;
        }

        public void Visit(ParenExprNode node)
        {
            node.Expr.Accept(this);
            node.ValType = node.Expr.ValType;
        }

        public void Visit(CallNode node)
        {
            var mSymbol = ObjEnv.Lookup(node.Id.GenMethodName()) as MethodSymbol;
            if (mSymbol != null)
            {
                node.ValType = mSymbol.RTType == GlobalEnv.SelfType ? ObjEnv : mSymbol.RTType;
            }
            else
            {
                FlagSemanticsError(node.Id, ErrorCode.UndefinedMethod, this);
                node.ValType = GlobalEnv.ObjectType;
            }
        }

        public void Visit(IfNode node)
        {
            node.Pred.Accept(this);
            node.Expr.Accept(this);
            node.Alter?.Accept(this);

            if (node.Pred.ValType == GlobalEnv.BoolType)
            {
                if (node.Alter == null)
                    node.ValType = node.Expr.ValType;
                else
                {
                    var type = LUB((ClassSymbol) node.Expr.ValType, (ClassSymbol) node.Alter.ValType);
                    node.ValType = type;
                }
            }
            else
            {
                FlagSemanticsError(node.IfToken, ErrorCode.CondNotBoolean, this);
                node.ValType = GlobalEnv.ObjectType;
            }
        }

        public void Visit(WhileNode node)
        {
            node.Pred.Accept(this);
            if (node.Pred.ValType == GlobalEnv.BoolType)
            {
                node.LoopExpr.Accept(this);
            }
            else
            {
                FlagSemanticsError(node.WhileToken, ErrorCode.CondNotBoolean, this);
            }
            node.ValType = GlobalEnv.ObjectType;
        }

        public void Visit(AttrNode node)
        {
            var attrType = ObjEnv.LookupForType(node.TypeName.Text);
            AssertTypeDefined(attrType, node.TypeName, this);
            if (attrType == GlobalEnv.SelfType)
                attrType = ObjEnv;

            if (node.Expr != null)
            {
                node.Expr.Accept(this);
                var exprType = (ClassSymbol) node.Expr.ValType;
                if (exprType == attrType || IsSubType(exprType, attrType))
                {
                    node.ValType = attrType;
                }
                else
                {
                    FlagSemanticsError(node.AttrName, ErrorCode.BadAssignment, this);
                    node.ValType = GlobalEnv.ObjectType;
                }
            }
            node.ValType = attrType;
        }
        public void Visit(LetNode node)
        {
            var scope = (MethodSymbol)_stack.Peek();
            foreach (var attr in node.Attrs)
            {
                attr.Accept(this);
                var attrType = ObjEnv.LookupForType(attr.TypeName.Text);
                AssertTypeDefined(attrType, attr.TypeName, this);
                var variable = new VariableSymbol(attr.AttrName.Text, attrType);
                scope.Shadow(variable.SymName, variable);
            }

            node.Expr.Accept(this);
            node.ValType = node.Expr.ValType;

            scope.RevertHiddenSymbols();
        }

        public void Visit(PatternNode node)
        {
            ClassSymbol lub = null;
            foreach (var cs in node.Cases)
            {
                var scope = (MethodSymbol)_stack.Peek();
                var formal = cs.Item1;
                var expr = cs.Item2;
                var typeName = ObjEnv.LookupForType(formal.TypeName.Text);
                AssertTypeDefined(typeName, formal.TypeName, this);

                var variable = new VariableSymbol(formal.IdName.Text, typeName);

                scope.Shadow(variable.SymName, variable);

                expr.Accept(this);
                var valType = (ClassSymbol) expr.ValType;
                lub = lub == null ? valType : LUB(lub, valType);
               
                scope.RevertHiddenSymbols();
            }
            node.ValType = lub;
        }

        public void Visit(NewObjNode node)
        {
            var type = ObjEnv.LookupForType(node.TypeToken.Text);
            AssertTypeDefined(type, node.TypeToken, this);

            if (type == GlobalEnv.SelfType)
                type = ObjEnv;

            node.ValType = type;
        }

        public void Visit(BlockNode node)
        {
            node.ValType = node.Exprs.Select(expr => expr.Accept(this).ValType).Last();
        }

        public void Visit(FormalNode node)
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