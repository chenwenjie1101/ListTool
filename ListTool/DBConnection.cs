using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;
using System.Configuration;

namespace ListTool
{
    public class DBConnection
    {
        /// <summary>
        /// 获取数据库链接
        /// </summary>
        private static string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public OracleConnection GetConnection()
        {
            OracleConnection conn = new OracleConnection(connStr);
            return conn;
        }
        public DataSet GetDataSet(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                OracleConnection oclConn = GetConnection();
                //OracleCommand cmd = new OracleCommand(sql, oclConn);
                //oclConn.Open();
                OracleDataAdapter da = new OracleDataAdapter(sql, oclConn);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }

            return ds;
        }
        public int update(string sql)
        {
            OracleConnection oclConn = GetConnection();
            oclConn.Open();
            OracleCommand cmd = new OracleCommand(sql, oclConn);
            int temp = cmd.ExecuteNonQuery();
            oclConn.Close();
            return temp;
        }
        public string updates(List<string> sqls)
        {
            OracleConnection oclConn = GetConnection();
            oclConn.Open();
            OracleTransaction trans = oclConn.BeginTransaction();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = oclConn;
            cmd.Transaction = trans;
            try
            {
                foreach(var sql in sqls)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
            }
            catch(Exception ex)
            {
                trans.Rollback();
                return ex.Message + ex.Source + ex.StackTrace + ex.TargetSite;
            }
            oclConn.Close();
            return "0";
        }
    }
}
