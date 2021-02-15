
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
            this.treeViewTypes = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_Graph = new System.Windows.Forms.TabPage();
            this.graphEditor1 = new ULEditor2.GraphEditor();
            this.tabPage_CSharp = new System.Windows.Forms.TabPage();
            this.rtb_CSharpCode = new System.Windows.Forms.RichTextBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_Graph.SuspendLayout();
            this.tabPage_CSharp.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewTypes
            // 
            this.treeViewTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTypes.Location = new System.Drawing.Point(0, 0);
            this.treeViewTypes.Name = "treeViewTypes";
            this.treeViewTypes.Size = new System.Drawing.Size(200, 448);
            this.treeViewTypes.TabIndex = 0;
            this.treeViewTypes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTypes_AfterSelect);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(-1, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewTypes);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(801, 448);
            this.splitContainer1.SplitterDistance = 200;
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
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer2.Size = new System.Drawing.Size(597, 448);
            this.splitContainer2.SplitterDistance = 400;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_Graph);
            this.tabControl1.Controls.Add(this.tabPage_CSharp);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(400, 448);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_Graph
            // 
            this.tabPage_Graph.Controls.Add(this.graphEditor1);
            this.tabPage_Graph.Location = new System.Drawing.Point(4, 26);
            this.tabPage_Graph.Name = "tabPage_Graph";
            this.tabPage_Graph.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Graph.Size = new System.Drawing.Size(392, 418);
            this.tabPage_Graph.TabIndex = 0;
            this.tabPage_Graph.Text = "蓝图";
            this.tabPage_Graph.UseVisualStyleBackColor = true;
            // 
            // graphEditor1
            // 
            this.graphEditor1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.graphEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphEditor1.Location = new System.Drawing.Point(3, 3);
            this.graphEditor1.memberInfo = null;
            this.graphEditor1.Name = "graphEditor1";
            this.graphEditor1.Size = new System.Drawing.Size(386, 412);
            this.graphEditor1.TabIndex = 0;
            // 
            // tabPage_CSharp
            // 
            this.tabPage_CSharp.Controls.Add(this.rtb_CSharpCode);
            this.tabPage_CSharp.Location = new System.Drawing.Point(4, 26);
            this.tabPage_CSharp.Name = "tabPage_CSharp";
            this.tabPage_CSharp.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_CSharp.Size = new System.Drawing.Size(392, 418);
            this.tabPage_CSharp.TabIndex = 1;
            this.tabPage_CSharp.Text = "C#";
            this.tabPage_CSharp.UseVisualStyleBackColor = true;
            // 
            // rtb_CSharpCode
            // 
            this.rtb_CSharpCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_CSharpCode.Location = new System.Drawing.Point(3, 3);
            this.rtb_CSharpCode.Name = "rtb_CSharpCode";
            this.rtb_CSharpCode.Size = new System.Drawing.Size(386, 412);
            this.rtb_CSharpCode.TabIndex = 0;
            this.rtb_CSharpCode.Text = "";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(193, 448);
            this.propertyGrid1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "ULEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Graph.ResumeLayout(false);
            this.tabPage_CSharp.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}

