using System.ComponentModel;

namespace GUISocketClient2
{
    partial class NameGiver
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.lName = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lName
            // 
            this.lName.Location = new System.Drawing.Point(12, 9);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(236, 23);
            this.lName.TabIndex = 0;
            this.lName.Text = "What is your name?";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 35);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(236, 22);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 63);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(236, 46);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save you name";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // NameGiver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 122);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "NameGiver";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NameGiver";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnSave;

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.Label lName;

        #endregion
    }
}