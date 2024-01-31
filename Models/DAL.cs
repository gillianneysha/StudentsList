using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace DataAccess
{
    public class DAL
    {
        public static string cs = "Server=DESKTOP-CPVB2FH\\SQLEXPRESS;" +
            "Database=Finals; Integrated Security=SSPI;" + "TrustServerCertificate=true;" + "MultipleActiveResultSets=true";

        private SqlConnection cn = new(cs);
        private SqlCommand cmd;
        private SqlDataAdapter da = new();
        public void Open()
        {
            if (cn.State == System.Data.ConnectionState.Closed)
                cn.Open();
        }

        public void Close()
        {
            if (cn.State == System.Data.ConnectionState.Open)
                cn.Close();
        }

        public void SetSql(string sql)
        {
            cmd = new(sql, cn);
        }

        public void AddParam(string paramName, object paramValue)
        {
            cmd.Parameters.AddWithValue(paramName, paramValue);
        }

        public void Execute()
        {
            try
            {
                Open();
                cmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                Close();
            }

        }

        public SqlDataReader GetReader()
        {
            return cmd.ExecuteReader();
        }
        public DataTable GetData()
        {
            try
            {
                da.SelectCommand = cmd;

                DataTable dt = new();
                Open();
                da.Fill(dt);
                Close();

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Close();
            }
        }

        public bool checkAvailable()
        {
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {

                cn.Close();
                return (true);
            }
            else
            {

                cn.Close();
                return (false);

            }

        }

        public bool checkSuccess()
        {
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {

                return (true);

            }
            else
            {
                cn.Close();
                return (false);
            }
            cn.Close();
            return (false);
        }

    }
}
