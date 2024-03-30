using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecurityImplementation.Helper;
using SecurityImplementation.Model;
using MySql.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace SecurityImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("LoginUnEncrypted")]
        public IActionResult Login([FromBody] LoginRequestModel login)
        {
            LoginProcRespModel LoginResponse = new LoginProcRespModel();
            LoginResponse = Procedure.LoginAuthenticate(login);

            if (LoginResponse.Code == "0")
            {
                return
                Ok(new LoginResponseModel
                {
                    Username = login.username,
                    Status = "SUCCESS",
                    UserID = Procedure.GetUserId(login.username),
                    CSRF = Convert.ToBase64String(Encoding.UTF8.GetBytes(login.username + "," + Convert.ToString(DateTime.Now))),
                    Message = $"{login.username} is Logged in successfully, No Encryption/Decryption applied"
                });
            }

            return Ok(new LoginResponseModel { Status = "FAIL", Message = $"{login.username} FAILED TO LOGIN" });
        }

        [HttpGet("MapActivities/{userId}")]
        public IActionResult Map([FromRoute] int userId)
        {
            if (userId.Equals(1))
            {
                return Ok(new { Activities = "1,2,3" });
            }
            else
            {
                return Ok(new { Activities = "1" });
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout([FromBody] LoginRequestModel user)
        {
            return Ok(new LoginResponseModel { Status = "SUCCESS", Message = $"{user.username} LOGGED OUT SUCCESFULLY." });
        }

        [HttpPost("OTPVerify")]
        public IActionResult OTPVerify([FromBody] LoginRequestModel user)
        {
            return Ok(new LoginResponseModel { Status = "SUCCESS", Message = $"{user.username} Your OTP is Verified." });
        }

        [HttpGet("Search/{query}")]
        public IActionResult Search([FromQuery] string query)
        {
            return Ok();
        }

        [HttpPost("LoginEncryptedAES")]
        public IActionResult LoginEncryptedAES(LoginRequestModel login)
        {
            LoginProcRespModel LoginResponse = new LoginProcRespModel();
            LoginResponse = Procedure.LoginAuthenticate(login);

            if (LoginResponse.Code == "0")
            {
                return
                Ok(new LoginResponseModel
                {
                    Username = login.username,
                    Status = "SUCCESS",
                    UserID = Procedure.GetUserId(login.username),
                    CSRF = Convert.ToBase64String(Encoding.UTF8.GetBytes(login.username + "," + Convert.ToString(DateTime.Now))),
                    Message = $"{login.username} is Logged in successfully, No Encryption/Decryption applied"
                });
            }

            return Ok(new LoginResponseModel { Status = "FAIL", Message = $"{login.username} FAILED TO LOGIN" });
        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword([FromBody] LoginRequestModel user)
        {
            var newPassword = GenerateRandomPassword(8);

            Procedure.RawQueryReset(user.username, newPassword);

            return Ok(new { Username = user.username, NewPassword = newPassword });
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequestModel user)
        {
            var resp = Procedure.RawQueryChange(user.username, user.password, user.newPassword, user.newPassword);

            if (resp.Code == "00")
                return Ok(new { Username = user.username, NewPassword = user.newPassword });
            else
                return Ok(new { Username = user.username, NewPassword = "" });
        }

        [HttpPost("ChangePasswordWithoutOld")]
        public IActionResult ChangePasswordWithoutOld([FromHeader(Name = "userId")] int userId, [FromBody] ChangePasswordRequestModel user)
        {
            var resp = Procedure.RawQueryChangeWithoutOldPassword(userId, user.newPassword);

            if (resp.Code == "00")
                return Ok(new { status = "SUCCESS" });
            else
                return Ok(new { status = "FAIL" });
        }

        private string GenerateRandomPassword(int length)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var random = new Random();
            var chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }
            return new string(chars);
        }
    }
}
