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
        public abstract void Execute(string command);
    }
}
