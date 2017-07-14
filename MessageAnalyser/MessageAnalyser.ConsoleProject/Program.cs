using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MessageAnalyzer.SocialNetworkService;
using MessageAnalyzer.SocialNetworkService.Factory;
using MessageAnalyzer.StatisticService;
using Newtonsoft.Json;

namespace MessageAnalyzer.ConsoleProject
{
    internal class Program
    {
        private const int AmountEntry = 5;
        private static ISocialNetworkService _networkService;

        public static Dictionary<string, User> UserStore = new Dictionary<string, User>();

        private static readonly Dictionary<string, Action<IEnumerable<string>>> AllCommandDictionary =
            new Dictionary<string, Action<IEnumerable<string>>>
            {
                {"help", Execute(() => { Console.WriteLine(File.ReadAllText(@"~/../../../help.txt")); })},

                {"clear", Execute(Console.Clear)},

                {"exit", Execute(() => Environment.Exit(0))},

                {"add", Execute(ReadLogins)},

                {"stat", Execute(MakeStatistic)},

                {"logout", Execute(LogOut)},

                {"print", ExecuteWithParams(args => PrintStatisticByUser(args.ElementAt(0)))},

                {"post", ExecuteWithParams(args => PostStatisticByUser(args.ElementAt(0)))}
            };

        public static void Main(string[] args)
        {
            Init();

            while (true)
            {
                Console.WriteLine("\nПожалуйста, введите комманду.");
                Console.Write(">> ");

                var readLine = Console.ReadLine();

                if (readLine != null)
                {
                    var tokens = readLine.Split(' ').ToList();

                    var commandName = tokens[0].ToLower();
                    if (string.IsNullOrEmpty(commandName)) continue;

                    var commandArgs = new string[10];
                    if (tokens.Count > 1) commandArgs = new[] {tokens[1]};

                    if (!AllCommandDictionary.ContainsKey(commandName))
                    {
                        Console.WriteLine($"Такой комманды нет: {commandName}");
                        continue;
                    }

                    try
                    {
                        AllCommandDictionary[commandName](commandArgs);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Выполнение с ошибкой: {e.Message}");
                    }
                }
            }
        }

        private static void LogOut()
        {
            _networkService.UserLogOut();
            Console.WriteLine("Пользователь разлогинился.");
        }

        private static void ReadLogins()
        {
            Console.WriteLine("Введите пустую строку, чтобы закончить добавление аккаунтов.");
            Console.Write(" >> ");
            var readLine = Console.ReadLine();

            while (!string.IsNullOrEmpty(readLine))
            {
                var regex = new Regex("@{0,1}(\\w+)");
                var username = regex.Match(readLine).Groups[1].Value;

                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine($"Невалидное имя: {readLine}");
                    Console.Write(" >> ");
                    readLine = Console.ReadLine();
                    continue;
                }

                if (UserStore.ContainsKey(username))
                {
                    Console.WriteLine($"Такой пользоватеь уже добавлен: {username}");
                    Console.Write(" >> ");
                    readLine = Console.ReadLine();
                    continue;
                }

                UserStore.Add(username, new User {Name = username});
                Console.Write(" >> ");
                readLine = Console.ReadLine();
            }
        }

        private static void MakeStatistic()
        {
            if (UserStore.Keys.Count == 0)
            {
                Console.WriteLine("Список аккаунтов пуст. Добавьте с помощью команды add.");
                return;
            }

            Console.WriteLine("Подождите. Это может занять некоторое время.");

            foreach (var username in UserStore.Keys)
            {
                var user = UserStore[username];

                if (user.Enties != null)
                    continue;

                var entries = _networkService.GetLastEntries(username, AmountEntry);
                user.Enties = entries;

                var etriesToString = string.Join(string.Empty, entries);
                user.Statistic = TextStatisticService.GetLetterFrequency(etriesToString);
            }

            Console.WriteLine(
                "Статистика успешно подсчитана. Посмотреть можно с помощью команды print с аргументом username.");
        }

        private static void PrintStatisticByUser(string username)
        {
            var regex = new Regex("\\@");
            username = regex.Replace(username, string.Empty);

            if (IsUserKnown(username))
                Console.WriteLine(GetJsonStatistic(username));
        }

        private static void PostStatisticByUser(string username)
        {
            var regex = new Regex("\\@");
            username = regex.Replace(username, string.Empty);

            if (IsUserKnown(username))
            {
                try
                {
                    _networkService.UserOAuth();
                    _networkService.PostEntry(GetJsonStatistic(username).Substring(0, 140));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                Console.WriteLine("Успешно опубликован ваш твит");
            }
        }

        private static string GetJsonStatistic(string username)
        {
            var etriesToString = string.Join(string.Empty, UserStore[username].Enties);
            var letterFrequency = TextStatisticService.GetLetterFrequency(etriesToString);
            var json = JsonConvert.SerializeObject(letterFrequency, Formatting.None);

            return $"@{username}, статистика для последних твитов: {json}";
        }

        private static bool IsUserKnown(string username)
        {
            if (!UserStore.ContainsKey(username))
            {
                Console.WriteLine("Этот пользователь не был добавлен в список. Добавьте его с помощью команды add.");
                Console.WriteLine("Будьте внимательны, имя пользователя чувствительно к регистру.");
                return false;
            }

            if (UserStore[username].Statistic == null)
            {
                Console.WriteLine("Статистика для этого пользователя не посчитана. Запустите команду stat.");
                return false;
            }
            return true;
        }

        private static void Init()
        {
            Console.OutputEncoding = Encoding.UTF8; //для корректного вывода русских символов в консоли

            Console.WriteLine(@"Привет, я TwitterBot!");
            Console.WriteLine(@"Справка по командам - help");

            _networkService = SocialNetworkFactory.Create(SocialNetworkType.Twitter);
            _networkService.AppOAuth(); //авторизуем приложение
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
                var enumerable = args as string[] ?? args.ToArray();
                if (enumerable.Any(i => i == null))
                    throw new ArgumentException("Необходимы аргументы");
                aсtion(enumerable);
            };
        }
    }
}