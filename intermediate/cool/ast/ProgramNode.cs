using System.Collections.Generic;
using System.Linq;

namespace Compiler.intermediate.cool.ast
{
    public class ProgramNode : AstNode
    {
        public string Name { get; }
        // List<ClassNode>
        public List<ClassNode> Classes { get; }

        public ProgramNode(string name, List<ClassNode> classes)
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
            var classes = Classes.Select((node) => node.Copy()) as List<ClassNode>;
            var programNode = new ProgramNode(Name, classes);
            
            return programNode;
        }

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }

        public ClassNode AddClass(ClassNode classNode)
        {
            Classes.Add(classNode);
            classNode.ParentNode = this;
            return classNode;
        }
    }
}