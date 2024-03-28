namespace SecurityImplementation.Model
{
    public class LoginRequestModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string randomString { get; set; }
    }

    public class ChangePasswordRequestModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string newPassword { get; set; }
    }
}
