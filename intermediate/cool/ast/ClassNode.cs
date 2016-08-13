using System.Collections.Generic;
using System.Linq;
using Compiler.frontend.cool.tokens;

namespace Compiler.intermediate.cool.ast
{
    public class ClassNode : AstNode
    {
        public WordToken ClassName { get; }
        public WordToken ParentName { get; }

        // List<FeatureNode>
        public List<FeatureNode> Features;

        public ClassNode(WordToken clsName, WordToken prtName, List<FeatureNode> features)
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
            var features = Features.Select((f) => f.Copy()) as List<FeatureNode>;
            var classNode = new ClassNode(ClassName, ParentName, features);

            return classNode;
        }

        public override IAstNode Accept(ICoolVisitor coolVisitor)
        {
            coolVisitor.Visit(this);
            return this;
        }

        public FeatureNode AddFeature(FeatureNode feature)
        {
            Features.Add(feature);
            feature.ParentNode = this;
            return feature;
        }
    }
}