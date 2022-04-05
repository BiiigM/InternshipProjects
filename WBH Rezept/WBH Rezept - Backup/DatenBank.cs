using System.Data;
using System.Data.SqlClient;

namespace WBH_Rezept
{
    public class DatenBank
    {
        private SqlConnection con;
        public DatenBank(string server,string database,string user,string password)
        {
            con = new SqlConnection();
            con.ConnectionString = $"Server={server};Database={database};User Id={user};Password={password};";
        }

        public SqlConnection connect()
        {
            con.Open();
            return con;
        }

        public void disconnect()
        {
            con.Close();
        }

        public SqlCommand getpPID(string id)
        {
            string sql = $"SELECT id, previousProcess_id FROM Process WHERE id = '{id}'";

            SqlCommand cmd = new SqlCommand(sql, con);
            return cmd;
        }

        public SqlCommand getIDByCustomNumber(string CustomNumber, string Typ)
        {
            string sql =   @"SELECT p.id, p.previousProcess_id, p.latest FROM Process p "
                           + "JOIN StringParameter sp " 
                           +   "ON sp.Process_id = p.id "
                           + $"WHERE sp.name = 'CustomNumber' AND sp.value = '{CustomNumber}' AND p.processType = '{Typ}'";
            SqlCommand cmd = new SqlCommand(sql, con);
                return cmd;
        }

        public SqlCommand GetWbhID(string pPid)
        {
            string sql = $"SELECT * FROM Process WHERE id = '{pPid}'";

            SqlCommand cmd = new SqlCommand(sql, con);
            return cmd;
        }

        public SqlCommand GetRecipeIDs(string typ_pPid)
        {
            string sql = "SELECT p.id, pt.component FROM Process p " 
                         +"JOIN ProcessTemplate pt "
                         + "ON pt.id = p.processTemplate_id "
                         + $"WHERE p.id in (SELECT recipeId FROM Process_Recipes WHERE processId = '{typ_pPid}')";

            SqlCommand cmd = new SqlCommand(sql, con);
            return cmd;
        }

        public SqlCommand GetParameter(string id, string parameter)
        {
            string sql = $"SELECT p.*, t.dataType FROM {parameter} p JOIN ParameterTemplate t ON p.parameterTemplate_id = t.id WHERE Process_id = '{id}'";

            SqlCommand cmd = new SqlCommand(sql, con);
            return cmd;
        }
    }
}