using System.Collections.Generic;
using System.Linq;

namespace Compiler.intermediate.coolast
{
    public class CoolClassNode : CoolAstNode
    {
        public string ClassName { get; set; }
        public string ParentName { get; set; }
        public List<CoolFeatureNode> Features;

        public CoolClassNode()
        {
            
        }

        public CoolClassNode(string clsName, string prtName, List<CoolFeatureNode> features)
        {
            ClassName = clsName;
            ParentName = prtName;
            Features = features;

            foreach (var feature in Features)
            {
                feature.ParentNode = this;
            }
        }

        public override CoolAstNode Copy()
        {
            var features = Features.Select((f) => f) as List<CoolFeatureNode>;
            var classNode = new CoolClassNode(ClassName, ParentName, features);

            return classNode;
        }

        public CoolFeatureNode AddFeature(CoolFeatureNode feature)
        {
            Features.Add(feature);
            feature.ParentNode = this;
            return feature;
        }
    }
}