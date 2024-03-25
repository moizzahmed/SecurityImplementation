using System;
using System.IO;

/// <summary>
/// Summary description for GlobalInfo
/// </summary>
public class GlobalInfo
{
    public GlobalInfo()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static string _PEM_KEY = @"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEA0djjOAxOYd4qUFlXTvvFhsrDUIvOCYg9VV8t5eoDXaxIjz82
tMLX5M7sCpFykCSNUTYBIePM0jP2YVZax7nVzEpqZabUVU4SgFzYj0Uhhn4W/6No
7me6Zt3anTuW0/JtmgWxqhZIVbDRhrOhMng67s0kmsAT3GCCB0yPbQmi7NXGUCEr
flCf4k+O0Fc525jWZZAN3O0SAYsIg3P5YjYeoEgfuux1a2loTD27VJB30wb1JHVZ
gz6+sK+TAQrcMM3bUYMlB53x5mgOp8nqHmdaa9JOnfKFU0mvaz/KIM8XF310TK0t
Gbn5o9k1b6/1cnN4OsSwqQPyNCz0J129+MXHMwIDAQABAoIBAAeYVgD9UQjxxY5K
7YIKUsfiJtePd9GYgL/KRhsAMUb+qVGl9BDuzGmXo6tuQDs/V24NClVzrUIeZ1h2
i6qW60oNl3Q+aYj83maO9beZxPDdN2/hNjcKwhBfDz6JnZfmOllMw3VogQ247Jx3
e+obt+ZUvnJraUycoZgqCTblFmSD93ZlQlG5zJkhDNLioBgO5+dI8gvYFES2H2N5
MlAkgt41W2MoNXk9TWVCYc26XfI/5tNr/+84265RGhpBfcI+ta8X5GBSoaVnBlsb
2AcNGONl04LiMTT3bBSXoML2mx+7rnfcc+GohtRYrYHLGaJ8Rip2eKT0HMwN95KB
Z2PEOUECgYEA5/35PtBeTV0mLXNS4KqJdEMFRD0wc0GJaFgwAAsNYJ/wZ3ZSx42C
5njOZwsrqiRtVE+JCrr5+70/AO07qjuSNCV5JN2aTPfg3NpacRyck9H9K7ty2YQO
YXpk5LiMLuJPEnv0xt/eZQKY6cBApB1zG6A7jV291zUwaKcQlpgSIUkCgYEA55A+
S8OE2cJJHZfJvzc2PaKqlweMQdk4AmzSPkS5CbAFhiKKEd2Zz7APKRrAXBPZ1gEV
Y5aJhU5x0gnsjKTiutX1xJDd36PFYslQDTNRCsGwIKZO4P/yO87IL8/wOfqZNPew
qAx8HxzFfe2I2ZRBeocKUEDUz24RTpR8b8cOoJsCgYEAxuMkYGaQwjB+f26j+boa
d1LR3Au9UsI3w/3+wLq48EKN/pUhKLU4KNAe25ZnC8mI9UPukAV2NQysS+YWRb4m
fJA3yJQ3KY9E9vI3oUtPLSdUrb5ZGwOstIMSpkdU3wjjk0wzsJ/ScLSGVbedc1VQ
DroS9AIOs8aExuObRBABUwkCgYEAgUClVRrke7wTnb4M8Xu6/fpfKAAhjNvXhJsD
W5h2hG7JFo8O7Fv6L9BBFhuFhZ/a9rYSH012o38ezzOU4whiOhGVpkuKXPuIANUH
puae9NcaHY9W9gZHSpTobq+tkl9LxyH+bD7TxXYE0n6U5YX4apEX86XMY6A3bto4
/xdoFcMCgYBVRlOj+tO6qqBK5CY2GkVQ2V/FEgUBmFFeu2HZgBVKPp7K0FP/fpYX
IuCpGkeeR58waDcHFB5vN4Z3+qqFhb4cTTj/DuSbvHCBrL4gIMOWxJmup9ZKYkmb
bZdd4TKkax675aXwgTvJlD23VtY64XqRx5Qgvfv+YVxM5AkTX0lviw==
-----END RSA PRIVATE KEY-----
";


    internal static string GetFolderPath(string path)
    {
        DateTime date = DateTime.Now;
        string folderPath = path;
        
        folderPath = folderPath + date.Year.ToString("D4"); 
        if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }

        folderPath = folderPath + "\\" + date.Month.ToString("D2"); 
        if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }

        folderPath = folderPath + "\\" + date.Day.ToString("D2");
        if (!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }

        return folderPath + "\\";

    }
       

}
