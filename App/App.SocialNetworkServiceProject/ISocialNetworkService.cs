using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace App.SocialNetworkService
{
    interface ISocialNetworkService
    {
        string UserOAuth();

        string AppOAuth();
        
        List<string> GetLastEntries(string username, int count);

        string PostEntry(string message);

    }
}
