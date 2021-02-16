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
        static Core.ExcelReader excelReader = null;

        static List<T> LoadDataList<T>(string fileName) where T : new()
        {
            return excelReader.LoadDataList<T>(fileName);
        }

        static Dictionary<K, T> LoadDataDic<K, T>(string fileName) where T : new()
        {
            return excelReader.LoadDataDic<K, T>(fileName);
        }

        static void BeginLoadData(string path)
        {
            excelReader = Core.ExcelReader.LoadFromExcel(path);
        }

        static void EndLoadData()
        {
            excelReader = null;
        }

        public static void LoadData()
        {
            var app_path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            app_path = System.IO.Path.GetDirectoryName(app_path);
            var file_dir = System.IO.Path.Combine(app_path, "..", "..", "..", "..", "..", "Documents");
            var filePath = System.IO.Path.Combine(file_dir, "System.xlsx");
            BeginLoadData(filePath);
            Data.types.Clear();
            var type_list = LoadDataList<ULTypeInfo>("Type");
            foreach(var t in type_list)
            {
                Data.types.Add(t.ID, t);
            }
            Data.members.Clear();
            var member_list = LoadDataList<ULMemberInfo>("Member");
            foreach (var t in member_list)
            {
                Data.members.Add(t.ID, t);
            }

            EndLoadData();


            var nodeFilePath = System.IO.Path.Combine(file_dir, "node.json");
            try
            {
                var graphs = Core.JSON.ToObject<List<ULGraph>>(System.IO.File.ReadAllText(nodeFilePath, Encoding.UTF8));
                foreach(var g in graphs)
                {
                    Data.graphics.Add(g.MethodID, g);
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
            var nodeFilePath = System.IO.Path.Combine(file_dir, "node.json");

            try
            {
                System.IO.File.WriteAllText(nodeFilePath, Core.JSON.ToJSON(Data.graphics.Values.ToList()), Encoding.UTF8);
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

            treeViewTypes.BeginUpdate();
            foreach (var t in Data.types.Values)
            {
                TreeNode nsNode = null;
                var ns = treeViewTypes.Nodes.Find(t.Namespace,false);
                if (ns.Length > 0)
                    nsNode = ns[0];
                if(nsNode == null)
                {
                    nsNode = treeViewTypes.Nodes.Add(t.Namespace, t.Namespace);
                }

                var typeNode = nsNode.Nodes.Add(t.Name, t.Name);
                typeNode.Tag = t;

                foreach(var m in t.Members)
                {
                    var memberNode = typeNode.Nodes.Add(m.Name, m.Name);
                    memberNode.Tag = m;
                }
            }

            treeViewTypes.EndUpdate();

            graphEditor1.onSelectNodeChanged += (node) =>
                {
                    if(node!=null)
                        propertyGrid1.SelectedObject = node;
                };
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
    }
}
