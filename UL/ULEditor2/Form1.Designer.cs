
namespace ULEditor2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.treeViewTypes = new System.Windows.Forms.TreeView();
            this.contextMenuStrip_TreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_AddType = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_AddMember = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_Compile = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_Graph = new System.Windows.Forms.TabPage();
            this.graphEditor1 = new ULEditor2.GraphEditor();
            this.tabPage_CSharp = new System.Windows.Forms.TabPage();
            this.rtb_CSharpCode = new System.Windows.Forms.RichTextBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmi_File = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Load = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_About = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_TreeView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_Graph.SuspendLayout();
            this.tabPage_CSharp.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewTypes
            // 
            this.treeViewTypes.ContextMenuStrip = this.contextMenuStrip_TreeView;
            this.treeViewTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTypes.Location = new System.Drawing.Point(0, 0);
            this.treeViewTypes.Name = "treeViewTypes";
            this.treeViewTypes.Size = new System.Drawing.Size(267, 639);
            this.treeViewTypes.TabIndex = 0;
            this.treeViewTypes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTypes_AfterSelect);
            // 
            // contextMenuStrip_TreeView
            // 
            this.contextMenuStrip_TreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_AddType,
            this.tsmi_AddMember,
            this.tsmi_Delete,
            this.tsmi_Refresh});
            this.contextMenuStrip_TreeView.Name = "contextMenuStrip_TreeView";
            this.contextMenuStrip_TreeView.Size = new System.Drawing.Size(151, 92);
            this.contextMenuStrip_TreeView.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_TreeView_ItemClicked);
            // 
            // tsmi_AddType
            // 
            this.tsmi_AddType.Name = "tsmi_AddType";
            this.tsmi_AddType.Size = new System.Drawing.Size(150, 22);
            this.tsmi_AddType.Text = "AddType";
            // 
            // tsmi_AddMember
            // 
            this.tsmi_AddMember.Name = "tsmi_AddMember";
            this.tsmi_AddMember.Size = new System.Drawing.Size(150, 22);
            this.tsmi_AddMember.Text = "AddMember";
            // 
            // tsmi_Delete
            // 
            this.tsmi_Delete.Name = "tsmi_Delete";
            this.tsmi_Delete.Size = new System.Drawing.Size(150, 22);
            this.tsmi_Delete.Text = "Delete";
            // 
            // tsmi_Refresh
            // 
            this.tsmi_Refresh.Name = "tsmi_Refresh";
            this.tsmi_Refresh.Size = new System.Drawing.Size(150, 22);
            this.tsmi_Refresh.Text = "Refresh";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(-1, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewTypes);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1070, 639);
            this.splitContainer1.SplitterDistance = 267;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer2.Size = new System.Drawing.Size(799, 639);
            this.splitContainer2.SplitterDistance = 535;
            this.splitContainer2.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.YellowGreen;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_Compile});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(535, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // tsb_Compile
            // 
            this.tsb_Compile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_Compile.ForeColor = System.Drawing.Color.DodgerBlue;
            this.tsb_Compile.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Compile.Image")));
            this.tsb_Compile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Compile.Name = "tsb_Compile";
            this.tsb_Compile.Size = new System.Drawing.Size(23, 22);
            this.tsb_Compile.Text = "Compile";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage_Graph);
            this.tabControl1.Controls.Add(this.tabPage_CSharp);
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(535, 611);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage_Graph
            // 
            this.tabPage_Graph.Controls.Add(this.graphEditor1);
            this.tabPage_Graph.Location = new System.Drawing.Point(4, 26);
            this.tabPage_Graph.Name = "tabPage_Graph";
            this.tabPage_Graph.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Graph.Size = new System.Drawing.Size(527, 581);
            this.tabPage_Graph.TabIndex = 0;
            this.tabPage_Graph.Text = "蓝图";
            this.tabPage_Graph.UseVisualStyleBackColor = true;
            // 
            // graphEditor1
            // 
            this.graphEditor1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.graphEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphEditor1.Location = new System.Drawing.Point(3, 3);
            this.graphEditor1.memberInfo = null;
            this.graphEditor1.Name = "graphEditor1";
            this.graphEditor1.Size = new System.Drawing.Size(521, 575);
            this.graphEditor1.TabIndex = 0;
            // 
            // tabPage_CSharp
            // 
            this.tabPage_CSharp.Controls.Add(this.rtb_CSharpCode);
            this.tabPage_CSharp.Location = new System.Drawing.Point(4, 26);
            this.tabPage_CSharp.Name = "tabPage_CSharp";
            this.tabPage_CSharp.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_CSharp.Size = new System.Drawing.Size(527, 581);
            this.tabPage_CSharp.TabIndex = 1;
            this.tabPage_CSharp.Text = "C#";
            this.tabPage_CSharp.UseVisualStyleBackColor = true;
            // 
            // rtb_CSharpCode
            // 
            this.rtb_CSharpCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_CSharpCode.Location = new System.Drawing.Point(3, 3);
            this.rtb_CSharpCode.Name = "rtb_CSharpCode";
            this.rtb_CSharpCode.Size = new System.Drawing.Size(521, 575);
            this.rtb_CSharpCode.TabIndex = 0;
            this.rtb_CSharpCode.Text = "";
            this.rtb_CSharpCode.WordWrap = false;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(260, 639);
            this.propertyGrid1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_File,
            this.tsmi_Help});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1069, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmi_File
            // 
            this.tsmi_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Load,
            this.tsmi_Save,
            this.tsmi_Close});
            this.tsmi_File.Name = "tsmi_File";
            this.tsmi_File.Size = new System.Drawing.Size(39, 21);
            this.tsmi_File.Text = "File";
            // 
            // tsmi_Load
            // 
            this.tsmi_Load.Name = "tsmi_Load";
            this.tsmi_Load.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmi_Load.Size = new System.Drawing.Size(152, 22);
            this.tsmi_Load.Text = "Load";
            // 
            // tsmi_Save
            // 
            this.tsmi_Save.Name = "tsmi_Save";
            this.tsmi_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmi_Save.Size = new System.Drawing.Size(152, 22);
            this.tsmi_Save.Text = "Save";
            // 
            // tsmi_Close
            // 
            this.tsmi_Close.Name = "tsmi_Close";
            this.tsmi_Close.Size = new System.Drawing.Size(152, 22);
            this.tsmi_Close.Text = "Close";
            // 
            // tsmi_Help
            // 
            this.tsmi_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_About});
            this.tsmi_Help.Name = "tsmi_Help";
            this.tsmi_Help.Size = new System.Drawing.Size(47, 21);
            this.tsmi_Help.Text = "Help";
            // 
            // tsmi_About
            // 
            this.tsmi_About.Name = "tsmi_About";
            this.tsmi_About.Size = new System.Drawing.Size(111, 22);
            this.tsmi_About.Text = "About";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 666);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "ULEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip_TreeView.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Graph.ResumeLayout(false);
            this.tabPage_CSharp.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewTypes;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private GraphEditor graphEditor1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_Graph;
        private System.Windows.Forms.TabPage tabPage_CSharp;
        private System.Windows.Forms.RichTextBox rtb_CSharpCode;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_Compile;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmi_File;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Load;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Close;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Help;
        private System.Windows.Forms.ToolStripMenuItem tsmi_About;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Save;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_TreeView;
        private System.Windows.Forms.ToolStripMenuItem tsmi_AddType;
        private System.Windows.Forms.ToolStripMenuItem tsmi_AddMember;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Delete;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Refresh;
    }
}

