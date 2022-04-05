using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Image = MigraDoc.DocumentObjectModel.Shapes.Image;

namespace WBH_Rezept
{
    public partial class Form1 : Form
    {
        #region Vars
        
        //SQL
        private readonly DatenBank db = new DatenBank(@"localhost\DEV", "iControl4-A1401", "sa", "wst");
        private readonly string[] parameterName = new[]
            {"IntParameter", "FloatParameter", "ShortParameter", "StringParameter", "DateTimeParameter"};
        private bool connected;
        
        //All IDs
        private string AuftragID = "";
        private string TypID = "";
        private string wbhID = "";
        private readonly IDictionary<int, string> recipeID_Name = new Dictionary<int, string>();
        private string customNumberOrID;
        
        //Saved Data
        private Auftrag _auftrag;
        private Typ _typ;
        private WBH _wbh;
        private Rezept _rezept;
        private Dictionary<string, DataTable> allTables = new Dictionary<string, DataTable>();
        
        private Tabel _tabel;
        private Dictionary<string, Rezept> rezepte;

        private bool isConsole = false;
        
        #endregion
        
        public Form1(string customNumber)
        {
            isConsole = true;
            GetAllIDbyCN(customNumber);
            GetRecipeIDs();
            LoadAllData();
            CreateAllTables(null);
            CreatePDF();
        }

        public Form1()
        {
            InitializeComponent();
            flowLayoutPanel1.Size = new Size(this.Width - 267, this.Height - 47);
            
            //set Checked
            for (int i = 0; i < cbParameter.Items.Count; i++)
            {
                cbParameter.SetItemChecked(i, true);
            }

            customNumberOrID = tbCustomNumber.Text;
        }

        private void btnVerbinden_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            customNumberOrID = tbCustomNumber.Text;
            //It is for the checkBox with the Parameter
            connected = true;
            
            GetIDs(customNumberOrID);
            LoadAllData();
            
            //Looking for unchecked Paramters
            List<string> checkedItems = new List<string>();
            for (int i = 0; i < cbParameter.Items.Count; i++)
            {
                if (!cbParameter.GetItemChecked(i))
                {
                    checkedItems.Add(cbParameter.Items[i].ToString());
                }
            }
            
            CreateAllTables(checkedItems);
        }

        private void GetIDs(string CNorID)
        {
            AuftragID = "";
            TypID = "";
            wbhID = "";
            recipeID_Name.Clear();
            
            ;
            switch (cbID_CN.SelectedIndex)
            {
                case 0:
                    //You can find it in the UserInputID region
                    GetAllIDsByUserInputID(CNorID);
                    break;
                case 1:
                    //You can find it in the CustomNumberID region
                    GetAllIDsByCustomNumber(CNorID);
                    break;
            }
            
        }
        
        #region CustomNumberID
        private void GetAllIDsByCustomNumber(string CN)
        {
            switch (cbWAT.SelectedIndex)
            {
                case 0:
                    GetAllIDbyCN(CN);
                    break;
                case 1:
                    GetTypAndWbhIDbyCN(CN);
                    break;
                case 2:
                    GetWbhIDbyCN(CN);
                    break;
            }

            GetRecipeIDs();
        }

        private void GetAllIDbyCN(string CN)
        {
            SqlCommand cmdAuftrag = db.getIDByCustomNumber(CN, "AUFTRAG");
            db.connect();
            using (SqlDataReader dr = cmdAuftrag.ExecuteReader())
            {
                while (dr.Read())
                {
                    AuftragID = dr["id"].ToString();
                    TypID = dr["previousProcess_id"].ToString();
                }
            }
            db.disconnect();

            string tmppPID = "";

            SqlCommand cmdTypID = db.getpPID(TypID);
            db.connect();
            using (SqlDataReader dr = cmdTypID.ExecuteReader())
            {
                while (dr.Read())
                {
                    tmppPID = dr["previousProcess_id"].ToString();
                }
            }
            db.disconnect();

            db.connect();
            SqlCommand cmdWbh = db.GetWbhID(tmppPID);
            using (SqlDataReader dr = cmdWbh.ExecuteReader())
            {
                while (dr.Read())
                {
                    wbhID = dr["id"].ToString();
                }
            }
            db.disconnect();
        }
        
        private void GetTypAndWbhIDbyCN(string CN)
        {
            SqlCommand cmdAuftrag = db.getIDByCustomNumber(CN, "TYP");
            db.connect();
            using (SqlDataReader dr = cmdAuftrag.ExecuteReader())
            {
                while (dr.Read())
                {
                    if(dr["latest"].ToString().Equals("False"))
                        continue;
                    TypID = dr["id"].ToString();
                    wbhID = dr["previousProcess_id"].ToString();
                }
            }
            db.disconnect();
        }

        private void GetWbhIDbyCN(string CN)
        {
            SqlCommand cmdAuftrag = db.getIDByCustomNumber(CN, "WBHREZEPT");
            db.connect();
            using (SqlDataReader dr = cmdAuftrag.ExecuteReader())
            {
                while (dr.Read())
                {
                    if(dr["latest"].ToString().Equals("False"))
                        continue;
                    wbhID = dr["id"].ToString();
                }
            }
            db.disconnect();
        }
        #endregion
        
