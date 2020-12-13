using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace WpfApp1
{
    public class TreeNode : TreeViewItem
    {
        protected ImageSource iconSource;
        protected TextBlock textBlock;
        protected Image icon;

        public TreeNode()
        {
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            Header = stack;
            //Uncomment this code If you want to add an Image after the Node-HeaderText
            //textBlock = new TextBlock();
            //textBlock.VerticalAlignment = VerticalAlignment.Center;
            //stack.Children.Add(textBlock);
            icon = new Image();
            icon.VerticalAlignment = VerticalAlignment.Center;
            icon.Height = 10;
            icon.Width = 10;
            icon.Margin = new Thickness(0, 0, 4, 0);
            icon.Source = iconSource;
            stack.Children.Add(icon);
            //Add the HeaderText After Adding the icon
            textBlock = new TextBlock();
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            stack.Children.Add(textBlock);
        }

        public ImageSource Icon
        {
            set
            {
                iconSource = value;
                icon.Source = iconSource;
            }
            get
            {
                return iconSource;
            }
        }

        protected override void OnUnselected(RoutedEventArgs args)
        {
            base.OnUnselected(args);
            icon.Source = iconSource;
        }

        protected override void OnSelected(RoutedEventArgs args)
        {
            base.OnSelected(args);
            icon.Source = iconSource;
        }

        /// <summary>
        /// Gets/Sets the HeaderText of TreeViewWithIcons
        /// </summary>
        public string HeaderText
        {
            get
            {
                return textBlock.Text;
            }
            set
            {
                textBlock.Text = value;
            }
        }

    }
    class TypeNodeItem: TreeNode
    {
        public Model.ULTypeInfo type;
        public TypeNodeItem(Model.ULTypeInfo t) { type = t; textBlock.Text = type.Name; }

        public override void EndInit()
        {
            base.EndInit();
            textBlock.Text = type.Name;
        }

        public void Refresh()
        {
            textBlock.Text = type.Name;
        }
    }
    class MemberNodeItem : TreeNode
    {
        public Model.ULMemberInfo member;
        public MemberNodeItem(Model.ULMemberInfo t) { member = t; textBlock.Text = member.Name; }

        public override void EndInit()
        {
            base.EndInit();
            textBlock.Text = member.Name;
        }

        public void Refresh()
        {
            if(member.Name!=textBlock.Text)
            {
                textBlock.Text = member.Name;

            }
            
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Model.ModelData.Load();

            foreach (var t in Model.ModelData.GetTypeList())
            {
                AddTypeNode(t);
            }

            Model.ModelData.onAddType += (t) =>
            {
                AddTypeNode(t);
            };
            Model.ModelData.onRemoveType += (t) =>
            {
                var node = FindTypeNode(t);
                if (node != null)
                {
                    (node.Parent as TreeNode).Items.Remove(node);
                }
            };
        }

        TypeNodeItem FindTypeNode(Model.ULTypeInfo type)
        {
            return FindChild<TypeNodeItem>(treeView.Items, (v) => { return v.type.FullName == type.FullName; }, true);
        }


        T FindChild<T>(ItemCollection p,System.Func<T,bool> condition=null,bool children=false) where T: TreeNode
        {
            foreach(var n in p)
            {
                var t = n as T;
                if (t != null && (condition == null || condition(t)))
                {
                    return t;
                }

                if (children)
                {
                    var c = FindChild<T>((n as TreeViewItem).Items, condition, children);
                    if (c != null)
                        return c;
                }
            }
            return null;
        }
        T[] FindChildren<T>(ItemCollection p, System.Func<T, bool> condition = null, bool children = false) where T : TreeNode
        {
            List<T> cs = new List<T>();
            foreach (var n in p)
            {
                var t = n as T;

                if (t !=null && ( condition ==null || condition(t)))
                {
                    cs.Add(t);
                }

                if (children)
                {
                    var c = FindChildren<T>((n as TreeViewItem).Items, condition, children);
                    if (c != null)
                    {
                        cs.AddRange(c);
                    }
                }
            }
            return cs.ToArray();
        }
        void AddTypeNode(Model.ULTypeInfo type)
        {
            var ns_list = type.Namespace.Split('.');
            ItemCollection lastCollection = treeView.Items;

            TreeNode parentNode = null;

            foreach (var n in ns_list)
            {
                parentNode = FindChild<TreeNode>(lastCollection,(v)=>v.HeaderText == n);

                if (parentNode == null)
                {
                    parentNode = new TreeNode();
                    parentNode.HeaderText = n;
                    lastCollection.Add(parentNode);
                    
                }
                lastCollection = parentNode.Items;
            }


            var classNode = new TypeNodeItem(type);
            var bm = new BitmapImage(new Uri("Images/type.png", UriKind.RelativeOrAbsolute));
            classNode.Icon = bm;
            parentNode.Items.Add(classNode);

            foreach(var m in type.Members)
            {
                AddMember(classNode, m);
            }
        }

        void UpdateTypeNode(Model.ULTypeInfo type)
        {
            var node = FindChild<TypeNodeItem>(treeView.Items, (v) => { return v.type.FullName == type.FullName; }, true);
            if(node!=null)
            {
                node.type = type;
                node.Items.Clear();
                foreach (var m in type.Members)
                {
                    AddMember(node, m);
                }
                node.Refresh();
            }
           
        }

        void AddMember(TypeNodeItem node,Model.ULMemberInfo memberInfo)
        {
            var classNode = new MemberNodeItem(memberInfo);
            var bm = new BitmapImage(new Uri("Images/func.png", UriKind.RelativeOrAbsolute));
            classNode.Icon = bm;
            node.Items.Add(classNode);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Model.ModelData.Save();
        }

        private void OnClick_TreeNodeAdd(object sender, RoutedEventArgs e)
        {
            
            var typeNode = treeView.SelectedItem as TypeNodeItem;
            if(typeNode!=null)
            {
                var t = typeNode.type;
                var m = new Model.ULMemberInfo();
                m.ReflectTypeName = t.FullName;
                m.ExportType = Model.EExportScope.Public;
                m.IsStatic = false;
                m.Name = "NewMember";
                //m.SetGuid(t.Guid + "-" + t.Methods.Count);
                t.Members.Add(m);
                AddMember(typeNode, m);
                return;
            }
            var nsNode = treeView.SelectedItem as TreeNode;
            if(nsNode!=null)
            {
                var t = new Model.ULTypeInfo();
                t.Namespace = GetNamespace(nsNode);
                t.Name = "NewClass";
                Model.ModelData.AddType(t);
                //AddTypeNode(t);
            }
            else
            {
                var t = new Model.ULTypeInfo();
                t.Namespace = Model.ModelData.GloableNamespaceName;
                t.Name = "NewClass";
                Model.ModelData.AddType(t);
                //AddTypeNode(t);
            }
        }

        private void OnClick_TreeNodeDelete(object sender, RoutedEventArgs e)
        {
            {
                var typeNode = treeView.SelectedItem as TypeNodeItem;
                if (typeNode != null)
                {
                    Model.ModelData.RemoveType(typeNode.type.FullName);
                }
            }

            {
                var memberNode = treeView.SelectedItem as MemberNodeItem;
                if (memberNode != null)
                {
                    if(memberNode.member.ReflectType!=null)
                    {
                        memberNode.member.ReflectType.Members.Remove(memberNode.member);
                    }
                    else
                    {
                        (memberNode.Parent as TypeNodeItem).type.Members.Remove(memberNode.member);
                    }
                    
                    (memberNode.Parent as TreeNode).Items.Remove(memberNode);
                }
            }

            
        }

        string GetNamespace(TreeNode vv)
        {
            List<string> ns_list = new List<string>();
            while(vv!=null)
            {
                ns_list.Add(vv.HeaderText);
                vv = vv.Parent as TreeNode;
            }
            ns_list.Reverse();
            StringBuilder sb = new StringBuilder();
            for(int i=0;i<ns_list.Count;i++)
            {
                sb.Append(ns_list[i]);
                if(i+1<ns_list.Count)
                {
                    sb.Append(".");
                }
            }
            return sb.ToString();
        }

        void FindWordFromPosition(TextPointer position, string pattern)
        {
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);

                    // Find the starting index of any substring that matches "word".

                    var r = new System.Text.RegularExpressions.Regex(pattern);
                    System.Text.RegularExpressions.Match m = r.Match(textRun);

                    TextPointer textPointerEnd = position;
                    {
                        var position_start = position.GetPositionAtOffset(m.Index);
                        textPointerEnd = position.GetPositionAtOffset(m.Index + m.Length);
                        TextRange tr = new TextRange(position_start, textPointerEnd);
                        tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);

                    }
                }
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        string GetAllText(TextPointer position)
        {
            StringBuilder sb = new StringBuilder();
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);

                    sb.Append(textRun);
                }
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            return sb.ToString();
        }

        private void TreeView_OnSelectedChange(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var typeNode = treeView.SelectedItem as TypeNodeItem;
            if (typeNode != null)
            {
                this.propertyGrid.SelectedObject = typeNode.type;
                UpdateCode(typeNode.type);
                return;
            }

            var memberNode = treeView.SelectedItem as MemberNodeItem;
            if(memberNode!=null)
            {
                this.propertyGrid.SelectedObject = memberNode.member;
                UpdateCode(memberNode.member.ReflectType);
            }
            
        }

        void UpdateCode(Model.ULTypeInfo typeInfo)
        {
            switch (tabControl.SelectedIndex)
            {
                case 0:
                    {
                        cs_richTextBox.Document.Blocks.Clear();
                        Paragraph paragraph = new Paragraph();
                        paragraph.Inlines.Add(new Run() { Text = Model.ULToCS.To(typeInfo) });
                        cs_richTextBox.Document.Blocks.Add(paragraph);
                        foreach (var k in Model.ULToCS.keywords)
                            FindWordFromPosition(cs_richTextBox.Document.ContentStart, k);
                    }

                    break;
                case 1:
                    {
                        lua_richTextBox.Document.Blocks.Clear();
                        Paragraph paragraph = new Paragraph();
                        paragraph.Inlines.Add(new Run() { Text = Model.ULToLua.To(typeInfo) });
                        lua_richTextBox.Document.Blocks.Add(paragraph);
                        foreach (var k in Model.ULToLua.keywords)
                            FindWordFromPosition(lua_richTextBox.Document.ContentStart, k);
                    }
                    break;
                case 2:
                    {
                        cpp_richTextBox.Document.Blocks.Clear();
                        Paragraph paragraph = new Paragraph();
                        paragraph.Inlines.Add(new Run() { Text = Model.ULToCpp.To(typeInfo) });
                        cpp_richTextBox.Document.Blocks.Add(paragraph);
                        //foreach (var k in Model.ULToCpp.keywords)
                        //    FindWordFromPosition(cpp_richTextBox.Document.ContentStart, k);
                    }
                    break;
                case 3:
                    break;
            }
        }

        private void propertyGrid_PropertyValueChanged(object sender, Xceed.Wpf.Toolkit.PropertyGrid.PropertyValueChangedEventArgs e)
        {
            TreeView_OnSelectedChange(null,null);
            if(treeView.SelectedItem is TypeNodeItem)
                (treeView.SelectedItem as TypeNodeItem).Refresh();

            if(treeView.SelectedItem is MemberNodeItem)
                (treeView.SelectedItem as MemberNodeItem).Refresh();
        }

        private void btnCompile_Click(object sender, RoutedEventArgs e)
        {
            if(tabControl.SelectedIndex == 0)
            {
                var type_list =Model.CSToUL.Convert(GetAllText(cs_richTextBox.Document.ContentStart));
                foreach(var t in type_list)
                {
                    if (Model.ModelData.FindTypeByFullName(t.FullName)!=null)
                    {
                        Model.ModelData.UpdateType(t);
                        //FindTypeNode(t);
                        //var typeNode = treeView.SelectedItem as TypeNodeItem;
                        //if(typeNode!=null && typeNode.type.FullName == t.FullName)
                        //{
                        //    typeNode.type = t;
                        //    UpdateTypeNode(t);
                        TreeView_OnSelectedChange(null, null);
                        //}
                        //else
                        //{

                        //    UpdateTypeNode(t);
                        //}
                    }
                    else
                    {
                        Model.ModelData.AddType(t);
                    }
                }
                
            }
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var typeNode = treeView.SelectedItem as TypeNodeItem;
            if (typeNode != null)
            {
                this.propertyGrid.SelectedObject = typeNode.type;
                UpdateCode(typeNode.type);
                return;
            }
        }
    }
}
