using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;

namespace ULEditor2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region 加载

        public static void LoadData()
        {
            var app_path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            app_path = System.IO.Path.GetDirectoryName(app_path);
            var file_dir = System.IO.Path.Combine(app_path, "..", "..", "..", "..", "..", "Documents");

            try
            {

                var nodeFilePath = System.IO.Path.Combine(file_dir, "data.json");
                var types = Core.JSON.ToObject<List<ULTypeInfo>>(System.IO.File.ReadAllText(nodeFilePath, Encoding.UTF8));
                foreach (var g in types)
                {
                    Data.types[g.ID] = g;
                }
            }
            catch(Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        static void SaveData()
        {
            var app_path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            app_path = System.IO.Path.GetDirectoryName(app_path);
            var file_dir = System.IO.Path.Combine(app_path, "..", "..", "..", "..", "..", "Documents");
            var nodeFilePath = System.IO.Path.Combine(file_dir, "data.json");

            try
            {
                System.IO.File.WriteAllText(nodeFilePath, Core.JSON.ToJSON(Data.types.Values.ToList()), Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        ULTypeInfo _selectedType;

        private void Form1_Load(object sender, EventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            LoadData();
            ResetView();


            graphEditor1.onSelectNodeChanged += (node) =>
                {
                    if(node!=null)
                        propertyGrid1.SelectedObject = node;
                };

            tsmi_Load.Click += MenuItemClicked;
            tsmi_Save.Click += MenuItemClicked;
        }

        void ResetView()
        {
            treeViewTypes.BeginUpdate();
            treeViewTypes.Nodes.Clear();
            
            foreach (var t in Data.types.Values)
            {
                TreeNode nsNode = null;
                var ns = treeViewTypes.Nodes.Find(t.Namespace, false);
                if (ns.Length > 0)
                    nsNode = ns[0];
                if (nsNode == null)
                {
                    nsNode = treeViewTypes.Nodes.Add(t.Namespace, t.Namespace);
                }

                var typeNode = nsNode.Nodes.Add(t.ID, t.Name);
                typeNode.Tag = t;

                foreach (var m in t.Members)
                {
                    var memberNode = typeNode.Nodes.Add(m.ID, m.Name);
                    memberNode.Tag = m;
                }
            }

            treeViewTypes.EndUpdate();
        }

        private void treeViewTypes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;

            if (e.Node.Tag is ULTypeInfo)
            {
                _selectedType = e.Node.Tag as ULTypeInfo;
            }
            else if (e.Node.Tag is ULMemberInfo)
            {
                _selectedType = Data.GetType((e.Node.Tag as ULMemberInfo).DeclareTypeID);
            }

            if (tabControl1.SelectedIndex == 0)
            {
                if (e.Node.Tag is ULMemberInfo)
                {
                    graphEditor1.memberInfo = e.Node.Tag as ULMemberInfo;
                }
            }
            else if(tabControl1.SelectedIndex == 1)
            {
                UpdateCSCode();
            }

            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
        }

        void UpdateCSCode()
        {
            if(_selectedType!=null)
            {
                rtb_CSharpCode.Clear();
                rtb_CSharpCode.SelectionColor = Color.Black;
                rtb_CSharpCode.Text = ULToCS.To(_selectedType);
                
                foreach(var key in ULToCS.keywords)
                {
                    int start = 0;
                    do
                    {
                        start = rtb_CSharpCode.Find(key, start, RichTextBoxFinds.WholeWord);
                        if(start>=0)
                        {
                            rtb_CSharpCode.Select(start, key.Length);
                            rtb_CSharpCode.SelectionColor = Color.Blue;
                            start++;
                        }
                    } while (start >= 0);
                    
                }
            }
                
            
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //c#
            if(tabControl1.SelectedIndex == 1)
            {
                UpdateCSCode();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem == tsb_Compile)
            {
                var r = CSToUL.Convert(rtb_CSharpCode.Text);
                foreach (var g in r.GetChildrenTypes(r))
                {
                    Data.types[g.ID] = g;
                }
                ResetView();
                if(_selectedType!=null)
                {
                    _selectedType = Data.GetType(_selectedType.ID);
                    UpdateCSCode();
                    var nodes = treeViewTypes.Nodes.Find(_selectedType.ID, true);
                    if (nodes.Length > 0)
                    {
                        treeViewTypes.SelectedNode = nodes[0];
                        treeViewTypes.SelectedNode.Expand();
                    }
                       
                }
                    
            }
        }

        void MenuItemClicked(object sender, EventArgs e)
        {
            if(sender == tsmi_Load)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "JSON文件(*.json)|*.json";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var types = Core.JSON.ToObject<List<ULTypeInfo>>(System.IO.File.ReadAllText(dialog.FileName, Encoding.UTF8));
                        foreach (var g in types)
                        {
                            Data.types[g.ID] = g;
                        }

                        ResetView();
                    }
                    catch (Exception excep)
                    {
                        Console.Error.WriteLine(excep.Message);
                    }
                }
            }
            else if(sender == tsmi_Save)
            {

            }
        }

        private void contextMenuStrip_TreeView_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem == tsmi_AddType)
            {
                var form = new Form_AddType();
                if(form.ShowDialog() == DialogResult.OK)
                {
                    var type = new ULTypeInfo();
                    type.Name = form.InputName;
                    type.Namespace = form.InputNamespace;
                    if (Data.types.ContainsKey(type.ID))
                    {
                        return;
                    }
                    Data.types[type.ID] = type;
                    ResetView();
                }

                
            }
            else if(e.ClickedItem == tsmi_AddMember && _selectedType!=null)
            {
                var form = new Form_AddMember();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var member = new ULMemberInfo();
                    member.Name = form.InputName;
                    member.TypeID = form.InputTypeID;
                    member.DeclareTypeID = _selectedType.ID;
                    _selectedType.Members.Add(member);
                    ResetView();
                }
            }
            else if(e.ClickedItem == tsmi_Delete)
            {
                if(treeViewTypes.SelectedNode!=null)
                {
                    var selectedType = treeViewTypes.SelectedNode.Tag as ULTypeInfo;
                    if(selectedType!=null)
                    {
                        Data.types.Remove(selectedType.ID);
                        treeViewTypes.Nodes.Remove(treeViewTypes.SelectedNode);
                    }
                    else
                    {
                        var selectMethod = treeViewTypes.SelectedNode.Tag as ULMemberInfo;
                        if(selectMethod!=null)
                        {
                            Data.GetType(selectMethod.DeclareTypeID).Members.Remove(selectMethod);
                            treeViewTypes.Nodes.Remove(treeViewTypes.SelectedNode);
                        }
                    }
                }
            }
            else if(e.ClickedItem == tsmi_Refresh)
            {
                ResetView();
            }
        }
    }
}
