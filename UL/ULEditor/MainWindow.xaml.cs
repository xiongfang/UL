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
    public class TreeViewWithIcons : TreeViewItem
    {
        protected ImageSource iconSource;
        protected TextBlock textBlock;
        protected Image icon;

        public TreeViewWithIcons()
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
    class TypeNodeItem: TreeViewWithIcons
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

            foreach (var t in Model.ModelData.Types)
            {
                AddType(t.Value);

            }
        }


        T FindChild<T>(ItemCollection p,System.Func<T,bool> condition=null,bool children=false) where T: TreeViewWithIcons
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
        T[] FindChildren<T>(ItemCollection p, System.Func<T, bool> condition = null, bool children = false) where T : TreeViewWithIcons
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
        void AddType(Model.ULTypeInfo type)
        {
            var ns_list = type.Namespace.Split('.');
            ItemCollection lastCollection = treeView.Items;

            TreeViewWithIcons parentNode = null;

            foreach (var n in ns_list)
            {
                parentNode = FindChild<TreeViewWithIcons>(lastCollection,(v)=>v.HeaderText == n);

                if (parentNode == null)
                {
                    parentNode = new TreeViewWithIcons();
                    parentNode.HeaderText = n;
                    lastCollection.Add(parentNode);
                    
                }
                lastCollection = parentNode.Items;
            }


            var classNode = new TypeNodeItem(type);
            var bm = new BitmapImage(new Uri("Images/type.png", UriKind.RelativeOrAbsolute));
            classNode.Icon = bm;
            parentNode.Items.Add(classNode);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Model.ModelData.Save();
        }

        private void OnClick_AddType(object sender, RoutedEventArgs e)
        {
            
            var typeNode = treeView.SelectedItem as TypeNodeItem;
            if(typeNode!=null)
            {
                var t = typeNode.type;
                var m = new Model.ULMemberInfo();
                m.ReflectTypeId = t.Guid;
                m.ExportType = Model.EExportType.Public;
                m.IsStatic = false;
                m.Name = "NewMember";
                t.Members[t.Name] = m;
                return;
            }
            var nsNode = treeView.SelectedItem as TreeViewWithIcons;
            if(nsNode!=null)
            {
                var t = new Model.ULTypeInfo();
                t.Namespace = GetNamespace(nsNode);
                t.Name = "NewClass";
                t.SetGUID(Guid.NewGuid().ToString());
                Model.ModelData.Types.Add(t.Guid, t);
                AddType(t);
            }
            else
            {
                var t = new Model.ULTypeInfo();
                t.Namespace = "gloable";
                t.Name = "NewClass";
                t.SetGUID(Guid.NewGuid().ToString());
                Model.ModelData.Types.Add(t.Guid, t);
                AddType(t);
            }
        }

        string GetNamespace(TreeViewWithIcons vv)
        {
            List<string> ns_list = new List<string>();
            while(vv!=null)
            {
                ns_list.Add(vv.HeaderText);
                vv = vv.Parent as TreeViewWithIcons;
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
                    var ms = r.Matches(textRun);
                    foreach(System.Text.RegularExpressions.Match m in ms)
                    {
                        var position_start = position.GetPositionAtOffset(m.Index);
                        var position_end = position.GetPositionAtOffset(m.Index + m.Length);
                        TextRange tr = new TextRange(position_start, position_end);
                        tr.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
                    }

                    position = position.GetNextContextPosition(LogicalDirection.Forward);
                    
                }
                else
                {
                    position = position.GetNextContextPosition(LogicalDirection.Forward);
                }
            }
        }

        private void TreeView_OnSelectedChange(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var typeNode = treeView.SelectedItem as TypeNodeItem;
            if (typeNode != null)
            {
                this.propertyGrid.SelectedObject = typeNode.type;
   
                switch ( tabControl.SelectedIndex)
                {
                    case 0:
                        cs_richTextBox.Document.Blocks.Clear();
                        Paragraph paragraph = new Paragraph();
                        paragraph.Inlines.Add(new Run() { Text = Model.ULToCS.To(typeNode.type) });
                        cs_richTextBox.Document.Blocks.Add(paragraph);
                        foreach(var k in Model.ULToCS.keywords)
                            FindWordFromPosition(cs_richTextBox.Document.ContentStart, k);
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
            }
        }

        private void propertyGrid_PropertyValueChanged(object sender, Xceed.Wpf.Toolkit.PropertyGrid.PropertyValueChangedEventArgs e)
        {
            TreeView_OnSelectedChange(null,null);
            (treeView.SelectedItem as TypeNodeItem).Refresh();
        }
    }
}
