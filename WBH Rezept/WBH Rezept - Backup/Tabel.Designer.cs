using System.ComponentModel;

namespace WBH_Rezept
{
    partial class Tabel
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
            this.lTableName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lTableName
            // 
            this.lTableName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lTableName.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.lTableName.Location = new System.Drawing.Point(0, 0);
            this.lTableName.Name = "lTableName";
            this.lTableName.Size = new System.Drawing.Size(250, 36);
            this.lTableName.TabIndex = 0;
            this.lTableName.Text = "label1";
            this.lTableName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Tabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lTableName);
            this.Name = "Tabel";
            this.Size = new System.Drawing.Size(250, 336);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.Label lTableName;

        private System.Windows.Forms.Label label1;

        #endregion
    }
}