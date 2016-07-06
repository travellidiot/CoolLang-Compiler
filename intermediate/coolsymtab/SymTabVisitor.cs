using Compiler.frontend.cool;
using Compiler.intermediate.coolast;

namespace Compiler.intermediate.coolsymtab
{
    public class SymTabVisitor : IVisitor
    {
        public void Visit(CoolProgramNode node)
        {
            node.Scope = new SymbolScope("Global", null);
            node.Scope.Enter("Object", new BuiltInTypeSymbol("Object"));
            node.Scope.Enter("Int", new BuiltInTypeSymbol("Int"));
            node.Scope.Enter("IO", new BuiltInTypeSymbol("IO"));
            node.Scope.Enter("String", new BuiltInTypeSymbol("IO"));
            node.Scope.Enter("Bool", new BuiltInTypeSymbol("Bool"));

            foreach (var cls in node.Classes)
            {
                var clsName = ((CoolClassNode) cls).ClassName.Text;
                var clsSymbol = new ClassSymbolScope(clsName, node.Scope);
                node.Scope.Enter(clsName, clsSymbol);
                cls.Scope = clsSymbol;
            }

            foreach (var cls in node.Classes)
            {
                cls.Accept(this);
            }
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
            foreach (var feature in node.Features)
            {
                feature.Accept(this);
            }
        }

        public void Visit(CoolFactorNode node)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(CoolMethodNode node)
        {
            var methodName = node.MethodName.Text;
            var methodSymbol = new MethodSymbolScope(methodName, node.ParentNode.Scope);
            node.ParentNode.Scope.Enter(methodName, methodSymbol);

            node.Scope = methodSymbol;
            foreach (var arg in node.Formals)
            {
                var formal = (CoolFormalNode)arg;
                var argName = formal.IdName.Text;
                var argType = node.ParentNode.Scope.Lookup(formal.TypeName.Text);

                if (argType == null)
                {
                }
            }
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

        public void Visit(CoolValueNode node)
        {
            throw new System.NotImplementedException();
        }
    }
}