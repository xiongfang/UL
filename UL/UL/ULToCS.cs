using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ULToCS
    {
        public readonly static string[] keywords = new string[]
        {
            "public",
            "namespace",
            "class",
            "void",
            "protected",
            "private",
            "override",
            "static",
            "if",
            "else",
            "switch",
            "do",
            "while"

        };
        ULTypeInfo typeInfo;

        int depth = 0;
        StringBuilder sb = new StringBuilder();

        void AppendLine(string t)
        {
            for (int i = 0; i < depth; i++)
            {
                sb.Append("\t");
            }
            sb.AppendLine(t);
        }

        void Append(string t)
        {
            sb.Append(t);
        }
        void BeginAppendLine()
        {
            for (int i = 0; i < depth; i++)
            {
                sb.Append("\t");
            }
        }

        void EndAppendLine()
        {
            sb.AppendLine();
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        public static string To(ULTypeInfo typeInfo)
        {
            var sb = new ULToCS();
            sb.typeInfo = typeInfo;
            sb.ToType();

            return sb.ToString();
        }

        void ToType()
        {
            if (!string.IsNullOrEmpty(typeInfo.Namespace))
            {
                AppendLine("namespace " + typeInfo.Namespace);
                AppendLine("{");
                depth++;
            }

            BeginAppendLine();

            if (typeInfo.Modifier == EModifier.Public)
            {
                Append("public ");
            }
            else if (typeInfo.Modifier == EModifier.Protected)
            {
                Append("protected ");
            }
            else
            {
                Append("private ");
            }

            Append("class " + typeInfo.Name);
            EndAppendLine();

            AppendLine("{");
            depth++;
            foreach (var m in typeInfo.Members)
            {

                ToMember(m);
            }
            depth--;
            AppendLine("}");

            if (!string.IsNullOrEmpty(typeInfo.Namespace))
            {
                depth--;
                AppendLine("}");
            }
        }

        void ToMember(ULMemberInfo memberInfo)
        {
            BeginAppendLine();

            if (memberInfo.Modifier == EModifier.Public)
            {
                Append("public ");
            }
            else if (memberInfo.Modifier == EModifier.Protected)
            {
                Append("protected ");
            }
            else
            {
                Append("private ");
            }

            if (memberInfo.IsStatic)
            {
                Append("static ");
            }
            var memberType = Data.GetType(memberInfo.TypeID);

            Append(memberType.Name);
            Append(" ");
            Append(memberInfo.Name);
            switch (memberInfo.MemberType)
            {
                case ULMemberInfo.EMemberType.Field:
                    Append(";");
                    break;
                case ULMemberInfo.EMemberType.Property:
                    Append("{ get; set;}");
                    break;
                case ULMemberInfo.EMemberType.Method:
                    Append("(");

                    if (memberInfo.Graph != null)
                    {
                        for (int i = 0; i < memberInfo.Graph.Args.Count; i++)
                        {
                            Append(Data.GetType(memberInfo.Graph.Args[i].TypeID).Name + " " + memberInfo.Graph.Args[i].Name);
                            if (i < memberInfo.Graph.Args.Count - 1)
                            {
                                Append(",");
                            }
                        }

                    }

                    Append(")");
                    break;
            }
            EndAppendLine();

            if (memberInfo.MemberType == ULMemberInfo.EMemberType.Method)
            {
                AppendLine("{");
                ToGraph(memberInfo.Graph);
                AppendLine("}");
            }
        }

        ULGraph _graph;
        void ToGraph(ULGraph block)
        {
            _graph = block;

            depth++;

            var start = block.Nodes.Find((v) => v.Name == ULNode.name_entry);
            if(start!=null)
                ToNode(start);
            depth--;


        }

        void ToNode(ULNode node)
        {
            if (node == null)
                return;

            if(node.Type == ULNode.ENodeType.Control)
            {
                switch(node.Name)
                {
                    case ULNode.name_if:
                        ToNode_IF(node);
                        break;
                    case ULNode.name_switch:
                        ToNode_Switch(node);
                        break;
                    case ULNode.name_entry:
                        ToNode_Entry(node);
                        break;
                }
            }
            else
            {
                ToNode_Method(node);
            }
        }


        ULNode GetControlNode(string control)
        {
            if (string.IsNullOrEmpty(control))
                return null;
            return _graph.FindNode(control.Split('.')[0]);
        }
        string GetInputArg(string input)
        {
            if (input == null)
                return "";

            if (input.Contains("."))
            {
                var str = input.Split('.');
                return (str[0] + "_" + str[1]);
            }
            else
            {
                return input;
            }
        }
        void ToNode_Entry(ULNode node)
        {
            if(node.ControlOutputs[0].Contains("."))
            {
                ToNode(GetControlNode(node.ControlOutputs[0]));
            }
            
        }


        void ToNode_IF(ULNode node)
        {
            AppendLine("if(" + GetInputArg(node.Inputs[0]) + ")");

            var trueNode = GetControlNode(node.ControlOutputs[0]);
            var falseNode = GetControlNode(node.ControlOutputs[1]);
            AppendLine("{");
            if (trueNode!=null)
            {
                
                depth++;
                ToNode(trueNode);
                depth--;
                
            }
            AppendLine("}");

            if (falseNode!=null)
            {
                AppendLine("else");
                AppendLine("{");
                depth++;
                ToNode(falseNode);
                depth--; 
                AppendLine("}");
            }
        }

        void ToNode_Switch(ULNode node)
        {

        }

        void ToNode_Method(ULNode node)
        {
            var method = Data.GetMember(node.Name);
            bool hasRet = method.Graph.Outputs.Count > 0;

            BeginAppendLine();
            if(hasRet)
            {
                Append("var " + node.NodeID + "_0 = ");
            }

            Append(method.Name);
            Append("(");
            for(int i=0;i<node.Inputs.Length;i++)
            {
                Append(GetInputArg(node.Inputs[i]));
                if (i!=node.Inputs.Length-1)
                {
                    Append(",");
                }
            }
            Append(");");
            EndAppendLine();
            ToNode(GetControlNode(node.ControlOutputs[0]));
        }
    }
}
