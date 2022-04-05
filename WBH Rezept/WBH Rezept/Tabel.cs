using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace WBH_Rezept
{
    public partial class Tabel : UserControl
    {
        private DataGridView _gridView;

        public Tabel()
        {
            InitializeComponent();
        }

        public void Build(DataTable dataTables, string tableName)
        {
            _gridView = new DataGridView();
            _gridView.Name = "gridView";
            _gridView.AutoSize = true;
            _gridView.Location = new Point(2, 34);
            _gridView.DataSource = dataTables;
            this.Controls.Add(_gridView);
            this.AutoSize = true;
           
            
            lTableName.Text = tableName;
        }
    }
}