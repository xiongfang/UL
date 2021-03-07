using Microsoft.CodeAnalysis.CSharp.Syntax;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace UL.CompileNode
{

    public class CompileNode_ExportBody : CompileNode
    {
        public ULGraph graph;

        Dictionary<string, string> LocalVarables = new Dictionary<string, string>();

        public override IdentifierInfo GetIdentifierInfo(string identifier)
        {
            if(LocalVarables.TryGetValue(identifier,out var type))
            {
                IdentifierInfo info = new IdentifierInfo();
                info.type = IdentifierInfo.EIdentifierType.Local;
                info.TypeID = type;
                return info;
            }

            return base.GetIdentifierInfo(identifier);
        }

        public void ExportBody(BlockSyntax bs, ULGraph graph)
        {
            this.graph = graph;
            var entry = ULNode.NewControlNode(ULNode.name_entry);

            graph.Nodes.Add(entry);

            //NodeBlock block = new NodeBlock(entry, 0);
            //blocks.Push(block);
            var n = ExportStatement(bs);
            if(n!=null)
                entry.LinkTo(n, 0, 0);
        }

        ULNode ExportStatement(StatementSyntax node)
        {
            if (node is IfStatementSyntax)
            {
                return ExportStatement(node as IfStatementSyntax);
            }
            else if (node is ExpressionStatementSyntax)
            {
                return ExportStatement(node as ExpressionStatementSyntax);
            }
            else if (node is BlockSyntax)
            {
                return ExportStatement(node as BlockSyntax);
            }
            else if (node is LocalDeclarationStatementSyntax)
            {
                return ExportStatement(node as LocalDeclarationStatementSyntax);
            }
            else if (node is ForStatementSyntax)
            {
                return ExportStatement(node as ForStatementSyntax);
            }
            else if (node is DoStatementSyntax)
            {
                return ExportStatement(node as DoStatementSyntax);
            }
            else if (node is WhileStatementSyntax)
            {
                return ExportStatement(node as WhileStatementSyntax);
            }
            //else if (node is SwitchStatementSyntax)
            //{
            //    ExportStatement(node as SwitchStatementSyntax);
            //}
            else if (node is BreakStatementSyntax)
            {
                return ExportStatement(node as BreakStatementSyntax);
            }
            else if (node is ReturnStatementSyntax)
            {
                ExportStatement(node as ReturnStatementSyntax);
            }
            //else if (node is TryStatementSyntax)
            //{
            //    ExportStatement(node as TryStatementSyntax);
            //}
            //else if (node is ThrowStatementSyntax)
            //{
            //    ExportStatement(node as ThrowStatementSyntax);
            //}
            else
            {
                Console.Error.WriteLine("error:Unsopproted StatementSyntax" + node);
            }

            return null;
        }

        ULNode ExportStatement(BlockSyntax node)
        {
            //currentBlock = new NodeBlock(preNode, pinOutIndex);
            ULNode firstNode = null;
            ULNode preNode = null;
            foreach (var s in node.Statements)
            {
                var n = ExportStatement(s);
                if(n != null)
                {
                    if (firstNode == null)
                    {
                        firstNode = n;
                    }
                    if(preNode!=null)
                    {
                        preNode.LinkTo(n, 0, 0);
                    }

                    preNode = n;
                }
                
            }

            return firstNode;
        }

        ULNode ExportStatement(IfStatementSyntax node)
        {
            ULNode nodeIf = ULNode.NewControlNode(ULNode.name_if);
            graph.Nodes.Add(nodeIf);

            var cond = ExportExp(node.Condition);
            cond.LinkTo(nodeIf, 0, 1);

            if (node.Statement != null)
            {
                var node_true = ExportStatement(node.Statement);
                if(node_true!=null)
                    nodeIf.LinkTo(node_true, 1, 0);
            }
            if (node.Else!=null && node.Else.Statement != null)
            {
                var node_false = ExportStatement(node.Else.Statement);
                if(node_false!=null)
                    nodeIf.LinkTo(node_false, 2, 0);
            }

            return nodeIf;
        }
        ULNode ExportStatement(WhileStatementSyntax node)
        {
            ULNode nodeWhile = ULNode.NewControlNode(ULNode.name_while);
            graph.Nodes.Add(nodeWhile);

            var cond = ExportExp(node.Condition);
            cond.LinkTo(nodeWhile, 1, 0);

            if (node.Statement != null)
            {
                var node_true = ExportStatement(node.Statement);
                nodeWhile.LinkTo(node_true, 1, 0);
            }

            return nodeWhile;

            //var cond = ExportExp(node.Condition);

            //var ifStatement = new ULStatementWhile();
            //ifStatement.Parent = currentBlock;
            //ifStatement.arg = cond.GetOutputName(0);
            //if (node.Statement is BlockSyntax)
            //    ifStatement.block = ExportStatement(node.Statement as BlockSyntax);
            //currentBlock.statements.Add(ifStatement);
        }

        ULNode ExportStatement(ExpressionStatementSyntax node)
        {
            var call = ExportExp(node.Expression);
            return call;
        }

        ULNode ExportStatement(LocalDeclarationStatementSyntax ss)
        {
            ULNode firstNode = null;
            ULNode preNode = null;
            var Type = GetTypeInfo(ss.Declaration.Type);
            foreach (var v in ss.Declaration.Variables)
            {
                var vName = v.Identifier.Text;
                LocalVarables[vName] = Type.ID;
                if (v.Initializer != null)
                {
                    ULNode node = ULNode.NewControlNode(ULNode.name_setlocal);
                    graph.Nodes.Add(node);
                    var exp = ExportExp(v.Initializer.Value);
                    exp.LinkTo(node, 0, 1);
                    if(firstNode == null)
                    {
                        firstNode = node;
                    }
                    if(preNode == null)
                    {
                        preNode = node;
                    }
                    else
                    {
                        preNode.LinkTo(node);
                        preNode = node;
                    }
                }
            }
            return firstNode;
        }

        ULNode ExportStatement(ForStatementSyntax ss)
        {
            var node = ULNode.NewControlNode(ULNode.name_for);
            graph.Nodes.Add(node);

            var Condition = ExportExp(ss.Condition);
            Condition.LinkTo(node, 0, 1);
            ULNode Declaration = ExportExp(ss.Declaration);
            foreach (var inc in ss.Incrementors)
            {
                var i = ExportExp(inc);
                node.LinkTo(i, 1, 0);
            }
            var block = ExportStatement(ss.Statement as BlockSyntax);
            node.LinkTo(block,2, 0);
            Declaration.LinkTo(node, 0, 0);
            return Declaration;
        }

        ULNode ExportStatement(DoStatementSyntax ss)
        {
            
            var node = ULNode.NewControlNode(ULNode.name_do);
            graph.Nodes.Add(node);

            var cond = ExportExp(ss.Condition);


            cond.LinkTo(node, 1, 0);

            if (ss.Statement != null)
            {
                var node_true = ExportStatement(ss.Statement);
                node.LinkTo(node_true, 1, 0);
            }

            return node;
        }

        ULNode ExportStatement(BreakStatementSyntax ss)
        {
            var node = ULNode.NewControlNode(ULNode.name_break);
            graph.Nodes.Add(node);

            return node;
        }

        ULNode ExportStatement(ReturnStatementSyntax ss)
        {
            var node = ULNode.NewControlNode(ULNode.name_exit);
            graph.Nodes.Add(node);

            var exp = ExportExp(ss.Expression);
            exp.LinkTo(node, 0, 1);

            return node;
        }

        void ExportStatement(SwitchStatementSyntax ss)
        {
            //var node = new ULStatementSwitch();
            //node.Parent = currentBlock;
            //node.Condition = ExportExp(ss.Expression).GetOutputName(0);
            //node.Sections = new List<ULStatementSwitch.Section>();
            //foreach (var s in ss.Sections)
            //{
            //    var section = new ULStatementSwitch.Section();
            //    section.Labels = new List<string>();
            //    section.Statements = new List<ULNodeBlock>();
            //    node.Sections.Add(section);

            //    foreach (var l in s.Labels)
            //    {
            //        if (l is CaseSwitchLabelSyntax)
            //        {
            //            CaseSwitchLabelSyntax csls = l as CaseSwitchLabelSyntax;
            //            section.Labels.Add(ExportExp(csls.Value).GetOutputName(0));
            //        }
            //    }

            //    foreach (var statement in s.Statements)
            //    {
            //        section.Statements.Add(ExportStatement(statement as BlockSyntax));
            //    }
            //}



            //currentBlock.statements.Add(node);
        }

        ULNode ExportExp(VariableDeclarationSyntax es)
        {
            var typeInfo = GetTypeInfo(es.Type);

            ULNode firstNode = null;
            ULNode preNode = null;

            foreach (var v in es.Variables)
            {
                //node = new ULCall();
                //node.Parent = currentBlock;
                //node.callType = ULCall.ECallType.DeclarationLocal;
                //node.Args.Add(typeName);
                //node.Args.Add(v.Identifier.Text);
                LocalVarables[v.Identifier.Text] = typeInfo.ID;
                if (v.Initializer != null)
                {
                    ULNode node = ULNode.NewControlNode(ULNode.name_setlocal);
                    graph.Nodes.Add(node);
                    var exp = ExportExp(v.Initializer.Value);
                    exp.LinkTo(node, 0, 1);
                    if (firstNode == null)
                    {
                        firstNode = node;
                    }
                    if (preNode == null)
                    {
                        preNode = node;
                    }
                    else
                    {
                        preNode.LinkTo(node);
                        preNode = node;
                    }
                }
            }

            return firstNode;
        }

        ULNode ExportExp(ExpressionSyntax es)
        {
            if (es is LiteralExpressionSyntax)
            {
                return ExportExp(es as LiteralExpressionSyntax);
            }
            else if (es is ThisExpressionSyntax)
            {
                return ExportExp(es as ThisExpressionSyntax);
            }
            else if (es is ObjectCreationExpressionSyntax)
            {
                return ExportExp(es as ObjectCreationExpressionSyntax);
            }
            else if (es is InvocationExpressionSyntax)
            {
                return ExportExp(es as InvocationExpressionSyntax);
            }
            else if (es is MemberAccessExpressionSyntax)
            {
                return ExportExp(es as MemberAccessExpressionSyntax);
            }
            else if (es is IdentifierNameSyntax)
            {
                return ExportExp(es as IdentifierNameSyntax);
            }
            else if (es is AssignmentExpressionSyntax)
            {
                return ExportExp(es as AssignmentExpressionSyntax);
            }
            else if (es is BinaryExpressionSyntax)
            {
                return ExportExp(es as BinaryExpressionSyntax);
            }
            //else if (es is PostfixUnaryExpressionSyntax)
            //{
            //    return ExportExp(es as PostfixUnaryExpressionSyntax);
            //}
            //else if (es is ArrayCreationExpressionSyntax)
            //{
            //    return ExportExp(es as ArrayCreationExpressionSyntax);
            //}
            //else if (es is PrefixUnaryExpressionSyntax)
            //{
            //    return ExportExp(es as PrefixUnaryExpressionSyntax);
            //}
            //else if (es is BaseExpressionSyntax)
            //{
            //    return ExportExp(es as BaseExpressionSyntax);
            //}
            //else if (es is ThrowExpressionSyntax)
            //{
            //    return ExportExp(es as ThrowExpressionSyntax);
            //}
            //else if (es is ParenthesizedExpressionSyntax)
            //{
            //    return ExportExp(es as ParenthesizedExpressionSyntax);
            //}
            //else if (es is ElementAccessExpressionSyntax)
            //{
            //    return ExportExp(es as ElementAccessExpressionSyntax);
            //}
            else
            {
                Console.Error.WriteLine(string.Format("error:不支持的表达式 {0} {1}", es.GetType().Name, es.ToString()));
            }
            return null;
        }

        ULNode ExportExp(LiteralExpressionSyntax e)
        {
            ULNode node = ULNode.NewControlNode(ULNode.name_const);
            graph.Nodes.Add(node);
            node.Outputs[0].Value = (e.Token.Text);
            if(e.Token.Text.StartsWith("\""))
            {
                node.Outputs[0].TypeID = "System.String";
            }
            else
            {
                node.Outputs[0].TypeID = "System.Int32";
            }
            return node;
        }
        ULNode ExportExp(ThisExpressionSyntax e)
        {
            ULNode node = ULNode.NewControlNode(ULNode.name_getthis);
            graph.Nodes.Add(node);
            return node;
        }
        ULNode ExportExp(AssignmentExpressionSyntax es)
        {
            var left = ExportExp(es.Left);
            var right = ExportExp(es.Right);

            right.LinkTo(left, 0, 0);

            return right;
        }
        ULNode ExportExp(ObjectCreationExpressionSyntax es)
        {
            ULNode node = new ULNode();
            node.NodeID = Guid.NewGuid().ToString();
            graph.Nodes.Add(node);
            var typeInfo = GetTypeInfo(es.Type);
            node.Name = typeInfo.ID+"."+ typeInfo.Name;

            node.Inputs.Add(new ULPin() { Name = "in" });
            node.Outputs.Add(new ULPin() { Name = "exit" });


            if (es.ArgumentList != null)
            {
                foreach (var a in es.ArgumentList.Arguments)
                {
                    var exp = ExportExp(a.Expression);
                    node.Inputs.Add(new ULPin() { Name = a.NameColon.Name.Identifier.Text });
                    exp.LinkTo(node, 0, node.Inputs.Count-1);
                }
            }

            return node;
        }

        ULNode ExportExp(InvocationExpressionSyntax es)
        {
            ULNode node = new ULNode();
            node.NodeID = Guid.NewGuid().ToString();
            //node.Type = ULNode.ENodeType.Method;
            graph.Nodes.Add(node);

            //if (es.Expression is MemberAccessExpressionSyntax)
            //{
            //    MemberAccessExpressionSyntax maes = es.Expression as MemberAccessExpressionSyntax;
            //    db_les.Name = (maes).Name.Identifier.Text;
            //    db_les.Caller = ExportExp(maes.Expression);
            //}
            //else if (es.Expression is IdentifierNameSyntax)
            //{
            //    IdentifierNameSyntax nameSyntax = es.Expression as IdentifierNameSyntax;
            //    db_les.Name = nameSyntax.Identifier.Text;
            //    db_les.Caller = new Metadata.Expression.ThisExp();
            //}
            //else
            //{
            //    Console.Error.WriteLine("不支持的方法调用表达式 " + es.ToString());
            //}
            var identifier = (es.Expression as IdentifierNameSyntax).Identifier.Text;
            var idInfo =  GetIdentifierInfo(identifier);
            node.Name = idInfo.Member.ID;
            MakeNodePins(node,idInfo.Member);

            int arg_index = 1;
            foreach (var a in es.ArgumentList.Arguments)
            {
                var exp = ExportExp(a.Expression);
                exp.LinkTo(node, 0, arg_index);
                arg_index++;
            }
            //currentBlock.statements.Add(node);
            return node;
        }

        void MakeNodePins(ULNode node,ULMemberInfo member)
        {
            node.Inputs.Add(new ULPin() { Name = "in" });
            node.Outputs.Add(new ULPin() { Name = "exit" });

            if (member != null)
            {
                for (int i = 0; i < member.Graph.Args.Count; i++)
                {
                    node.Inputs.Add(new ULPin() { TypeID = member.Graph.Args[i].TypeID,  Name = member.Graph.Args[i].Name});
                }
                for (int i = 0; i < member.Graph.Outputs.Count; i++)
                {
                    node.Outputs.Add(new ULPin() { TypeID = member.Graph.Outputs[i].TypeID, Name = member.Graph.Outputs[i].Name });
                }
            }
        }

        ULNode ExportExp(MemberAccessExpressionSyntax es)
        {
            ULNode node = new ULNode();
            node.NodeID = Guid.NewGuid().ToString();
            graph.Nodes.Add(node);

            var exp = ExportExp(es.Expression);
            node.Name = es.Name.Identifier.Text;
            node.Inputs.Add(new ULPin() { Name = "object" });
            exp.LinkTo(node, 0, 0);
            return node;
        }
        ULNode ExportExp(IdentifierNameSyntax es)
        {
            ULNode node = new ULNode();
            node.NodeID = Guid.NewGuid().ToString();
            graph.Nodes.Add(node);
            var id = GetIdentifierInfo(es.Identifier.Text);
            node.Name = es.Identifier.Text;
            node.Outputs.Add(new ULPin() { Name = "Indentfier", Value = es.Identifier.Text,TypeID = id.TypeID });

            return node;
        }

        string GetBinaryOperatorTokenMethodName(string token)
        {
            switch (token)
            {
                case "+":
                    return "op_Add";
                case "-":
                    return "op_Sub";
                case "*":
                    return "op_Mul";
                case "/":
                    return "op_Div";
                default:
                    return "error";
            }
        }
        string GetPostfixOperatorTokenMethodName(string token)
        {
            switch (token)
            {
                case "++":
                    return "op_AddAddPost";
                case "--":
                    return "op_SubSubPost";
                default:
                    return "error";
            }
        }
        string GetPrefixOperatorTokenMethodName(string token)
        {
            switch (token)
            {
                case "++":
                    return "op_AddAdd";
                case "--":
                    return "op_SubSub";
                default:
                    return "error";
            }
        }

        ULNode ExportExp(BinaryExpressionSyntax es)
        {
            ULNode node = new ULNode();
            node.NodeID = Guid.NewGuid().ToString();
            graph.Nodes.Add(node);



            node.Name = GetBinaryOperatorTokenMethodName(es.OperatorToken.Text);

            var Left = ExportExp(es.Left);
            var Right = ExportExp(es.Right);

            

            node.Inputs.Add(new ULPin() { TypeID = Left.Outputs[0].TypeID, Name = "Left" });
            node.Inputs.Add(new ULPin() { TypeID = Right.Outputs[0].TypeID, Name ="Right" });
            node.Outputs.Add(new ULPin() { TypeID = Right.Outputs[0].TypeID, Name = "Result" });


            Left.LinkTo(node, 0, 0);
            Right.LinkTo(node, 0, 1);

            return node;
        }

        //ULCall ExportExp(PostfixUnaryExpressionSyntax es)
        //{

        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Method;

        //    node.Name = GetPostfixOperatorTokenMethodName(es.OperatorToken.Text);

        //    var Left = ExportExp(es.Operand);

        //    node.Args.Add(Left.GetOutputName(0));


        //    //currentBlock.statements.Add(node);


        //    return node;
        //}

        //ULCall ExportExp(PrefixUnaryExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Method;

        //    node.Name = GetPrefixOperatorTokenMethodName(es.OperatorToken.Text);

        //    var Left = ExportExp(es.Operand);

        //    node.Args.Add(Left.GetOutputName(0));


        //    //currentBlock.statements.Add(node);


        //    return node;
        //}

        //ULCall ExportExp(ArrayCreationExpressionSyntax es)
        //{
        //    var db_les = new ULCall();
        //    db_les.Parent = currentBlock;
        //    db_les.callType = ULCall.ECallType.CreateArray;

        //    db_les.Args.Add(GetType(es.Type).FullName);
        //    foreach (var p in es.Type.RankSpecifiers)
        //    {
        //        foreach (var s in p.Sizes)
        //            db_les.Args.Add(ExportExp(s).GetOutputName(0));
        //    }

        //    //currentBlock.statements.Add(db_les);

        //    return db_les;
        //}

        //ULCall ExportExp(BaseExpressionSyntax es)
        //{
        //    var db_les = new ULCall();
        //    db_les.Parent = currentBlock;
        //    db_les.callType = ULCall.ECallType.GetBase;
        //    //currentBlock.statements.Add(db_les);
        //    return db_les;
        //}

        //ULCall ExportExp(ElementAccessExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.ElementAccess;

        //    var exp = ExportExp(es.Expression);
        //    node.Args.Add(exp.GetOutputName(0));
        //    foreach (var a in es.ArgumentList.Arguments)
        //    {
        //        node.Args.Add(ExportExp(a.Expression).GetOutputName(0));
        //    }

        //    return node;
        //}

        //ULCall ExportExp(ParenthesizedExpressionSyntax es)
        //{
        //    return ExportExp(es.Expression);
        //}
    }
}
