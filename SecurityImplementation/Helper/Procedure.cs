using MySql.Data.MySqlClient;
using SecurityImplementation.Controllers;
using SecurityImplementation.Model;
using System.Data;

namespace SecurityImplementation.Helper
{
    public class Procedure
    {
        public static LoginProcRespModel LoginAuthenticate(LoginRequestModel login)
        {
            string PKG_PRC = "PRC_AUTHENTICATE_USER";

            MySqlConnection conn = new MySqlConnection("server=172.16.1.132;user=training_user;database=TRAINING;port=3306;password=Aksa@1234;");
            MySqlCommand cmd = new MySqlCommand(PKG_PRC, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 999999;


            cmd.Parameters.Add("PUSER_ID", MySqlDbType.VarChar).Value = login.username;
            cmd.Parameters.Add("PPASSWORD", MySqlDbType.VarChar).Value = login.password;

            cmd.Parameters.Add("pCODE", MySqlDbType.VarChar).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("pDESC", MySqlDbType.VarChar).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("pMSG", MySqlDbType.VarChar).Direction = ParameterDirection.Output;


            try
            {
                conn.Close();
                conn.Open();

                cmd.ExecuteNonQuery();


                string sCODE = cmd.Parameters["PCODE"].Value != null ? cmd.Parameters["PCODE"].Value.ToString() : "";
                string sDESC = cmd.Parameters["PDESC"].Value != null ? cmd.Parameters["PDESC"].Value.ToString() : "";
                string sMSG = cmd.Parameters["PMSG"].Value != null ? cmd.Parameters["PMSG"].Value.ToString() : "";

                conn.Close();

                return new LoginProcRespModel { Code = sCODE, Desc = sDESC, Msg = sMSG };

            }
            catch (Exception ex)
            {
                return new LoginProcRespModel { Code = "99", Desc = "UNSUCCESFULL", Msg = "UNAUTHENTICATED" };
            }
            finally
            {

                cmd.Dispose();
                cmd = null;
                conn.Dispose();
                conn = null;

            }

        }

        public static LoginProcRespModel RawQueryReset(string username, string password)
        {
            string connStr = "server=172.16.1.132;user=training_user;database=TRAINING;port=3306;password=Aksa@1234;";
            using (var conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    int userId = 0;
                    string getUserIdSql = $"SELECT USER_ID FROM USERS WHERE USER_NAME = '{username}'"; // Assume the table is 'users' and has 'user_id'

                    using (var cmd = new MySqlCommand(getUserIdSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0); // Assuming user_id is the first column
                            }
                        }
                    }

                    if (userId > 0)
                    {
                        // Step 2: Update the Password
                        string updatePasswordSql = $"UPDATE USERS SET PASSWORD = '{password}' WHERE USER_ID = {userId}";
                        using (var cmd = new MySqlCommand(updatePasswordSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@NewPassword", password);
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                return new LoginProcRespModel { Code = "00", Msg = "UPDATED" };
                            }
                            else
                            {
                                return new LoginProcRespModel { Code = "99", Msg = "FAIL" };
                            }
                        }
                    }
                    else
                    {
                        return new LoginProcRespModel { Code = "99", Msg = "FAIL" };
                    }
                }
                catch (Exception ex)
                {
                    return new LoginProcRespModel { Code = "99", Msg = "FAIL" };
                }

            }
        }

        public static int GetUserId(string username)
        {
            string connStr = "server=172.16.1.132;user=training_user;database=TRAINING;port=3306;password=Aksa@1234;";
            using (var conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    int userId = 0;
                    string getUserIdSql = $"SELECT USER_ID FROM USERS WHERE USER_NAME = '{username}'"; // Assume the table is 'users' and has 'user_id'

                    using (var cmd = new MySqlCommand(getUserIdSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32(0); // Assuming user_id is the first column
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
                return 0;
            }
        }
        public static LoginProcRespModel RawQueryChange(string username, string oldPassword, string newPassword, string confirmPassword)
        {
            string connStr = "server=172.16.1.132;user=training_user;database=TRAINING;port=3306;password=Aksa@1234;";
            using (var conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Verify old password
                    string verifyOldPasswordSql = $"SELECT USER_ID FROM USERS WHERE USER_NAME = '{username}' AND PASSWORD = '{oldPassword}'"; // Add your actual password hash comparison method if needed
                    int userId = 0;
                    using (var cmd = new MySqlCommand(verifyOldPasswordSql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0); // Assuming user_id is the first column
                            }
                        }
                    }

                    if (userId == 0)
                    {
                        // Old password does not match
                        return new LoginProcRespModel { Code = "01", Msg = "OLD PASSWORD INCORRECT" };
                    }

                    if (newPassword != confirmPassword)
                    {
                        // New password and confirm password do not match
                        return new LoginProcRespModel { Code = "02", Msg = "PASSWORD MISMATCH" };
                    }

                    // Update to new password
                    string updatePasswordSql = $"UPDATE USERS SET PASSWORD = '{newPassword}' WHERE USER_ID = {userId}";
                    using (var cmd = new MySqlCommand(updatePasswordSql, conn))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return new LoginProcRespModel { Code = "00", Msg = "UPDATED" };
                        }
                        else
                        {
                            return new LoginProcRespModel { Code = "99", Msg = "UPDATE FAIL" };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new LoginProcRespModel { Code = "99", Msg = "EXCEPTION: " + ex.Message };
                }
            }
        }

        public static LoginProcRespModel RawQueryChangeWithoutOldPassword(int userId, string newPassword)
        {
            string connStr = "server=172.16.1.132;user=training_user;database=TRAINING;port=3306;password=Aksa@1234;";
            using (var conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // Update to new password
                    string updatePasswordSql = $"UPDATE USERS SET PASSWORD = '{newPassword}' WHERE USER_ID = {userId}";
                    using (var cmd = new MySqlCommand(updatePasswordSql, conn))
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return new LoginProcRespModel { Code = "00", Msg = "UPDATED" };
                        }
                        else
                        {
                            return new LoginProcRespModel { Code = "99", Msg = "UPDATE FAIL" };
                        }
                    }
                }
                catch (Exception ex)
                {
                    return new LoginProcRespModel { Code = "99", Msg = "EXCEPTION: " + ex.Message };
                }
            }
        }

    }
}
