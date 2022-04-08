using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;

namespace PeridoenSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<PeriodenElement> pes = new List<PeriodenElement>();

        private void btnLaden_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            SaveElements();
            CreateTemplate();
        }

        private void CreateTemplate()
        {
            foreach (PeriodenElement pe in pes)
            {
                SpecialTemplate(pe);
                if (pe.idx >= 58 && pe.idx <= 71)
                {
                    continue;
                }
                if (pe.idx >= 90 && pe.idx <= 103)
                {
                    continue;
                }
                
                template tp = new template(pe.idx.ToString(), pe.Name, pe.Prefix, pe.Mass, GetColor(pe.Kategorie));
                flowLayoutPanel1.Controls.Add(tp);
                
                MakeSpacing(pe);
            }

            foreach (PeriodenElement pe in pes)
            {
                if (pe.idx >= 58 && pe.idx <= 71)
                {
                    template tp = new template(pe.idx.ToString(), pe.Name, pe.Prefix, pe.Mass, GetColor(pe.Kategorie));
                    flowLayoutPanel1.Controls.Add(tp);
                }

                if (pe.idx == 71)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Space sp = new Space();
                        flowLayoutPanel1.Controls.Add(sp);
                    }
                }
                
                if (pe.idx >= 90 && pe.idx <= 103)
                {
                    template tp = new template(pe.idx.ToString(), pe.Name, pe.Prefix, pe.Mass, GetColor(pe.Kategorie));
                    flowLayoutPanel1.Controls.Add(tp);
                }
                
                if (pe.idx == 103)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Space sp = new Space();
                        flowLayoutPanel1.Controls.Add(sp);
                    }
                }
            }
        }

        private void SpecialTemplate(PeriodenElement periodenElement)
        {
            if (periodenElement.idx == 58)
            {
                template tp = new template("*", "", "", "", GetColor("Lanthanoid"));
                flowLayoutPanel1.Controls.Add(tp);
            }
            if (periodenElement.idx == 90)
            {
                template tp = new template("**", "", "", "", GetColor("Actinoid "));
                flowLayoutPanel1.Controls.Add(tp);
            }
        }

        private void MakeSpacing(PeriodenElement periodenElement)
        {
            if (periodenElement.idx == 1)
            {
                for (int i = 0; i < 17; i++)
                {
                    Space sp = new Space();
                    flowLayoutPanel1.Controls.Add(sp);
                }
            }
            else if(periodenElement.idx == 4 || periodenElement.idx == 12)
            {
                for (int i = 0; i < 11; i++)
                {
                    Space sp = new Space();
                    flowLayoutPanel1.Controls.Add(sp);
                }
            }
            else if(periodenElement.idx == 21 || periodenElement.idx == 39)
            {
                for (int i = 0; i < 1; i++)
                {
                    Space sp = new Space();
                    flowLayoutPanel1.Controls.Add(sp);
                }
            }
            else if (periodenElement.idx == 108)
            {
                for (int i = 0; i < 13; i++)
                {
                    Space sp = new Space();
                    flowLayoutPanel1.Controls.Add(sp);
                }
            }
        }

        private void SaveElements()
        {
            MySqlConnection mysqlCon = new MySqlConnection("server=localhost;uid=root;pwd=root;database=periode;");
            string sql = "SELECT * FROM Periodensystem";
            
            using (mysqlCon)
            {
                mysqlCon.Open();
                using(var command = new MySqlCommand(sql, mysqlCon))
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        BuildElement((int)dr["id"], dr["Name"].ToString(), dr["Symbol"].ToString(), dr["Masse"].ToString(), dr["Kategorie"].ToString());
                    }
                }
            }
        }

        private void BuildElement(int idx, string name, string prefix, string mass, string kategorie)
        {
            PeriodenElement pe = new PeriodenElement();

            pe.idx = idx;
            pe.Name = name;
            pe.Prefix = prefix;
            pe.Mass = mass;
            pe.Kategorie = kategorie;
            
            pes.Add(pe);
        }

        private Color GetColor(string kategorie)
        {
            Color c = Color.White;
            switch (kategorie)
            {
                case "Nichtmetal":
                    c = Color.FromArgb(166, 224, 219);
                    break;
                case "Edelgas ":
                    c = Color.FromArgb(189, 189, 189);
                    break;
                case "Alkalimetall":
                    c = Color.FromArgb(255, 135, 135);
                    break;
                case "Erdkalimetal":
                    c = Color.FromArgb(255, 195, 135);
                    break;
                case "?bergangsmetal":
                    c = Color.FromArgb(255, 251, 135);
                    break;
                case "Halbmetal":
                    c = Color.FromArgb(120, 196, 125);
                    break;
                case "Metall ":
                    c = Color.FromArgb(197, 255, 135);
                    break;
                case "Halogen ":
                    c = Color.FromArgb(135, 163, 255);
                    break;
                case "Lanthanoid":
                    c = Color.FromArgb(255, 158, 242);
                    break;
                case "Actinoid ":
                    c = Color.FromArgb(154, 106, 171);
                    break;
            }

            return c;
        }
    }
}