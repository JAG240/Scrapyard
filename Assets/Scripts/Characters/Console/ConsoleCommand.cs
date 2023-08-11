using System.Reflection;
using UnityEngine;

namespace Scrapyard.services.commands
{
    public abstract class ConsoleCommand
    {
        public string keyWord { get; protected set; }
        public ConsoleCommand(string keyWord)
        {
            this.keyWord = keyWord;
        }
        public virtual void Execute(string command)
        {
            string[] args = command.Split('.');

            int nextSpecialIndex = args[1].IndexOfAny(new char[] { '(', '.' });
            string nextCMD = nextSpecialIndex > 0 ? args[1].Substring(0, nextSpecialIndex) : args[1];

            MethodInfo method = GetType().GetMethod(nextCMD.Trim(' ', '\n'), BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);

            string[] methodStrings = new string[args.Length - 1];

            for (int i = 1; i < args.Length; i++)
                methodStrings[i - 1] = args[i];

            if (method != null)
                method.Invoke(this, new object[] { methodStrings });
            else
                ServiceLocator.Resolve<Console>().Log(LogType.ERROR, $"Command {args[1].TrimEnd('\n')} not found");
        }
    }
}
