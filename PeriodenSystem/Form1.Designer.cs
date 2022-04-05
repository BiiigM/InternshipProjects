namespace PeridoenSystem
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLaden = new System.Windows.Forms.ToolStripButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.btnLaden});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1571, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnLaden
            // 
            this.btnLaden.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLaden.Image = ((System.Drawing.Image) (resources.GetObject("btnLaden.Image")));
            this.btnLaden.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLaden.Name = "btnLaden";
            this.btnLaden.Size = new System.Drawing.Size(157, 24);
            this.btnLaden.Text = "Periodensystem laden";
            this.btnLaden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLaden.Click += new System.EventHandler(this.btnLaden_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 27);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1571, 832);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1571, 859);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

        private System.Windows.Forms.ToolStripButton btnLaden;

        private System.Windows.Forms.ToolStrip toolStrip1;

        #endregion
    }
}