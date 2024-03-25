using System;
using System.Data;
using System.Data.SqlClient;

namespace SecurityImplementation.Helper
{
    public class DB : IDisposable
    {
        private SqlCommand cmd;
        private SqlConnection cn;
        private bool disposed = false;

        public DB()
        {
            cn = new SqlConnection("Server=172.16.1.132;Database=TRAINING;Uid=training_user;Pwd=Aksa@1234;");
        }

        private void Open()
        {
            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }
        }

        private void Close()
        {
            if (cn.State != ConnectionState.Closed)
            {
                cn.Close();
            }
        }

        public SqlDataReader GetDataReader(string strSql)
        {
            SqlDataReader dr = null;
            try
            {
                Open();
                cmd = new SqlCommand(strSql, cn);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Close();
                throw new Exception("GetDataReader : " + ex.Message);
            }

            return dr;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    cmd?.Dispose();
                    cn?.Dispose();
                }

                disposed = true;
            }
        }

        ~DB()
        {
            Dispose(false);
        }
    }
}