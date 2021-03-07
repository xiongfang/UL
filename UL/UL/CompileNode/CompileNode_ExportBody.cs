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

        class PinOut
        {
            public ULNode node;
            public int index;
        }
        //public class NodeBlock
        //{
        //    ULNode parentNode;
        //    public int pinOutIndex;
        //    public NodeBlock(ULNode p, int index) { parentNode = p; pinOutIndex = index; }
            
        //    public void AddNode(ULNode node)
        //    {
        //        if (nodes.Count > 0)
        //        {
        //            nodes[nodes.Count - 1].LinkControlTo(node);
        //        }
        //        else
        //        {
        //            if (parentNode != null)
        //            {
        //                parentNode.LinkControlTo(node, pinOutIndex, 0);
        //            }
        //        }
        //        nodes.Add(node);
        //    }
        //}

        //Stack<NodeBlock> blocks = new Stack<NodeBlock>();
        //NodeBlock currentBlock { get { return blocks.Peek(); } }


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
            //else if (node is LocalDeclarationStatementSyntax)
            //{
            //    ExportStatement(node as LocalDeclarationStatementSyntax);
            //}
            //else if (node is ForStatementSyntax)
            //{
            //    ExportStatement(node as ForStatementSyntax);
            //}
            //else if (node is DoStatementSyntax)
            //{
            //    ExportStatement(node as DoStatementSyntax);
            //}
            else if (node is WhileStatementSyntax)
            {
                ExportStatement(node as WhileStatementSyntax);
            }
            //else if (node is SwitchStatementSyntax)
            //{
            //    ExportStatement(node as SwitchStatementSyntax);
            //}
            //else if (node is BreakStatementSyntax)
            //{
            //    ExportStatement(node as BreakStatementSyntax);
            //}
            //else if (node is ReturnStatementSyntax)
            //{
            //    ExportStatement(node as ReturnStatementSyntax);
            //}
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
            cond.LinkTo(nodeIf, 1, 0);

            if (node.Statement != null)
            {
                var node_true = ExportStatement(node.Statement);
                nodeIf.LinkTo(node_true, 1, 0);
            }
            if (node.Else.Statement != null)
            {
                var node_false = ExportStatement(node.Else.Statement);
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

        void ExportStatement(LocalDeclarationStatementSyntax ss)
        {
            //var Type = GetType(ss.Declaration.Type);
            //foreach (var v in ss.Declaration.Variables)
            //{
            //    var vName = v.Identifier.Text;
            //    frames.Peek().variables[vName] = Type.FullName;
            //    if (v.Initializer != null)
            //    {
            //        ULCall node = new ULCall();
            //        node.Parent = currentBlock;
            //        node.callType = ULCall.ECallType.Assign;
            //        node.Args.Add("local." + vName);
            //        node.Args.Add(ExportExp(v.Initializer.Value).GetOutputName(0));
            //        currentBlock.statements.Add(node);
            //    }
            //}
        }

        void ExportStatement(ForStatementSyntax ss)
        {
            //var db_ss = new ULStatementFor();
            //db_ss.Condition = ExportExp(ss.Condition).GetOutputName(0);
            //db_ss.Declaration = ExportExp(ss.Declaration).GetOutputName(0);
            //foreach (var inc in ss.Incrementors)
            //{
            //    db_ss.Incrementors.Add(ExportExp(inc).GetOutputName(0));
            //}
            //db_ss.block = ExportStatement(ss.Statement as BlockSyntax);
        }

        void ExportStatement(DoStatementSyntax ss)
        {
            //var cond = ExportExp(ss.Condition);

            //var ifStatement = new ULStatementWhile();
            //ifStatement.Parent = currentBlock;
            //ifStatement.arg = cond.GetOutputName(0);
            //if (ss.Statement is BlockSyntax)
            //    ifStatement.block = ExportStatement(ss.Statement as BlockSyntax);
            //currentBlock.statements.Add(ifStatement);
        }

        void ExportStatement(BreakStatementSyntax ss)
        {
            //var node = new ULStatementBreak();
            //node.Parent = currentBlock;
            //currentBlock.statements.Add(node);
        }

        void ExportStatement(ReturnStatementSyntax ss)
        {
            //var node = new ULStatementReturn();
            //node.Parent = currentBlock;
            //node.Arg = ExportExp(ss.Expression).GetOutputName(0);
            //currentBlock.statements.Add(node);
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

        //ULNode ExportExp(VariableDeclarationSyntax es)
        //{
        //    var typeName = GetType(es.Type).FullName;
        //    ULCall node = null;
        //    foreach (var v in es.Variables)
        //    {
        //        node = new ULCall();
        //        node.Parent = currentBlock;
        //        node.callType = ULCall.ECallType.DeclarationLocal;
        //        node.Args.Add(typeName);
        //        node.Args.Add(v.Identifier.Text);
        //        if (v.Initializer != null)
        //            node.Args.Add(ExportExp(v.Initializer.Value).GetOutputName(0));
        //    }

        //    return node;
        //}

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
            //else if (es is ObjectCreationExpressionSyntax)
            //{
            //    return ExportExp(es as ObjectCreationExpressionSyntax);
            //}
            else if (es is InvocationExpressionSyntax)
            {
                return ExportExp(es as InvocationExpressionSyntax);
            }
            //else if (es is MemberAccessExpressionSyntax)
            //{
            //    return ExportExp(es as MemberAccessExpressionSyntax);
            //}
            //else if (es is IdentifierNameSyntax)
            //{
            //    return ExportExp(es as IdentifierNameSyntax);
            //}
            //else if (es is AssignmentExpressionSyntax)
            //{
            //    return ExportExp(es as AssignmentExpressionSyntax);
            //}
            //else if (es is BinaryExpressionSyntax)
            //{
            //    return ExportExp(es as BinaryExpressionSyntax);
            //}
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
        //ULCall ExportExp(AssignmentExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Assign;
        //    node.Args.Add(ExportExp(es.Left).GetOutputName(0));
        //    node.Args.Add(ExportExp(es.Right).GetOutputName(0));
        //    //currentBlock.statements.Add(node);
        //    return node;
        //}
        //ULCall ExportExp(ObjectCreationExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Constructor;


        //    if (es.ArgumentList != null)
        //    {
        //        foreach (var a in es.ArgumentList.Arguments)
        //        {
        //            node.Args.Add(ExportExp(a.Expression).GetOutputName(0));
        //        }
        //    }

        //    node.Name = GetType(es.Type).FullName;

        //    //currentBlock.statements.Add(node);
        //    return node;
        //}

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

        //ULCall ExportExp(MemberAccessExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.GetField;

        //    node.Args.Add(ExportExp(es.Expression).GetOutputName(0));
        //    node.Name = es.Name.Identifier.Text;
        //    //currentBlock.statements.Add(node);
        //    return node;
        //}
        //ULCall ExportExp(IdentifierNameSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Identifier;

        //    node.Name = es.Identifier.Text;

        //    //currentBlock.statements.Add(node);
        //    return node;
        //}

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

        //ULCall ExportExp(BinaryExpressionSyntax es)
        //{
        //    ULCall node = new ULCall();
        //    node.Parent = currentBlock;
        //    node.callType = ULCall.ECallType.Method;

        //    node.Name = GetBinaryOperatorTokenMethodName(es.OperatorToken.Text);

        //    var Left = ExportExp(es.Left);
        //    var Right = ExportExp(es.Right);

        //    node.Args.Add(Left.GetOutputName(0));
        //    node.Args.Add(Right.GetOutputName(1));

        //    //currentBlock.statements.Add(node);


        //    return node;
        //}

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
