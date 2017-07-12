using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.SocialNetworkService;
using App.StatisticService;
using Newtonsoft.Json;

namespace App
{
    class Program
    {
        private List<string> usernames = new List<string>();

        static void Main(string[] args)
        {
            ISocialNetworkService networkService = new TwitterService();
            networkService.AppOAuth();
            var result = TextStatisticService.GetLetterFrequency
                (String.Join
                    (String.Empty, networkService.GetLastEntries("AsWeslyG", 5)));

            var json = JsonConvert.SerializeObject(result, Formatting.None);

            var str = new StringBuilder();
            str.Append("@AsWeslyG");
            str.Append(", статистика для последних 5 твитов: ");
            str.Append(json);
            str.Length = 140;

            networkService.UserOAuth();
            networkService.PostEntry(str.ToString());

            Console.ReadKey();
        }

    }

}