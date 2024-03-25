using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SecurityImplementation.Helper;
using SecurityImplementation.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SecurityImplementation.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AntiXssMiddleware
    {

        private readonly RequestDelegate _next;

        private bool ExcludeThisUrl(string urlPath)
        {
            string ep = urlPath.Split('/').Last();

            if (ep.ToLower().Contains("loginunencrypted")) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public AntiXssMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            // Check XSS in request content

            bool doDecrypt = true;
            string decryptedRequest = "";
            LoginRequestModel loginReq = new LoginRequestModel();
            string encryptionKeyFromRequest = "";
            string urlPath = context.Request.Path.Value;

            var buffer = new MemoryStream();
            var originalBody = context.Request.Body;
            var stream = context.Response.Body;


            if (ExcludeThisUrl(urlPath) || context.Request.HasFormContentType)
            {
                doDecrypt = false;
            }
            else
            {
                context.Response.Body = buffer;
            }

            try
            {

                var content = await ReadRequestBody(context);
                int matchIndex;
                if (!string.IsNullOrEmpty(content))
                {
                    #region Decrypt Request
                    if (doDecrypt && !string.IsNullOrEmpty(content))
                    {
                        // clsEncrypt_Decrypt clsEncrypt_Decrypt = new clsEncrypt_Decrypt();
                        ClientReq encryptedRequest = null;

                        try
                        {
                            encryptedRequest = JsonConvert.DeserializeObject<ClientReq>(content);
                        }
                        catch { }

                        if (encryptedRequest != null)
                        {
                            if (urlPath.ToLower().Contains("rsa"))
                                decryptedRequest = clsEncrypt_Decrypt.decryptRSA_Key(encryptedRequest.request, GlobalInfo._PEM_KEY);
                            else if (urlPath.ToLower().Contains("aes"))
                                decryptedRequest = clsEncrypt_Decrypt.Decrypt(encryptedRequest.request, "BgSFaPTML8gKaS55");

                            loginReq = JsonConvert.DeserializeObject<LoginRequestModel>(decryptedRequest);

                            encryptionKeyFromRequest = loginReq?.randomString;

                            //Will be required when setting key inresponse                            

                            byte[] bytes = Encoding.ASCII.GetBytes(decryptedRequest);
                            context.Request.Body = new MemoryStream(bytes);
                        }
                    }
                    #endregion
                }

                await _next(context);

                #region Encrypt Response
                if (context.Response.StatusCode == 200 && doDecrypt)
                {
                    string newId = "";
                    buffer.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(buffer);
                    using (var bufferReader = new StreamReader(buffer))
                    {
                        string body = await bufferReader.ReadToEndAsync();

                        //Setting security info like key, salt etc.
                        var responseObj = JsonConvert.DeserializeObject<LoginResponseModel>(body);

                        if (responseObj != null)
                        {
                            body = JsonConvert.SerializeObject(responseObj);
                        }

                        if (urlPath.ToLower().Contains("rsa"))
                            decryptedRequest = clsEncrypt_Decrypt.Encrypt(body, encryptionKeyFromRequest);
                        else if (urlPath.ToLower().Contains("aes"))
                            decryptedRequest = clsEncrypt_Decrypt.Encrypt(body, "BgSFaPTML8gKaS55");

                        ClientResp clientResponse = new ClientResp();
                        clientResponse.Response = decryptedRequest;
                        var jsonString = JsonConvert.SerializeObject(clientResponse);

                        //// Added new code
                        context.Response.Clear();
                        await context.Response.WriteAsync(jsonString);
                        context.Response.Body.Seek(0, SeekOrigin.Begin);

                        // below code is not working requires CopyToAsync.
                        //context.Response.Body.CopyTo(stream);
                        await context.Response.Body.CopyToAsync(stream);
                        context.Response.Body = stream;
                    }
                }
                #endregion


            }
            catch (Exception ex)
            {

                context.Response.Clear();
                context.Response.StatusCode = 403;

                await context.Response.WriteAsync(ex.Message);
                return;
            }
            finally
            {
                context.Request.Body = originalBody;

                buffer.Dispose();
                originalBody.Dispose();
                stream.Dispose();

            }
        }

        private static async Task<string> ReadRequestBody(HttpContext context)
        {
            var buffer = new MemoryStream();
            await context.Request.Body.CopyToAsync(buffer);
            context.Request.Body = buffer;
            buffer.Position = 0;

            var encoding = Encoding.UTF8;
            //   var contentType = context.Request.GetTypedHeaders().ContentType;
            // if (contentType?.Charset != null) encoding = Encoding.GetEncoding(contentType.Charset);

            var requestContent = await new StreamReader(buffer, encoding).ReadToEndAsync();
            context.Request.Body.Position = 0;

            return requestContent;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AntiXssMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAntiXssMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AntiXssMiddleware>();
        }
    }
}

