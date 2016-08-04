using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public class CoolClassNode : CoolAstNode
    {
        public CoolWordToken ClassName { get; }
        public CoolWordToken ParentName { get; }

        // List<CoolFeatureNode>
        public List<CoolFeatureNode> Features;

        public CoolClassNode(CoolWordToken clsName, CoolWordToken prtName, List<CoolFeatureNode> features)
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
            var features = Features.Select((f) => f.Copy()) as List<CoolFeatureNode>;
            var classNode = new CoolClassNode(ClassName, ParentName, features);

            return classNode;
        }

        public override void Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
        }

        public CoolFeatureNode AddFeature(CoolFeatureNode feature)
        {
            Features.Add(feature);
            feature.ParentNode = this;
            return feature;
        }
    }
}