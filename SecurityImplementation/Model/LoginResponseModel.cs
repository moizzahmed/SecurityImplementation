namespace SecurityImplementation.Model
{
    public class LoginResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public string Activities { get; set; }
        public int UserID { get; set; }
    }

    public class LoginProcRespModel
    {
        public string Code { get; set; }
        public string Desc { get; set; }
        public string Msg { get; set; }
    }
}
