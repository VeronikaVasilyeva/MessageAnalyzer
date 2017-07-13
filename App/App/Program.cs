using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using App.SocialNetworkService;
using App.StatisticService;
using Newtonsoft.Json;

namespace App
{
    class Program
    {
        private static readonly Dictionary<string, Action<IEnumerable<string>>> AllCommandDictionary =
            new Dictionary<string, Action<IEnumerable<string>>>
            {
                { "help", Execute(() => { Console.WriteLine(File.ReadAllText("\\help.txt")); })},

                { "clear", Execute(Console.Clear) },

                { "exit", Execute(() => Environment.Exit(0))},

                { "add", Execute(ReadLogins)},
                { "stat" , Execute( MakeStatistic)},

                { "print" , ExecuteWithParams( args => PrintStatisticByUser(args.ElementAt(0)) )},
                { "post", ExecuteWithParams( args => PostStatisticByUser(args.ElementAt(0)))},
                { "logout", Execute(LogOut)}
            };

        private static void LogOut()
        {
            NetworkService.UserLogOut();
        }

        public static Dictionary<string, User> Users = new Dictionary<string, User>();

        private static readonly string _messagePart = ", статистика для последних твитов: ";
        private static readonly int _amountEntry = 5;

        private static readonly ISocialNetworkService NetworkService = new TwitterService();

        public static void Main(string[] args)
        {
            Console.WriteLine("Привет, я TwitterBot!");

            while (true)
            {
                NetworkService.AppOAuth();

                Console.WriteLine("Пожалуйста, введите комманду. help - список всех комманд.");
                Console.WriteLine(">> ");

                string readLine = Console.ReadLine();

                if (readLine != null)
                {
                    List<string> tokens = readLine.Split(' ').ToList<string>();

                    string commandName = tokens[0];
                    if (string.IsNullOrEmpty(commandName)) continue;

                    IEnumerable<string> commandArgs = new[] {tokens[1]};
                    
                    if (AllCommandDictionary.ContainsKey(commandName))
                    {
                        try
                        {
                            AllCommandDictionary[commandName](commandArgs);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Выполнение с ошибкой: " + e.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Такой комманды нет: " + commandName);
                    }
                }
            }

        }


        private static void ReadLogins()
        {
            var listLogins = new List<string>();
            var readLine = Console.ReadLine();

            while (!string.IsNullOrEmpty(readLine))
            {
                var regex = new Regex("$@{0,1}(\\w+)");
                var username = regex.Match(readLine).Value;

                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Невалидное имя:" + username);
                    continue; 
                }

                if (Users.ContainsKey(username))
                {
                    Console.WriteLine("Такой пользоватеь уже добавлен: " + username);
                    continue;
                }

                Users.Add(username, new User { Name = username });
                readLine = Console.ReadLine();
            }
        }

        private static string GetJsonStatistic(string username)
        {
            var etriesToString = String.Join(String.Empty, Users[username].Enties);
            var letterFrequency = TextStatisticService.GetLetterFrequency(etriesToString);
            var json = JsonConvert.SerializeObject(letterFrequency, Formatting.None);

            return "@" + username + _messagePart + json;
        }

        private static void MakeStatistic()
        {
            foreach (var username in Users.Keys)
            {
                var user = Users[username];

                var entries = NetworkService.GetLastEntries(username, _amountEntry);
                user.Enties = entries;

                var etriesToString = String.Join(String.Empty, entries);
                user.Statistic = TextStatisticService.GetLetterFrequency(etriesToString);
            }
        }

        private static void PrintStatisticByUser(string username)
        {
            Console.WriteLine(GetJsonStatistic(username));
        }

        private static void PostStatisticByUser(string username)
        {
            NetworkService.UserOAuth();
            NetworkService.PostEntry(GetJsonStatistic(username).Substring(0, 140));
        }

        private static Action<IEnumerable<string>> Execute(Action aсtion)
        {
            return args =>
            {
                if (args.Any())
                    throw new ArgumentException("Эта комманда не поддерживает аргументы");
                aсtion();
            };
        }

        private static Action<IEnumerable<string>> ExecuteWithParams(Action<IEnumerable<string>> aсtion)
        {
            return aсtion;
        }
    }

}