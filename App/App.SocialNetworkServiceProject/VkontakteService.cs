using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace App.SocialNetworkServiceProject
{
    class VkontakteService : ISocialNetworkService
    {
        public string UserOAuthWithoutBrowser(string login, string password)
        {
            throw new NotImplementedException();
        }

        public string UserOAuthWithoutBrowser(string token)
        {
            throw new NotImplementedException();
        }

        public string UserOAuthInBrowser(string login, string password)
        {
            throw new NotImplementedException();
        }

        public string AppOAuth(string login, string password)
        {
            throw new NotImplementedException();
        }

        public string AppOAuth(string token)
        {
            throw new NotImplementedException();
        }

        public List<T> GetLastEntries<T>(string username, int count)
        {
            throw new NotImplementedException();
        }

        public string PostEntry(string message)
        {
            throw new NotImplementedException();
        }

        public string MakeRequest(Url url, string data)
        {
            throw new NotImplementedException();
        }

        public List<T> GetLastEntries<T>(int count)
        {
            throw new NotImplementedException();
        }
    }
}