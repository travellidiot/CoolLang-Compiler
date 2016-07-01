using System.Collections.Generic;
using System.Linq;

namespace Compiler.intermediate.coolast
{
    public class CoolProgramNode : CoolAstNode
    {
        // List<CoolClassNode>
        public List<IAstNode> Classes { get; }

        public CoolProgramNode(List<IAstNode> classes)
        {
            Classes = classes;
            foreach (var cls in Classes)
            {
                cls.ParentNode = this;
            }
        }
        public override IAstNode Copy()
        {
            var classes = Classes.Select((node) => node.Copy()) as List<IAstNode>;
            var programNode = new CoolProgramNode(classes);
            
            return programNode;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public CoolClassNode AddClass(CoolClassNode classNode)
        {
            Classes.Add(classNode);
            classNode.ParentNode = this;
            return classNode;
        }
    }
}