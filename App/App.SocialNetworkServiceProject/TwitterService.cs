using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.SocialNetworkServiceProject
{
    class TwitterService : ISocialNetworkService
    {
        private readonly static string ConsumerKey = "5PFhmG58BgUq9MkmSOXdr9P9o";
        private readonly static string ConsumerSecret = "9K9uIdrgN7j3XGg8SDvLKpVN0q1WPR0BfVDWGWRSH0qd9oA05M";

        public string UserOAuth(string login, string password)
        {
            throw new NotImplementedException();
        }

        public string UserOAuth(string token)
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

        public string PublishEntry(string message)
        {
            throw new NotImplementedException();
        }

        public List<T> GetLastEntries<T>(int count)
        {
            throw new NotImplementedException();
        }
    }
}
