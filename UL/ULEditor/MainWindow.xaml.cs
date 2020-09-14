using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfApp1
{
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
            var treeViewItem = new TreeViewItem();
            treeViewItem.Header = "System";

            treeViewItem.Items.Add(new TreeViewItem() { Header = "Int32" });
            treeViewItem.Items.Add(new TreeViewItem() { Header = "Boolean" });
            treeViewItem.Items.Add(new TreeViewItem() { Header = "Single" });

            this.treeView.Items.Add(treeViewItem);

        }
    }
}
