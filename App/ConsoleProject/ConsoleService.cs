using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace App.ConsoleService
{
    public class ConsoleService
    {
            private static Dictionary<string, Action<IEnumerable<string>>> _allCommandDictionary =
                new Dictionary<string, Action<IEnumerable<string>>>
                {
                    { "help", ExecuteCommand(() => { Console.WriteLine(File.ReadAllText("\\help.txt")); })},

                    { "clear", ExecuteCommand(Console.Clear) },

                    { "exit", ExecuteCommand(() => Environment.Exit(0))},

                    { "add", ExecuteCommand(() => ReadLogins())},
                    { "stat" , ExecuteCommand(Console.Clear)  },

                    { "post", ExecuteCommand(Console.Clear)  },
                    { "logout", ExecuteCommand(Console.Clear)  }
                };

            public static List<string> ReadLogins()
            {
                return null;
            }

            private static Action<IEnumerable<string>> ExecuteCommand(Action aсtion)
            {
                return args =>
                {
                    if (args.Any())
                        throw new ArgumentException("this command doesn't support args");
                    aсtion();
                };
            }

        public static void Run()
        {
            Console.WriteLine("Привет, я TwitterBot!");

            while (true)
            {
                Console.WriteLine("Пожалуйста, введите комманду. Help - список всех комманд.");

                string readLine = Console.ReadLine();

                List<string> parts = readLine.Split(' ').ToList<string>();

                string commandName = parts[0];
                parts.RemoveAt(0);
                string[] commandArgs = parts.ToArray<string>();

                try
                {
                    string result = Registry.Execute(commandName, commandArgs);
                    if (result != null)
                    {
                        Console.WriteLine("[{0}] {1}", commandName, result);
                    }
                }
                catch (CommandNotFoundException)
                {
                    Console.WriteLine("[ConsolePlus] No such command.");
                }
            }
        }

    }


}
