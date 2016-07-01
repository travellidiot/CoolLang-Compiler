using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.coolast
{
    public class CoolClassNode : CoolAstNode
    {
        public CoolWordToken ClassName { get; }
        public CoolWordToken ParentName { get; }

        // List<CoolFeatureNode>
        public List<IAstNode> Features;

        public CoolClassNode(CoolWordToken clsName, CoolWordToken prtName, List<IAstNode> features)
        {
            ClassName = clsName;
            ParentName = prtName;
            Features = features;

            foreach (var feature in Features)
            {
                feature.ParentNode = this;
            }
        }

        public override IAstNode Copy()
        {
            var features = Features.Select((f) => f.Copy()) as List<IAstNode>;
            var classNode = new CoolClassNode(ClassName, ParentName, features);

            return classNode;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public CoolAstNode AddFeature(CoolAstNode feature)
        {
            Features.Add(feature);
            feature.ParentNode = this;
            return feature;
        }
    }
}