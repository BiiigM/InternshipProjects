using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32.SafeHandles;
using MySqlConnector;

namespace FileDIalog_XML
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<CD> cds = new List<CD>();

        private void btnLaden_Click(object sender, EventArgs e)
        {
            if (!tbPath.Text.Contains(".xml"))
            {
                MessageBox.Show("Es muss eine XML file sein", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            XmlToCD();
            CDtoGridView();
        }

        private void XmlToCD()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(File.ReadAllText(tbPath.Text));
                cds.Clear();
            
                foreach (XmlNode firstChild in doc.DocumentElement.ChildNodes)
                {
                    List<string> tmp = new List<string>();
                
                    foreach (XmlNode child in firstChild)
                    {
                        tmp.Add(child.InnerText);
                    }
                
                    BuildCD(tmp[0],tmp[1],tmp[2],tmp[3],tmp[4],tmp[5]);
                }
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Datei nicht gefunden", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuildCD(string title, string artist, string country, string company, string price, string year)
        {
            CD cd = new CD();
            
            cd.Title = title;
            cd.Artist = artist;
            cd.Country = country;
            cd.Company = company;
            cd.Price = price;
            cd.Year = year;
            cds.Add(cd);
        }

        private void CDtoGridView()
        {
            AddColums();
            dataGridView1.Rows.Clear();
            foreach (CD cd in cds)
            {
                dataGridView1.Rows.Add(cd.Title, cd.Artist, cd.Country, cd.Company, cd.Price, cd.Year);
            }
        }

        private void AddColums()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("title","Title");
            dataGridView1.Columns.Add("title","Artist");
            dataGridView1.Columns.Add("title","Country");
            dataGridView1.Columns.Add("title","Company");
            dataGridView1.Columns.Add("title","Price");
            dataGridView1.Columns.Add("title","Year");
        }

        private void btnLoadDB_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection("Data Source=localhost;User id=root;Password=root;database=cdstable");
            string sql = "SELECT * FROM CDs";
            
            using (connection)
            {
                connection.Open();

                using(var cmd = new MySqlCommand(sql, connection))
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BuildCD(dr["Titel"].ToString(),dr["Artist"].ToString(),dr["Country"].ToString(),dr["Company"].ToString(),dr["Price"].ToString(),dr["Year"].ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Datenbank leer!!");
                    }
                }
            }
            
            CDtoGridView();
        }

        private void btnSaveDB_Click(object sender, EventArgs e)
        {
            cds.Clear();
            GridViewToCDs();
            ClearSQL_Tabel();
            CDsToSQL();
        }

        private void GridViewToCDs()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                List<string> tmp = new List<string>();
                foreach (DataGridViewCell rowCell in row.Cells)
                {
                    if (rowCell.Value != null)
                    {
                        tmp.Add(rowCell.Value.ToString());
                    }
                    else if(row.Index != dataGridView1.Rows.Count - 1)
                    {
                        MessageBox.Show("Bro, mach ma was da rein!");
                        tmp.Add("KA");
                        continue;
                    }
                }
                if(row.Index != dataGridView1.Rows.Count - 1)
                    BuildCD(tmp[0], tmp[1], tmp[2], tmp[3], tmp[4], tmp[5]);
            }
        }

        private void CDsToSQL()
        {
            foreach (CD cd in cds)
            {
                MySqlConnection connection = new MySqlConnection("Data Source=localhost;User id=root;Password=root;database=cdstable");
                string sql = $"INSERT INTO CDs (`Titel`, `Artist`, `Country`, `Company`, `Price`, `Year`) VALUES ('{cd.Title}', '{cd.Artist}', '{cd.Country}', '{cd.Company}', '{cd.Price}', '{cd.Year}')";
                
                using (connection)
                {
                    connection.Open();
                    using(MySqlCommand cmd = new MySqlCommand(sql, connection))
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ClearSQL_Tabel()
        {
            MySqlConnection connection = new MySqlConnection("Data Source=localhost;User id=root;Password=root;database=cdstable");
            string sql = "TRUNCATE TABLE CDs";

            using (connection)
            {
                connection.Open();
                using(var cmd = new MySqlCommand(sql, connection))
                cmd.ExecuteNonQuery();
            }
        }

        private void btnSaveXML_Click(object sender, EventArgs e)
        {
            cds.Clear();
            GridViewToCDs();
            CreateXML();
        }

        private void CreateXML()
        {
            try
            {
                if (dataGridView1.Rows.Count - 1 <= 0)
                {
                    MessageBox.Show("Leere Kataloge machen wir hier nicht", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = "    "; //  "\t";
                settings.OmitXmlDeclaration = false;
                settings.Encoding = System.Text.Encoding.UTF8;

                using (XmlWriter writer = XmlTextWriter.Create(tbPath.Text, settings))
                {
                    writer.WriteStartElement("CATALOG");

                    foreach (CD cd in cds)
                    {
                        writer.WriteStartElement("CD");
                        writer.WriteElementString("TITLE", cd.Title);
                        writer.WriteElementString("ARTIST", cd.Artist);
                        writer.WriteElementString("COUNTRY", cd.Country);
                        writer.WriteElementString("COMPANY", cd.Company);
                        writer.WriteElementString("PRICE", cd.Price);
                        writer.WriteElementString("YEAT", cd.Year);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }
            }
            catch (NotSupportedException e)
            {
                MessageBox.Show("Was ist das denn für ein Dateiname!?", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}