
namespace ULEditor2
{
    partial class GraphEditor
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.labelScale = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnReposition = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(106, 0);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(176, 45);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.Value = 50;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // labelScale
            // 
            this.labelScale.ForeColor = System.Drawing.SystemColors.Window;
            this.labelScale.Location = new System.Drawing.Point(0, 0);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(100, 45);
            this.labelScale.TabIndex = 2;
            this.labelScale.Text = "视图缩放";
            this.labelScale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(289, 0);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 45);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "重置视图";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnReposition
            // 
            this.btnReposition.Location = new System.Drawing.Point(371, 0);
            this.btnReposition.Name = "btnReposition";
            this.btnReposition.Size = new System.Drawing.Size(75, 45);
            this.btnReposition.TabIndex = 4;
            this.btnReposition.Text = "重新排列";
            this.btnReposition.UseVisualStyleBackColor = true;
            this.btnReposition.Click += new System.EventHandler(this.btnReposition_Click);
            // 
            // GraphEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.btnReposition);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.trackBar1);
            this.DoubleBuffered = true;
            this.Name = "GraphEditor";
            this.Size = new System.Drawing.Size(582, 421);
            this.Load += new System.EventHandler(this.GraphEditor_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GraphEditor_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GraphEditor_MouseDown);
            this.MouseEnter += new System.EventHandler(this.GraphEditor_MouseEnter);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GraphEditor_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GraphEditor_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnReposition;
    }
}
