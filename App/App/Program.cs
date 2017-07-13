﻿using System;
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
                { "help",   Execute(() => { Console.WriteLine(File.ReadAllText(@"~/../../../help.txt")); })},

                { "clear",  Execute(Console.Clear) },

                { "exit",   Execute(() => Environment.Exit(0))},

                { "add",    Execute(ReadLogins)},

                { "stat" ,  Execute( MakeStatistic)},

                { "logout", Execute(LogOut)},

                { "print" , ExecuteWithParams( args => PrintStatisticByUser(args.ElementAt(0)) )},
                { "post",   ExecuteWithParams( args => PostStatisticByUser(args.ElementAt(0)))},
            };

        public static Dictionary<string, User> Users = new Dictionary<string, User>();

        private static readonly string _messagePart = ", статистика для последних твитов: ";
        private static readonly int _amountEntry = 5;

        private static readonly ISocialNetworkService NetworkService = new TwitterService();

        public static void Main(string[] args)
        {
            Console.WriteLine("Привет, я TwitterBot!");
            Console.WriteLine("Справка по командам - help");

            while (true)
            {
                NetworkService.AppOAuth();

                Console.WriteLine("\nПожалуйста, введите комманду.");
                Console.Write(">> ");

                string readLine = Console.ReadLine();

                if (readLine != null)
                {
                    List<string> tokens = readLine.Split(' ').ToList<string>();

                    string commandName = tokens[0].ToLower();
                    if (string.IsNullOrEmpty(commandName)) continue;

                    var commandArgs = new String[10];

                    if (tokens.Count > 1)
                    {
                        commandArgs = new[] { tokens[1] };
                    }
                    
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

        private static void LogOut()
        {
            NetworkService.UserLogOut();
        }

        private static void ReadLogins()
        {
            Console.WriteLine("Чтобы закончить добавление аккаунтов, введите пустую строку.");
            Console.Write(" >> ");
            var readLine = Console.ReadLine();

            while (!string.IsNullOrEmpty(readLine))
            {
                var regex = new Regex("@{0,1}(\\w+)");
                var username = regex.Match(readLine).Groups[1].Value;

                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Невалидное имя:" + readLine);
                    continue; 
                }

                if (Users.ContainsKey(username))
                {
                    Console.WriteLine("Такой пользоватеь уже добавлен: " + username);
                    continue;
                }

                Users.Add(username, new User { Name = username });
                Console.Write(">> ");
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
            if (Users.Keys.Count == 0)
            {
                Console.WriteLine("Список аккаунтов пуст. Добавьте с помощью команды add.");
                return;
            }

            foreach (var username in Users.Keys)
            {
                var user = Users[username];

                if (user.Enties != null)
                {
                    continue;
                }

                var entries = NetworkService.GetLastEntries(username, _amountEntry);
                user.Enties = entries;

                var etriesToString = String.Join(String.Empty, entries);
                user.Statistic = TextStatisticService.GetLetterFrequency(etriesToString);
            }

            Console.WriteLine("Статистика успешно подсчитана. Посмотреть можно с помощью команды print с аргументом username.");
        }

        private static void PrintStatisticByUser(string username)
        {
            if (!Users.ContainsKey(username))
            {
                Console.WriteLine("Этот пользователь не был добавлен в список. Добавьте его с помощью команды add.");
                return;
            }

            if (Users[username].Statistic == null)
            {
                Console.WriteLine("Статистика для этогопользователя не посчитана. Запустите команду stat.");
                return;
            }

            Console.WriteLine(GetJsonStatistic(username));
        }

        private static void PostStatisticByUser(string username)
        {
            if (!Users.ContainsKey(username))
            {
                Console.WriteLine("Этот пользователь не был добавлен в список. Добавьте его с помощью команды add.");
                return;
            }

            if (Users[username].Statistic == null)
            {
                Console.WriteLine("Статистика для этогопользователя не посчитана. Запустите команду stat.");
                return;
            }

            try
            {
                NetworkService.UserOAuth();
                NetworkService.PostEntry(GetJsonStatistic(username).Substring(0, 140));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static Action<IEnumerable<string>> Execute(Action aсtion)
        {
            return args =>
            {
                if (args.Any(i => i != null))
                    throw new ArgumentException("Эта комманда не поддерживает аргументы");
                aсtion();
            };
        }

        private static Action<IEnumerable<string>> ExecuteWithParams(Action<IEnumerable<string>> aсtion)
        {
            return args =>
            {
                if (args.Any(i => i == null))
                    throw new ArgumentException("Необходимы аргументы");
                aсtion(args);
            };
        }
    }

}