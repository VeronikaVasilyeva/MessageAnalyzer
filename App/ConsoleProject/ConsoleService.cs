using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ConsoleService
{
    class ConsoleService
    {
            private Dictionary<string, Action<IEnumerable<string>>> _allCommandDictionary =
                new Dictionary<string, Action<IEnumerable<string>>>
                {
                    { "help", ExecuteCommand(() => { Console.WriteLine(File.ReadAllText("\\help.txt")); })},

                    { "clear", ExecuteCommand(Console.Clear) },

                    { "exit", ExecuteCommand(() => Environment.Exit(0))},

                    { "add", ExecuteCommand(Console.Clear)},
                    { "stat" , ExecuteCommand(Console.Clear)  },

                    { "post", ExecuteCommand(Console.Clear)  },
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

        }


}
