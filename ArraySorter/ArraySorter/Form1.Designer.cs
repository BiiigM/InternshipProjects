namespace ArrySorter
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
            this.tbInput = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lInput = new System.Windows.Forms.Label();
            this.lOutput = new System.Windows.Forms.Label();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.rbAuf = new System.Windows.Forms.RadioButton();
            this.rbAb = new System.Windows.Forms.RadioButton();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbInput
            // 
            this.tbInput.Location = new System.Drawing.Point(12, 40);
            this.tbInput.Multiline = true;
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(303, 398);
            this.tbInput.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(321, 369);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(159, 69);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lInput
            // 
            this.lInput.Location = new System.Drawing.Point(12, 9);
            this.lInput.Name = "lInput";
            this.lInput.Size = new System.Drawing.Size(303, 28);
            this.lInput.TabIndex = 3;
            this.lInput.Text = "Input";
            // 
            // lOutput
            // 
            this.lOutput.Location = new System.Drawing.Point(486, 9);
            this.lOutput.Name = "lOutput";
            this.lOutput.Size = new System.Drawing.Size(302, 28);
            this.lOutput.TabIndex = 4;
            this.lOutput.Text = "Output";
            // 
            // tbOutput
            // 
            this.tbOutput.Location = new System.Drawing.Point(486, 40);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.Size = new System.Drawing.Size(302, 398);
            this.tbOutput.TabIndex = 5;
            // 
            // rbAuf
            // 
            this.rbAuf.Checked = true;
            this.rbAuf.Location = new System.Drawing.Point(321, 156);
            this.rbAuf.Name = "rbAuf";
            this.rbAuf.Size = new System.Drawing.Size(159, 35);
            this.rbAuf.TabIndex = 6;
            this.rbAuf.TabStop = true;
            this.rbAuf.Text = "Aufsteigend";
            this.rbAuf.UseVisualStyleBackColor = true;
            // 
            // rbAb
            // 
            this.rbAb.Location = new System.Drawing.Point(321, 208);
            this.rbAb.Name = "rbAb";
            this.rbAb.Size = new System.Drawing.Size(159, 35);
            this.rbAb.TabIndex = 7;
            this.rbAb.Text = "Absteigend";
            this.rbAb.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(321, 317);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(159, 46);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear Output";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.rbAb);
            this.Controls.Add(this.rbAuf);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.lOutput);
            this.Controls.Add(this.lInput);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbInput);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnClear;

        private System.Windows.Forms.RadioButton rbAuf;
        private System.Windows.Forms.RadioButton rbAb;

        private System.Windows.Forms.TextBox tbOutput;

        private System.Windows.Forms.TextBox textBox1;

        private System.Windows.Forms.Label lOutput;

        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lInput;

        #endregion
    }
}