using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace App.SocialNetworkServiceProject
{
    interface ISocialNetworkService
    {
        string UserOAuthWithoutBrowser(string login, string password);

        string UserOAuthWithoutBrowser(string token);

        string UserOAuthInBrowser(string login, string password);

        //return access_token, 
        string AppOAuth(string login, string password);

        string AppOAuth(string token);
        
        List<T> GetLastEntries<T>(string username, int count);

        string PostEntry(string message);

        string MakeRequest(Url url, string data);
    }
}
