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
        private readonly DatenBank _db = new DatenBank(@"localhost\DEV", "--", "--", "--");
        private readonly string[] _parameterName = new[]
            {"IntParameter", "FloatParameter", "ShortParameter", "StringParameter", "DateTimeParameter"};
        private bool _connected;
        
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
        
        private Tabel _table;
        private readonly Dictionary<string, Rezept> rezepte = new Dictionary<string, Rezept>();

        private bool isConsole = false;
        
        #endregion
        
        public Form1(string customNumber)
        {
            isConsole = true;
            
            Tuple<string, string, string> ids = GetAllIDbyCN(customNumber);
            AuftragID = ids.Item1;
            TypID = ids.Item2;
            wbhID = ids.Item3;
            
            GetRecipeIDs();
            LoadAllDataAndGetMergedDT();
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
        }

        private void btnVerbinden_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            customNumberOrID = tbCustomNumber.Text;
            
            //It is for the checkBox with the Parameter
            _connected = true;
            
            GetIDs(customNumberOrID);
            LoadAllDataAndGetMergedDT();
            CreateAllGridView(GetUncheckedItems());
        }

        private void GetIDs(string CNorID)
        {
            AuftragID = "";
            TypID = "";
            wbhID = "";
            recipeID_Name.Clear();
            
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
                //when "Auftrag" is selected
                case 0:
                    Tuple<string, string, string> ids = GetAllIDbyCN(CN);
                    AuftragID = ids.Item1;
                    TypID = ids.Item2;
                    wbhID = ids.Item3;
                    break;
                //when "Typ" is selected
                case 1:
                    Tuple<string, string> ids2 = GetTypAndWbhIDbyCN(CN);
                    TypID = ids2.Item1;
                    wbhID = ids2.Item2;
                    break;
                //when "WBHRezept" is selected
                case 2:
                    wbhID = GetWbhIDbyCN(CN);
                    break;
            }
            
            //You always need the RecipeIDs
            GetRecipeIDs();
        }
        private Tuple<string,string,string> GetAllIDbyCN(string CN)
        {
            //ID1 = AuftragsID, ID2 = TypID, ID3 = WBHRezeptID
            string ID1 = "";
            string ID2 = "";
            string ID3 = "";
            
            SqlCommand cmdAuftrag = _db.getIDByCustomNumber(CN, "AUFTRAG");
            _db.connect();
            using (SqlDataReader dr = cmdAuftrag.ExecuteReader())
            {
                while (dr.Read())
                {
                    ID1 = dr["id"].ToString();
                    ID2 = dr["previousProcess_id"].ToString();
                }
            }
            _db.disconnect();
            
            //needed to get the PreviousProcess Id for the wbhID
            string tmppPID = "";

            SqlCommand cmdTypID = _db.getIDAndpPID(ID2);
            _db.connect();
            using (SqlDataReader dr = cmdTypID.ExecuteReader())
            {
                while (dr.Read())
                {
                    tmppPID = dr["previousProcess_id"].ToString();
                }
            }
            _db.disconnect();

            _db.connect();
            SqlCommand cmdWbh = _db.getIDAndpPID(tmppPID);
            using (SqlDataReader dr = cmdWbh.ExecuteReader())
            {
                while (dr.Read())
                {
                    ID3 = dr["id"].ToString();
                }
            }
            _db.disconnect();

            return new Tuple<string, string, string>(ID1, ID2, ID3);
        }
        
        private Tuple<string, string> GetTypAndWbhIDbyCN(string CN)
        {
            //ID1 = TypID, ID2 = WBHRezeptID
            string ID1 = "";
            string ID2 = "";
            
            SqlCommand cmdAuftrag = _db.getIDByCustomNumber(CN, "TYP");
            _db.connect();
            using (SqlDataReader dr = cmdAuftrag.ExecuteReader())
            {
                while (dr.Read())
                {
                    if(dr["latest"].ToString().Equals("False"))
                        continue;
                    ID1 = dr["id"].ToString();
                    ID2 = dr["previousProcess_id"].ToString();
                }
            }
            _db.disconnect();

            return new Tuple<string, string>(ID1, ID2);
        }

        private string GetWbhIDbyCN(string CN)
        {
            string ID1 = "";
            
            SqlCommand cmdAuftrag = _db.getIDByCustomNumber(CN, "WBHREZEPT");
            _db.connect();
            using (SqlDataReader dr = cmdAuftrag.ExecuteReader())
            {
                while (dr.Read())
                {
                    if(dr["latest"].ToString().Equals("False"))
                        continue;
                    ID1 = dr["id"].ToString();
                }
            }
            _db.disconnect();

            return ID1;
        }
        #endregion
        
        #region UserInputID
        private void GetAllIDsByUserInputID(string id)
        {
            switch (cbWAT.SelectedIndex)
            {
                //when "Auftrag" is selected
                case 0:
                    Tuple<string, string, string> ids = GetAllID(id);
                    AuftragID = ids.Item1;
                    TypID = ids.Item2;
                    wbhID = ids.Item3;
                    break;
                //when "Typ" is selected
                case 1:
                    Tuple<string, string> ids2 = GetTypAndWbhID(id);
                    TypID = ids2.Item1;
                    wbhID = ids2.Item2;
                    break;
                //when "WBHRezept" is selected
                case 2:
                    wbhID = id;
                    break;
            }
            
            //You always need the RecipeIDs
            GetRecipeIDs();
        }
        
        private Tuple<string, string, string> GetAllID(string id)
        {
            //id = AuftragsID, ID2 = TypID, ID3 = WBHRezeptID
            string ID2 = "";
            string ID3 = "";
            
            SqlCommand cmdAuftrag = _db.getIDAndpPID(id);
            _db.connect();
            using (SqlDataReader dr = cmdAuftrag.ExecuteReader())
            {
                while (dr.Read())
                {
                    ID2 = dr["previousProcess_id"].ToString();
                }
            }
            _db.disconnect();
            
            SqlCommand cmdTyp = _db.getIDAndpPID(ID2);
            _db.connect();
            using (SqlDataReader dr = cmdTyp.ExecuteReader())
            {
                while (dr.Read())
                {
                    ID3 = dr["previousProcess_id"].ToString();
                }
            }
            _db.disconnect();
            
            return new Tuple<string, string, string>(id, ID2, ID3);
        }

        private Tuple<string, string> GetTypAndWbhID(string id)
        {
            string ID2 = "";
            SqlCommand cmdTyp = _db.getIDAndpPID(id);
            _db.connect();
            using (SqlDataReader dr = cmdTyp.ExecuteReader())
            {
                while (dr.Read())
                {
                    ID2 = dr["previousProcess_id"].ToString();
                }
            }
            _db.disconnect();

            return new Tuple<string, string>(id, ID2);
        }
        #endregion
        private void GetRecipeIDs()
        {
            SqlCommand cmdRecipeIDs = _db.GetRecipeIDs(wbhID);
            _db.connect();
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
            _db.disconnect();
        }
        
        private void CreateAllGridView(List<string> checkedItems)
        {
            //Adding the "Auftrag" DataGrigview
            if(!AuftragID.Equals(""))
                flowLayoutPanel1.Controls.Add(CreateGridView(_auftrag.DataTables, _auftrag.name, checkedItems));
            
            //Adding the "Typ" DataGrigview
            if(!TypID.Equals(""))
                flowLayoutPanel1.Controls.Add(CreateGridView(_typ.DataTables, _typ.name, checkedItems));
            
            //Adding the "wbh" DataGrigview
            if(!wbhID.Equals(""))
                flowLayoutPanel1.Controls.Add(CreateGridView(_wbh.DataTables, _wbh.name, checkedItems));
            
            //Adding all "Rezepte" DataGrigview
            foreach (KeyValuePair<string,Rezept> rezept in rezepte)
            {
                flowLayoutPanel1.Controls.Add(CreateGridView(rezept.Value.DataTables, rezept.Key, checkedItems));
            }
        }

        private Control CreateGridView(Dictionary<string, DataTable> dataTables, string Name, List<string> skipTable)
        {
            _table = new Tabel();
            DataTable tmpDt = new DataTable();
            
            foreach (KeyValuePair<string,DataTable> dataTable in dataTables)
            {
                if(!isConsole)
                {
                    if (skipTable.Contains(dataTable.Key))
                    {
                        continue;
                    }
                }
                tmpDt.Merge(dataTable.Value);
            }
            
            _table.Build(tmpDt, Name);
            
            if(tmpDt.Rows.Count == 0 || isConsole)
                return null;

            return _table;
        }
        
        #region LoadingTheDataAndSaveMergedDT
        private void LoadAllDataAndGetMergedDT()
        {
            allTables.Clear();
            _auftrag = new Auftrag(AuftragID, _db, _parameterName);
            _typ = new Typ(TypID, _db, _parameterName);
            _wbh = new WBH(wbhID, _db, _parameterName);
            
            if(!AuftragID.Equals(""))
                allTables.Add("Auftrag" ,LoadAndGetMergedDTAuftrag());
            
            if(!TypID.Equals(""))
                allTables.Add("Typ", LoadAndGetMergedDTTyp());
            
            if(!wbhID.Equals(""))
                allTables.Add("WBH", LoadAndGetMergedDTWBH());
            
            rezepte.Clear();
            foreach (KeyValuePair<int,string> rezeptNameID in recipeID_Name)
            {
                allTables.Add(rezeptNameID.Value, LoadAndGetMergedDTRecipe(rezeptNameID.Key, rezeptNameID.Value));
            }
        }

        private DataTable LoadAndGetMergedDTRecipe(int rezeptID, string rezeptName)
        {
            //first we load the data
            _rezept = new Rezept(rezeptID.ToString(), _db, _parameterName);
            _rezept.LoadData();
            
            //then we save the "Rezept"
            rezepte.Add(rezeptName, _rezept);
            
            //last but not least we merge the data and return the merged DataTable
            DataTable mergedDT = new DataTable();
            foreach (KeyValuePair<string,DataTable> dataTable in _rezept.DataTables)
            {
                mergedDT.Merge(dataTable.Value);
            }

            return mergedDT;
        }

        private DataTable LoadAndGetMergedDTWBH()
        {
            //first we load the data
            _wbh.LoadData();
            
            //then we merge the data and return the merged DataTable
            DataTable mergedDT = new DataTable();
            foreach (KeyValuePair<string,DataTable> dataTable in _wbh.DataTables)
            {
                mergedDT.Merge(dataTable.Value);
            }

            return mergedDT;
        }

        private DataTable LoadAndGetMergedDTTyp()
        {
            //first we load the data
            if(!isConsole && cbWAT.SelectedIndex > 1) return null;
            _typ.LoadData();
            
            //then we merge the data and return the merged DataTable
            DataTable mergedDT = new DataTable();
            foreach (KeyValuePair<string,DataTable> dataTable in _typ.DataTables)
            {
                mergedDT.Merge(dataTable.Value);
            }

            return mergedDT;
        }

        private DataTable LoadAndGetMergedDTAuftrag()
        {
            //first we load the data
            if(!isConsole && cbWAT.SelectedIndex != 0) return null;
            _auftrag.LoadData();
            
            //then we merge the data and return the merged DataTable
            DataTable mergedDT = new DataTable();
            foreach (KeyValuePair<string,DataTable> dataTable in _auftrag.DataTables)
            {
                mergedDT.Merge(dataTable.Value);
            }

            return mergedDT;
        }
        #endregion

        //Selecting and deselecting Parameters
        private void cbParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!_connected) return;
            flowLayoutPanel1.Controls.Clear();
            
            CreateAllGridView(GetUncheckedItems());
        }

        private List<string> GetUncheckedItems()
        {
            List<string> checkedItems = new List<string>();
            //looking for uncheck Items
            for (int i = 0; i < cbParameter.Items.Count; i++)
            {
                if (!cbParameter.GetItemChecked(i))
                {
                    checkedItems.Add(cbParameter.Items[i].ToString());
                }
            }

            return checkedItems;
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
            //Image Header
            Image image = section.Headers.FirstPage.AddImage("../wienstroth_logo_2.jpg");
            image.Height = "2.5cm";
            image.LockAspectRatio = true;
            image.RelativeHorizontal = RelativeHorizontal.Margin;
            image.RelativeVertical = RelativeVertical.Line;
            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Center;
            image.WrapFormat.Style = WrapStyle.Through;
            
            //Get the spacing between the imgane and the Text
            Paragraph paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "1.6cm";
            
            //Creating a Table to setup the Text structure
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
            
            //this foreach can be used to show the rezept customNumber
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
                
                //Create Table
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
            //Replacing all '_', 'PAR', 'T', 't' to get a better Readable text
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
            //this is used to get a consistence size for the flowLayoutPanel1
            flowLayoutPanel1.Size = new Size(this.Width - 267, this.Height - 47);
        }
    }
}