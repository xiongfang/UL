using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SyntaxLook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        class TreeNodeItem:TreeNode
        {
            public SyntaxNode node;
            public TreeNodeItem(SyntaxNode node)
            {
                this.node = node;
                Text = node.GetType().Name;
            }
            public override string ToString()
            {
                return node.GetType().Name;
            }
        }

        void PrintTree(SyntaxNode sn, TreeNode parentNode)
        {
            //for (int i = 0; i < depth; i++)
            //    Console.Write("\t");
            //Console.WriteLine("{0}", node.GetType().ToString());
            TreeNodeItem treeNodeItem = new TreeNodeItem(sn);
            parentNode.Nodes.Add(treeNodeItem);
            if(sn.ChildNodes().Count()>0)
            {
                foreach (var c in sn.ChildNodes())
                {
                    //PrintTree(c, ++depth);
                    PrintTree(c, treeNodeItem);
                }
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        void UpdateTree(string file)
        {
            if (file != null)
            {
                string code = System.IO.File.ReadAllText(file);
                SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
                treeView1.BeginUpdate();
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(new TreeNode("Root"));
                PrintTree(tree.GetRoot(), treeView1.Nodes[0]);
                treeView1.EndUpdate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                UpdateTree(dialog.FileName);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNodeItem ti = e.Node as TreeNodeItem;
            if(ti!=null)
            {
                textBox1.Text = ti.node.ToFullString();
            }
        }
    }
}
