namespace WBH_Rezept
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
            this.btnVerbinden = new System.Windows.Forms.Button();
            this.tbCustomNumber = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbParameter = new System.Windows.Forms.CheckedListBox();
            this.btnPDF = new System.Windows.Forms.Button();
            this.cbID_CN = new System.Windows.Forms.ComboBox();
            this.cbWAT = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnVerbinden
            // 
            this.btnVerbinden.Location = new System.Drawing.Point(12, 168);
            this.btnVerbinden.Name = "btnVerbinden";
            this.btnVerbinden.Size = new System.Drawing.Size(93, 26);
            this.btnVerbinden.TabIndex = 0;
            this.btnVerbinden.Text = "Verbinden";
            this.btnVerbinden.UseVisualStyleBackColor = true;
            this.btnVerbinden.Click += new System.EventHandler(this.btnVerbinden_Click);
            // 
            // tbCustomNumber
            // 
            this.tbCustomNumber.Location = new System.Drawing.Point(12, 130);
            this.tbCustomNumber.Name = "tbCustomNumber";
            this.tbCustomNumber.Size = new System.Drawing.Size(148, 22);
            this.tbCustomNumber.TabIndex = 2;
            this.tbCustomNumber.Text = "01328235";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(249, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1104, 1061);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // cbParameter
            // 
            this.cbParameter.CheckOnClick = true;
            this.cbParameter.FormattingEnabled = true;
            this.cbParameter.Items.AddRange(new object[] {"IntParameter", "FloatParameter", "ShortParameter", "StringParameter", "DateTimeParameter"});
            this.cbParameter.Location = new System.Drawing.Point(12, 230);
            this.cbParameter.Name = "cbParameter";
            this.cbParameter.Size = new System.Drawing.Size(231, 106);
            this.cbParameter.TabIndex = 5;
            this.cbParameter.SelectedIndexChanged += new System.EventHandler(this.cbParameter_SelectedIndexChanged);
            // 
            // btnPDF
            // 
            this.btnPDF.Location = new System.Drawing.Point(40, 12);
            this.btnPDF.Name = "btnPDF";
            this.btnPDF.Size = new System.Drawing.Size(93, 26);
            this.btnPDF.TabIndex = 6;
            this.btnPDF.Text = "To PDF";
            this.btnPDF.UseVisualStyleBackColor = true;
            this.btnPDF.Click += new System.EventHandler(this.btnPDF_Click);
            // 
            // cbID_CN
            // 
            this.cbID_CN.FormattingEnabled = true;
            this.cbID_CN.Items.AddRange(new object[] {"ID", "CustomNumber"});
            this.cbID_CN.Location = new System.Drawing.Point(12, 100);
            this.cbID_CN.Name = "cbID_CN";
            this.cbID_CN.Size = new System.Drawing.Size(148, 24);
            this.cbID_CN.TabIndex = 7;
            this.cbID_CN.Text = "CustomNumber";
            // 
            // cbWAT
            // 
            this.cbWAT.FormattingEnabled = true;
            this.cbWAT.Items.AddRange(new object[] {"Auftrag", "Typ", "WBHRezept"});
            this.cbWAT.Location = new System.Drawing.Point(12, 70);
            this.cbWAT.Name = "cbWAT";
            this.cbWAT.Size = new System.Drawing.Size(148, 24);
            this.cbWAT.TabIndex = 8;
            this.cbWAT.Text = "Auftrag";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1353, 1061);
            this.Controls.Add(this.cbWAT);
            this.Controls.Add(this.cbID_CN);
            this.Controls.Add(this.btnPDF);
            this.Controls.Add(this.cbParameter);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tbCustomNumber);
            this.Controls.Add(this.btnVerbinden);
            this.MinimumSize = new System.Drawing.Size(1371, 1108);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox cbID_CN;

        private System.Windows.Forms.ComboBox cbWAT;

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;

        private System.Windows.Forms.Button btnPDF;

        private System.Windows.Forms.CheckedListBox cbParameter;

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

        private System.Windows.Forms.TextBox tbCustomNumber;

        private System.Windows.Forms.Button btnVerbinden;

        #endregion
    }
}