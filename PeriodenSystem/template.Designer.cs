using System.ComponentModel;

namespace PeridoenSystem
{
    partial class template
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lCounter = new System.Windows.Forms.Label();
            this.lName = new System.Windows.Forms.Label();
            this.lPrefix = new System.Windows.Forms.Label();
            this.lMasse = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lCounter
            // 
            this.lCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lCounter.Location = new System.Drawing.Point(-1, 0);
            this.lCounter.Name = "lCounter";
            this.lCounter.Size = new System.Drawing.Size(77, 21);
            this.lCounter.TabIndex = 0;
            this.lCounter.Text = "1";
            this.lCounter.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lName
            // 
            this.lName.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lName.Location = new System.Drawing.Point(-1, 69);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(77, 16);
            this.lName.TabIndex = 1;
            this.lName.Text = "Test";
            this.lName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lPrefix
            // 
            this.lPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lPrefix.Location = new System.Drawing.Point(-1, 21);
            this.lPrefix.Name = "lPrefix";
            this.lPrefix.Size = new System.Drawing.Size(77, 27);
            this.lPrefix.TabIndex = 2;
            this.lPrefix.Text = "H";
            this.lPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lMasse
            // 
            this.lMasse.Location = new System.Drawing.Point(-1, 48);
            this.lMasse.Name = "lMasse";
            this.lMasse.Size = new System.Drawing.Size(77, 21);
            this.lMasse.TabIndex = 3;
            this.lMasse.Text = "0";
            this.lMasse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // template
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lMasse);
            this.Controls.Add(this.lPrefix);
            this.Controls.Add(this.lName);
            this.Controls.Add(this.lCounter);
            this.Name = "template";
            this.Size = new System.Drawing.Size(77, 87);
            this.Load += new System.EventHandler(this.template_Load);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lMasse;

        private System.Windows.Forms.Label lPrefix;

        private System.Windows.Forms.Label lName;

        private System.Windows.Forms.Label lCounter;

        private System.Windows.Forms.Label label1;

        #endregion
    }
}