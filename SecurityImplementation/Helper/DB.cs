using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace SecurityImplementation.Helper
{
    public class DB : IDisposable
    {
        private MySqlCommand cmd;
        private MySqlConnection cn;
        private bool disposed = false;

        public DB()
        {
            cn = new MySqlConnection("server=172.16.1.132;user=training_user;database=TRAINING;port=3306;password=Aksa@1234;");
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

        public MySqlDataReader GetDataReader(string strSql)
        {
            MySqlDataReader dr = null;
            try
            {
                Open();
                cmd = new MySqlCommand(strSql, cn);
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