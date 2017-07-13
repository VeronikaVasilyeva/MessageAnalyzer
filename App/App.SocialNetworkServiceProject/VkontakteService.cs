using System;
using System.Collections.Generic;

namespace App.SocialNetworkService
{
    public class VkontakteService : ISocialNetworkService
    {
        public string UserOAuth()
        {
            throw new NotImplementedException();
        }

        public string AppOAuth()
        {
            throw new NotImplementedException();
        }

        public List<string> GetLastEntries(string username, int count)
        {
            throw new NotImplementedException();
        }

        public string PostEntry(string message)
        {
            throw new NotImplementedException();
        }
    }
}