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
            Data.types = LoadDataDic<string, ULTypeInfo>("Type");
            Data.members = LoadDataDic<string, ULMemberInfo>("Member");
            
            EndLoadData();


            var nodeFilePath = System.IO.Path.Combine(file_dir, "node.json");
            try
            {
                Data.nodes = Core.JSON.ToObject<List<ULNode>>(System.IO.File.ReadAllText(nodeFilePath, Encoding.UTF8));
            }
            catch(Exception e)
            {
                Data.nodes = new List<ULNode>();
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
                var nodes = Data.nodes.Where((v) => v!=null && v.MethodID!=null && Data.members.ContainsKey(v.MethodID)).ToList();
                System.IO.File.WriteAllText(nodeFilePath, Core.JSON.ToJSON(nodes), Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion


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
        }

        private void treeViewTypes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;
            if(e.Node.Tag is ULMemberInfo)
            {
                graphEditor1.memberInfo = e.Node.Tag as ULMemberInfo;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
        }
    }
}
