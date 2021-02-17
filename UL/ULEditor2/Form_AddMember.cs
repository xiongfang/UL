using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ULEditor2
{
    public partial class Form_AddMember : Form
    {
        public Form_AddMember()
        {
            InitializeComponent();
        }
        public string InputName;
        public string InputTypeID;

        private void button_OK_Click(object sender, EventArgs e)
        {
            InputName = textBox1.Text;
            InputTypeID = textBox2.Text;
        }
    }
}
