using System.Collections.Generic;
using System.Linq;

namespace Compiler.intermediate.coolast
{
    public class CoolProgramNode : CoolAstNode
    {
        public List<CoolClassNode> Classes { get; set; }
        public override CoolAstNode Copy()
        {
            var programNode = new CoolProgramNode();
            var classes = Classes.Select((node) =>
            {
                node.ParentNode = programNode;
                return node;
            }) as List<CoolClassNode>;

            programNode.Classes = classes;

            return programNode;
        }

        public CoolClassNode AddClass(CoolClassNode classNode)
        {
            Classes.Add(classNode);
            classNode.ParentNode = this;
            return classNode;
        }
    }
}