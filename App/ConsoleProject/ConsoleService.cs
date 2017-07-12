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
            private Dictionary<string, Action<IEnumerable<string>>> _allCommand =
                new Dictionary<string, Action<IEnumerable<string>>>
                {
                    { "help", ExecuteCommand(() => { Console.WriteLine(File.ReadAllText("\\help.txt")); })},
                    { "clear", ExecuteCommand(Console.Clear) },
                    { "exit", ExecuteCommand(() => Environment.Exit(0))},

                    { "add", ExecuteCommand(() => Console.Clear())  },
                    { "stat" , ExecuteCommand(() => Console.Clear())  },

                    { "post", ExecuteCommand(() => Console.Clear())  },
                };

            public static string ReadPassword()
            {
                string password = "";
                ConsoleKeyInfo info = Console.ReadKey(true);

                while (info.Key != ConsoleKey.Enter)
                {
                    if (info.Key != ConsoleKey.Backspace)
                    {
                        Console.Write("*");
                        password += info.KeyChar;
                    }
                    else if (info.Key == ConsoleKey.Backspace)
                    {
                        if (!string.IsNullOrEmpty(password))
                        {
                            // remove one character from the list of password characters
                            password = password.Substring(0, password.Length - 1);
                            // get the location of the cursor
                            int pos = Console.CursorLeft;
                            // move the cursor to the left by one character
                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                            // replace it with space
                            Console.Write(" ");
                            // move the cursor to the left by one character again
                            Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        }
                    }
                    info = Console.ReadKey(true);
                }
                // add a new line because user pressed enter at the end of their password
                Console.WriteLine();
                return password;
            }

            public static List<string> ReadLogins()
            {


                return null;
            }

            private static Action<IEnumerable<string>> ExecuteCommand(Action a)
            {
                return args =>
                {
                    if (args.Any())
                        throw new ArgumentException("this command doesn't support args");
                    a();
                };
            }

            IEnumerable<string> SplitIntoTokens(string s)
            {
                // тут надо бы что-то похитрее, как минимум учитывать группировку
                // кавычками и escape-символы
                return s.Split(null as char[], StringSplitOptions.RemoveEmptyEntries);
            }
        }


}
