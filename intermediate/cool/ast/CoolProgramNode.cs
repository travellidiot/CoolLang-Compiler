using System.Collections.Generic;
using System.Linq;

namespace Compiler.intermediate.cool.ast
{
    public class CoolProgramNode : CoolAstNode
    {
        public string Name { get; }
        // List<CoolClassNode>
        public List<CoolClassNode> Classes { get; }

        public CoolProgramNode(string name, List<CoolClassNode> classes)
        {
            Classes = classes;
            Name = name;
            foreach (var cls in Classes)
            {
                cls.ParentNode = this;
            }
        }
        public override IAstNode Copy()
        {
            var classes = Classes.Select((node) => node.Copy()) as List<CoolClassNode>;
            var programNode = new CoolProgramNode(Name, classes);
            
            return programNode;
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }

        public CoolClassNode AddClass(CoolClassNode classNode)
        {
            Classes.Add(classNode);
            classNode.ParentNode = this;
            return classNode;
        }
    }
}