        #region UserInputID
        private void GetAllIDsByUserInputID(string id)
        {
            switch (cbWAT.SelectedIndex)
            {
                case 0:
                    GetAllID(id);
                    break;
                case 1:
                    GetTypAndWbhID(id);
                    break;
                case 2:
                    wbhID = id;
                    break;
            }

            GetRecipeIDs();
        }
        
        private void GetAllID(string id)
        {
            AuftragID = id;
            
            SqlCommand cmdAuftrag = db.getpPID(id);
            db.connect();
            using (SqlDataReader dr = cmdAuftrag.ExecuteReader())
            {
                while (dr.Read())
                {
                    TypID = dr["previousProcess_id"].ToString();
                }
            }
            db.disconnect();
            
            SqlCommand cmdTyp = db.GetWbhID(TypID);
            db.connect();
            using (SqlDataReader dr = cmdTyp.ExecuteReader())
            {
                while (dr.Read())
                {
                    wbhID = dr["previousProcess_id"].ToString();
                }
            }
            db.disconnect();
        }

        private void GetTypAndWbhID(string id)
        {
            TypID = id;
            SqlCommand cmdTyp = db.GetWbhID(TypID);
            db.connect();
            using (SqlDataReader dr = cmdTyp.ExecuteReader())
            {
                while (dr.Read())
                {
                    wbhID = dr["previousProcess_id"].ToString();
                }
            }
            db.disconnect();
        }

        private void GetRecipeIDs()
        {
            SqlCommand cmdRecipeIDs = db.GetRecipeIDs(wbhID);
            db.connect();
            if(recipeID_Name.Count > 0) recipeID_Name.Clear();
            
            using (SqlDataReader dr = cmdRecipeIDs.ExecuteReader())
            {
                while (dr.Read())
                {
                    //They get saved with id and name
                    Int32.TryParse(dr["id"].ToString(), out int id);
                    recipeID_Name[id] = dr["component"].ToString();
                }
            }
            db.disconnect();
        }
        #endregion
        
        private void CreateAllTables(List<string> checkedItems)
        {
            allTables.Clear();
            if(!AuftragID.Equals(""))
                CreateTable(_auftrag.DataTables, _auftrag.name, checkedItems);
            
            if(!TypID.Equals(""))
                CreateTable(_typ.DataTables, _typ.name, checkedItems);
            
            if(!wbhID.Equals(""))
                CreateTable(_wbh.DataTables, _wbh.name, checkedItems);
            
            foreach (KeyValuePair<string,Rezept> rezept in rezepte)
            {
                CreateTable(rezept.Value.DataTables, rezept.Key, checkedItems);
            }
        }

        private void CreateTable(Dictionary<string, DataTable> dataTables, string Name, List<string> skipTable)
        {
            _tabel = new Tabel();
            DataTable tmpDt = new DataTable();
            DataTable allDt = new DataTable();
            
            foreach (KeyValuePair<string,DataTable> dataTable in dataTables)
            {
                allDt.Merge(dataTable.Value);
                if(!isConsole)
                {
                    if (skipTable.Contains(dataTable.Key))
                    {
                        continue;
                    }
                }
                tmpDt.Merge(dataTable.Value);
            }
            
            allTables.Add(Name, allDt);
            
            if(tmpDt.Rows.Count == 0 || isConsole)
                return;
            _tabel.Build(tmpDt, Name);
            flowLayoutPanel1.Controls.Add(_tabel);
        }
        
        #region LoadingTheData
        private void LoadAllData()
        {
            LoadAuftrag();
            LoadTyp();
            LoadWBH();
            LoadRecipe();
        }

        private void LoadRecipe()
        {
            rezepte = new Dictionary<string, Rezept>();
            foreach (KeyValuePair<int, string> rID in recipeID_Name)
            {
                _rezept = new Rezept(rID.Key.ToString(), db, parameterName);
                _rezept.LoadData();
                rezepte.Add(rID.Value, _rezept);
            }
        }

        private void LoadWBH()
        {
            _wbh = new WBH(wbhID, db, parameterName);
            _wbh.LoadData();
        }

        private void LoadTyp()
        {
            _typ = new Typ(TypID, db, parameterName);
            if(!isConsole && cbWAT.SelectedIndex > 1) return;
            _typ.LoadData();
        }

        private void LoadAuftrag()
        {
            _auftrag = new Auftrag(AuftragID, db, parameterName);
            if(!isConsole && cbWAT.SelectedIndex != 0) return;
            _auftrag.LoadData();
        }
        #endregion

