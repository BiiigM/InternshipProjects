namespace GUISocketClient2
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
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbInput = new System.Windows.Forms.TextBox();
            this.lInfo = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(12, 12);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOutput.Size = new System.Drawing.Size(413, 241);
            this.tbOutput.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(12, 287);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(413, 58);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbInput
            // 
            this.tbInput.Location = new System.Drawing.Point(12, 259);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(413, 22);
            this.tbInput.TabIndex = 0;
            this.tbInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbInput_KeyDown);
            // 
            // lInfo
            // 
            this.lInfo.Location = new System.Drawing.Point(12, 418);
            this.lInfo.Name = "lInfo";
            this.lInfo.Size = new System.Drawing.Size(413, 23);
            this.lInfo.TabIndex = 3;
            this.lInfo.Text = "...";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 375);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(413, 24);
            this.comboBox1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 450);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.lInfo);
            this.Controls.Add(this.tbInput);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbOutput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox comboBox1;

        private System.Windows.Forms.Label lInfo;

        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.Button btnSend;

        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.TextBox textBox1;

        #endregion
    }
}