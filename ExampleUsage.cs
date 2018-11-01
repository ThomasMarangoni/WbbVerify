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

namespace WbbVerify
{
    public class ExampleUsage
    {
        public static void ExampleMethod(string username, string password)
        {
            WbbVerify.LoginResponse response = WbbVerify.VerifyUser(username, password);

            switch (response.StatusCode)
            {
                case WbbVerify.LoginStatusCode.DataMissing:
                    Console.WriteLine("Login Failed: username or password were missing.");
                    break;
                case WbbVerify.LoginStatusCode.Error:
                    Console.WriteLine("Login Failed: An unknown error occured, see console log for more information.");
                    break;
                case WbbVerify.LoginStatusCode.KeyWrong:
                    Console.WriteLine("Login Failed: The Anti-Bruteforce Key is wrong");
                    break;
                case WbbVerify.LoginStatusCode.WrongPasswordUsername:
                    Console.WriteLine("Login Failed: The username or password was incorrect.");
                    break;
                case WbbVerify.LoginStatusCode.Success:
                    Console.WriteLine("Login Success");
                    break;
            }
        }
    }
}