        //Selecting and deselecting Parameters
        private void cbParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!connected) return;
            
            flowLayoutPanel1.Controls.Clear();
            List<string> checkedItems = new List<string>();

            for (int i = 0; i < cbParameter.Items.Count; i++)
            {
                if (!cbParameter.GetItemChecked(i))
                {
                    checkedItems.Add(cbParameter.Items[i].ToString());
                }
            }
            
            CreateAllTables(checkedItems);
        }

        #region PDF
        private void btnPDF_Click(object sender, EventArgs e)
        {
            if (allTables.Count == 0)
            {
                MessageBox.Show("Sie müssen sich mit einer DatanBank verbinden!!", "Info");
                return;
            }
            
            CreatePDF();
        }

        private void CreatePDF()
        {
            Document pdf = CreateDocument();
            pdf.UseCmykColor = true;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);
            pdfRenderer.Document = pdf;
            pdfRenderer.RenderDocument();
            
            pdfRenderer.PdfDocument.Save("Parameter.pdf");
            Process.Start("Parameter.pdf");
        }
        
        private Document CreateDocument()
        {
            Document document = new Document();
            Section section = document.AddSection();
            section.PageSetup.DifferentFirstPageHeaderFooter = true;
            
            createHeader(section);

            addValues(section);
            
            return document;
        }
        
        private void createHeader(Section section)
        {
            Image image = section.Headers.FirstPage.AddImage("../wienstroth_logo_2.jpg");
            image.Height = "2.5cm";
            image.LockAspectRatio = true;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.RelativeVertical = RelativeVertical.Line;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Center;
            image.WrapFormat.Style = WrapStyle.Through;

            Paragraph paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "1.6cm";

            Table tableHeader = section.AddTable();

            Column column = tableHeader.AddColumn("2cm");
            column = tableHeader.AddColumn("5cm");

            Row row = tableHeader.AddRow();
            row.Cells[0].AddParagraph("Werk:");
            row.Cells[1].AddParagraph("...");
            
            row = tableHeader.AddRow();
            row.Cells[0].AddParagraph("Datum:");
            row.Cells[1].AddParagraph(DateTime.Today.ToString("dd.MM.yyyy"));
            
            if(!_auftrag.customNumber.Equals(""))
            {
                row = tableHeader.AddRow();
                row.Cells[0].AddParagraph("Auftrag:");
                row.Cells[1].AddParagraph(_auftrag.customNumber);
            }
            
            if(!_typ.customNumber.Equals(""))
            {
                row = tableHeader.AddRow();
                row.Cells[0].AddParagraph("Typ:");
                row.Cells[1].AddParagraph(_typ.customNumber);
            }
            
            row = tableHeader.AddRow();
            row.Cells[0].AddParagraph("WBH:");
            row.Cells[1].AddParagraph(_wbh.customNumber);

            /*foreach (KeyValuePair<string,Rezept> rezept in rezepte)
            {
                row = tableHeader.AddRow();
                row.Cells[0].AddParagraph(rezept.Key + ":");
                row.Cells[1].AddParagraph(rezept.Value.customNumber);
            }*/


        }
        
        private void addValues(Section section)
        {
            foreach (KeyValuePair<string,DataTable> table in allTables)
            {
                if(table.Value.Rows.Count == 0)
                    continue;
                //New Title
                Paragraph Title = section.AddParagraph();
                Title.AddFormattedText(table.Key + " Parameter", TextFormat.Bold);
                Title.Format.Alignment = ParagraphAlignment.Left;
                Title.Format.SpaceBefore = "1cm";
                Title.Format.SpaceAfter = "0.5cm";
                Title.Format.Font.Size = 15;
                
                //Create Tabele
                Table parameter = section.AddTable();
                parameter.Format.Alignment = ParagraphAlignment.Left;
                
                //Columms
                Column column = parameter.AddColumn("8cm");
                column = parameter.AddColumn("8cm");

                for (int i = 0; i < table.Value.Rows.Count - 1; i++)
                {
                    Row row = parameter.AddRow();
                    row.Cells[0].AddParagraph(GoodString(table.Value.Rows[i][1].ToString()));
                    row.Cells[1].AddParagraph(GoodString(table.Value.Rows[i][2].ToString()));
                }
            }
        }

        private string GoodString(string s)
        {
            string result = null; 
            if (s.Contains("_"))
            {
                IList<string> splittedName = s.Split('_').ToList();
                int length = splittedName.Count-1;

                if (splittedName.Contains("PAR"))
                {
                    int idx = splittedName.IndexOf("PAR");
                    splittedName.RemoveAt(idx);
                }

                if (splittedName.Contains("T"))
                {
                    int idx = splittedName.IndexOf("T");
                    splittedName[idx] = "Temperatur";
                }
                
                if (splittedName.Contains("t"))
                {
                    int idx = splittedName.IndexOf("t");
                    splittedName[idx] = "Zeit";
                }
                
                for (var i = 1; i < splittedName.Count; i += 2)
                {
                    splittedName.Insert(i, " ");
                }

                result = String.Join("",splittedName.ToArray());
            }
            else
            {
                return s;
            }
            
            return result;
        }
        #endregion
        
        private void Form1_Resize(object sender, EventArgs e)
        {
            flowLayoutPanel1.Size = new Size(this.Width - 267, this.Height - 47);
        }
    }
}