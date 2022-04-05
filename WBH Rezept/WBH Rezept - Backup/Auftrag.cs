using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WBH_Rezept
{
    public class Auftrag
    {
        private DataTable tmpDt;
        private string[] parameterName;
        private DatenBank db;
        private string id;
        public Dictionary<string, DataTable> DataTables;
        public string name = "Auftrag";
        public string customNumber;

        public Auftrag(string id, DatenBank db, string[] parameterName)
        {
            this.id = id;
            this.db = db;
            this.parameterName = parameterName;
            customNumber = "";
        }

        public void LoadData()
        {
            DataTables = new Dictionary<string, DataTable>();
            foreach (string s in parameterName)
            {
                tmpDt = new DataTable();
                AddColums();
                
                SqlCommand cmd = db.GetParameter(id, s);
                db.connect();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DataRow _row = tmpDt.NewRow();
                        _row["id"] = dr["id"];
                        _row["name"] = dr["name"];
                        _row["value"] = dr["value"];
                        _row["parameterTemplate_id"] = dr["parameterTemplate_id"];
                        _row["Process_id"] = dr["Process_id"];
                        _row["dataType"] = dr["dataType"];
                        tmpDt.Rows.Add(_row);

                        if (dr["name"].ToString().Equals("CustomNumber")) customNumber = dr["value"].ToString();
                    }
                }
                db.disconnect();
                
                DataTables.Add(s, tmpDt);
            }
        }

        private void AddColums()
        {
            tmpDt.Columns.Add("id");
            tmpDt.Columns.Add("name");
            tmpDt.Columns.Add("value");
            tmpDt.Columns.Add("parameterTemplate_id");
            tmpDt.Columns.Add("Process_id");
            tmpDt.Columns.Add("dataType");
        }
    }
}