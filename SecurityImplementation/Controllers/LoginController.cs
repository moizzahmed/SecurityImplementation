using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecurityImplementation.Helper;
using SecurityImplementation.Model;
using System.Data.SqlClient;

namespace SecurityImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("LoginUnEncrypted")]
        public IActionResult Login([FromBody] LoginRequestModel login)
        {
            // Implement your login logic here
            // For demonstration, let's assume a successful login
            return Ok(new LoginResponseModel { Status = "SUCCESS", Message = $"{login.username} is Logged in successfully, No Encryption/Decryption applied" });
        }

        [HttpPost("LoginEncryptedAES")]
        public IActionResult LoginEncryptedAES([FromBody] LoginRequestModel login)
        {
            // Implement your login logic here
            // For demonstration, let's assume a successful login
            return Ok(new LoginResponseModel { Status = "SUCCESS", Message = $"{login.username} is Logged in successfully, Symmetric Encryption/Decryption applied" });
        }

        [HttpPost("LoginEncryptedRSA")]
        public IActionResult LoginEncryptedRSA([FromBody] LoginRequestModel login)
        {
            // Implement your login logic here
            // For demonstration, let's assume a successful login
            return Ok(new LoginResponseModel { Status = "SUCCESS", Message = $"{login.username} is Logged in successfully, A-Symmetric Encryption/Decryption applied" });
        }

        [HttpPost("SQLInjection")]
        public IActionResult SQLInjection([FromBody] LoginRequestModel login)
        {

            SqlDataReader dr = new DB().GetDataReader("Select Email,Name from UserInfo where Email='" + login.username + "' AND Password='" + login.password + "'");

            if (dr != null)
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    //Session["UserName"] = dr["Name"];
                    //Response.Redirect("~/Home.aspx?Email=" + dr["Email"]);
                }
                else
                {
                    //lblMsg1.Text = "Check your credentials";
                }
            }
            dr.Close(); dr.Dispose(); dr = null;

            // Implement your login logic here
            // For demonstration, let's assume a successful login
            return Ok(new LoginResponseModel { Status = "SUCCESS", Message = $"{login.username} is Logged in successfully, A-Symmetric Encryption/Decryption applied" });
        }
    }
}
