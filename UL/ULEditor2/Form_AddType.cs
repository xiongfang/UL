using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ULEditor2
{
    public partial class Form_AddType : Form
    {
        public Form_AddType()
        {
            InitializeComponent();
        }
        public string InputName;
        public string InputNamespace;
        private void button_OK_Click(object sender, EventArgs e)
        {
            InputName = textBox1.Text;
            InputNamespace = textBox2.Text;
        }
    }
}
