﻿using System;
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

            Append(memberType != null ? memberType.Name : "void");
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
                        int arg_count = memberInfo.Graph.Args.Count + memberInfo.Graph.Outputs.Count;
                        if (memberType != null)
                            arg_count -= 1;
                        for (int i = 0; i < memberInfo.Graph.Args.Count; i++)
                        {
                            Append(Data.GetType(memberInfo.Graph.Args[i].TypeID).Name + " " + memberInfo.Graph.Args[i].Name);
                            if (i < arg_count - 1)
                            {
                                Append(",");
                            }
                        }
                        for (int i = 0; i < memberInfo.Graph.Outputs.Count; i++)
                        {
                            if (memberInfo.Graph.Outputs[i].Name != "ret")
                            {
                                Append("out " + Data.GetType(memberInfo.Graph.Outputs[i].TypeID).Name + " " + memberInfo.Graph.Outputs[i].Name);
                                if (i + memberInfo.Graph.Args.Count < arg_count - 1)
                                {
                                    Append(",");
                                }
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
            if (start != null)
                ToNode(start);
            depth--;


        }

        void ToNode(ULNode node)
        {
            if (node == null)
                return;


            switch (node.Name)
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
                default:
                    ToNode_Method(node);
                    break;
            }
        }


        ULNode GetControlNode(ULPin control)
        {
            if (string.IsNullOrEmpty(control.Link))
                return null;
            return _graph.FindNode(control.Link.Split('.')[0]);
        }
        string GetInputArg(ULPin input)
        {
            if (input == null || string.IsNullOrEmpty(input.Link))
                return "";

            if (input.Link.Contains("."))
            {
                var str = input.Link.Split('.');
                return (str[0] + "_" + str[1]);
            }
            else
            {
                return input.Value;
            }
        }
        void ToNode_Entry(ULNode node)
        {
            var next = GetControlNode(node.Outputs[0]);
            if (next!=null)
            {
                ToNode(next);
            }
            
        }


        void ToNode_IF(ULNode node)
        {
            AppendLine("if(" + GetInputArg(node.Inputs[0]) + ")");

            var trueNode = GetControlNode(node.Outputs[0]);
            var falseNode = GetControlNode(node.Outputs[1]);
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
            if (method == null)
                return;

            bool hasRet = method.Graph.Outputs.Count > 0;

            BeginAppendLine();
            if(hasRet)
            {
                Append("var " + node.NodeID + "_0 = ");
            }

            Append(method.Name);
            Append("(");
            for(int i=0;i<node.Inputs.Count;i++)
            {
                Append(GetInputArg(node.Inputs[i]));
                if (i!=node.Inputs.Count - 1)
                {
                    Append(",");
                }
            }
            Append(");");
            EndAppendLine();
            ToNode(GetControlNode(node.Outputs[0]));
        }
    }
}
