using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.services.commands
{
    public class SpawnCommands : ConsoleCommand
    {
        public SpawnCommands() : base("spawn")
        {

        }

        public override void Execute(string command)
        {
            base.Execute(command);
        }

        private void Char(string[] args)
        {
            if(args.Length < 2)
            {
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, $"Must define character name: EX: spawn.char.dummy");
                return;
            }

            Vector3 playerPos = GameObject.Find("Player").transform.position;
            playerPos.z += 2;

            string name = args[1];
            name = name.Trim('\n');

            if(ServiceLocator.Resolve<CharacterIndex>().SpawnCharacter(playerPos, name))
            {
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.LOG, $"Spawned {name}");
            }
            else
            {
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, $"Cannot find character with name {name}");
            }
        }
    }
}
