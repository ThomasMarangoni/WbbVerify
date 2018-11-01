/*
*	This File is under the license of Creative Commons BY-NC 3.0 AT
*	Author: Thomas Marangoni/DasChaos
*	Created: 30.07.2018
*
*	You are only allowed to use this file in non-commercial way.
*	If you are using this file, you have to mark the author, add a link to the License
*	and announce any changes. 
*
*	https://creativecommons.org/licenses/by-nc/3.0/at/
*
*/

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace WbbVerify
{
    public class WbbVerify
    {
        private const string Key = ""; //128-Character Key
        private const string RequestUrl = "";  //URL to PHP-File

        private static readonly HttpClient Client = new HttpClient();

        public enum LoginStatusCode
        {
            Error = 0,
            KeyWrong = 1,
            DataMissing = 2,
            Success = 10,
            WrongPasswordUsername = 11
        }

        public class LoginUserData
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public bool Banned { get; set; }
            public string BanReason { get; set; }
            public bool Whitelisted { get; set; }
        }

        public class LoginResponse
        {
            public LoginStatusCode StatusCode { get; set; }
            public LoginUserData UserData { get; set; }

            public LoginResponse(LoginStatusCode statusCode, LoginUserData userData)
            {
                StatusCode = statusCode;
                UserData = userData;
            }
        }

        private static async Task<LoginResponse> MakePostRequest(string requestUrl, string username, string password, string key)
        {
            var values = new Dictionary<string, string>
            {
                { "Username", username },
                { "Password", password },
                { "Key", key }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await Client.PostAsync(requestUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<LoginResponse>(responseString);
        }

        public static LoginResponse VerifyUser(string username, string password)
        {
            LoginResponse loginInfo;
            try
            {
                loginInfo = MakePostRequest(RequestUrl, username, password, Key).Result;
            }
            catch (Exception err)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception in WBB-Verify\n\n ERROR-Message START\n" + err.Message + "ERROR-Message End\n");
                Console.ResetColor();

                return new LoginResponse(LoginStatusCode.Error, null);
            }

            switch (loginInfo.StatusCode)
            {
                case LoginStatusCode.Success:
                    return loginInfo;
                case LoginStatusCode.WrongPasswordUsername:
                    return new LoginResponse(LoginStatusCode.WrongPasswordUsername, null);
                case LoginStatusCode.DataMissing:
                    return new LoginResponse(LoginStatusCode.DataMissing, null);
                case LoginStatusCode.KeyWrong:
                    return new LoginResponse(LoginStatusCode.KeyWrong, null);
                default:
                    return new LoginResponse(LoginStatusCode.Error, null);
            }
        }
    }
}
