using System.Collections.Generic;
using System.Diagnostics;
using Compiler.frontend.cool.tokens;
using Compiler.intermediate;
using Compiler.intermediate.coolast;
using Compiler.intermediate.coolsymtab;

namespace Compiler.frontend.cool.parsers
{
    /*
     * feature ::= ID([formal 【, formal】*]) : TYPE { expr }
	 *           | ID : TYPE [<- expr]
     */
    public class CoolFeatureParser : CoolTdParser
    {
        public CoolFeatureParser(Scanner scanner) : base(scanner)
        {
        }

        public CoolFeatureParser(CoolTdParser parent) : base(parent)
        {
        }

        public override IAstNode Parse()
        {
            var token = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.ObjectId });
            NextToken();
            var afterId = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.LeftParen, CoolTokenType.Colon });

            Debug.Assert(ScopeStack.Count != 0, "Feature is not in the class");
            var outerScope = (SymbolScope)ScopeStack.Peek();

            // Attribute
            if (Equals(afterId.Type, CoolTokenType.Colon))
            {
                NextToken(); // eat ":"
                var typeToken = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.TypeId });
                NextToken(); // eat "TYPE"

                // if type is undefined, create an temporary class symbol
                var typeSymbol = outerScope.LookupForType(typeToken.Text);
                var idSymbol = new VariableSymbol(token.Text, typeSymbol);
                outerScope.Enter(idSymbol.SymName, idSymbol);
                

                if (!Equals(CurrentToken().Type, CoolTokenType.Assign))
                    return new CoolAttrNode(token as CoolWordToken, typeToken as CoolWordToken);

                NextToken(); // eat "<-"
                var exprParser = new CoolExprParser(this);
                var exprNode = exprParser.Parse();

                return new CoolAttrNode(token as CoolWordToken, typeToken as CoolWordToken, exprNode);
            }

            // Method
            var mScope = new MethodSymbolScope(token.Text, outerScope);
            outerScope.Enter(mScope.SymName, mScope);
            ScopeStack.Push(mScope);

            var formalParser = new CoolFormalParser(this);
            var args = new List<IAstNode>();
            if (!Equals(NextToken().Type, CoolTokenType.RightParen))
            {
                // eat "("
                var formalNode = formalParser.Parse();
                if (formalNode != null)
                {
                    args.Add(formalNode);

                    var formal = (CoolFormalNode) formalNode;
                    var idType = outerScope.LookupForType(formal.TypeName.Text);
                    var idSymbol = new VariableSymbol(formal.IdName.Text, idType);

                    mScope.Enter(idSymbol.SymName, idSymbol);
                    mScope.TypeSignature.Add(idType);
                }
            }

            while (!Equals(CurrentToken().Type, CoolTokenType.RightParen))
            {
                Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Comma });
                NextToken(); // eat ","
                var formalNode = formalParser.Parse();
                if (formalNode != null)
                {
                    args.Add(formalNode);

                    var formal = (CoolFormalNode)formalNode;
                    var idType = outerScope.LookupForType(formal.TypeName.Text);
                    var idSymbol = new VariableSymbol(formal.IdName.Text, idType);

                    mScope.Enter(idSymbol.SymName, idSymbol);
                    mScope.TypeSignature.Add(idType);
                }
            }

            NextToken(); // eat ")"
            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.Colon });
            NextToken(); // eat ":"
            var rtToken = Synchronize(new SortedSet<ITokenType>() { CoolTokenType.TypeId });
            NextToken(); // eat "TYPE"
            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.LeftBracket });

            var rtType = outerScope.LookupForType(rtToken.Text);
            mScope.TypeSignature.Add(rtType);

            var bodyParser = new CoolExprParser(this);
            NextToken(); // eat "{"
            var bodyNode = bodyParser.Parse();
            Synchronize(new SortedSet<ITokenType>() { CoolTokenType.RightBracket});
            NextToken(); // eat "}"

            ScopeStack.Pop();

            return new CoolMethodNode(token as CoolWordToken, args, rtToken as CoolWordToken, bodyNode);
        }
    }
}