using System;
using System.Collections.Generic;
using System.Text;
using App.SocialNetworkService;
using App.StatisticService;
using Newtonsoft.Json;

namespace App
{
    class Program
    {
        private List<string> _usernames = new List<string>();
        private static readonly string _messagePart = ", статистика для последних твитов: ";
        private static readonly int _amountEntry = 150;
        
        static void Main(string[] args)
        {
            ISocialNetworkService networkService = new TwitterService();
            networkService.AppOAuth();

            var username = "AsWeslyG";
            var entries = networkService.GetLastEntries(username, _amountEntry);

            var message = MakePostEntry(username, entries);

            networkService.UserOAuth();
            networkService.PostEntry(message);

            Console.WriteLine("Press enter...");
            Console.ReadLine();
        }

        private static string MakePostEntry(string username, List<string> entries)
        {
            var etriesToString = String.Join(String.Empty, entries);
            var letterFrequency = TextStatisticService.GetLetterFrequency(etriesToString);
            var json = JsonConvert.SerializeObject(letterFrequency, Formatting.None);

            var str = new StringBuilder();
            str.Append("@");
            str.Append(username);
            str.Append(_messagePart);
            str.Append(json);
            str.Length = 140;

            return str.ToString();
        }
    }

}