using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.services;
using Scrapyard.items;
using Scrapyard.items.weapons;
using Scrapyard.core.character;
using System.Reflection;

namespace Scrapyard.services.commands
{
    public class PlayerCommands : ConsoleCommand
    {
        private Character _player;

        public PlayerCommands() : base("player")
        {

        }

        public override void Execute(string command)
        {
            _player = GameObject.Find("Player").GetComponent<Character>();

            string[] args = command.Split('.');

            int nextSpecialIndex = args[1].IndexOfAny(new char[] { '(', '.' });
            string nextCMD = nextSpecialIndex > 0 ? args[1].Substring(0, nextSpecialIndex) : args[1];

            MethodInfo method = GetType().GetMethod(nextCMD, BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);

            string[] methodStrings = new string[args.Length - 1];

            for(int i = 1; i < args.Length; i++)
                methodStrings[i- 1] = args[i];

            if (method != null)
                method.Invoke(this, new object[] { methodStrings });
            else
                ServiceLocator.Resolve<Console>().Log(LogType.ERROR, $"Player Command {args[1]} not found");

        }

        //player.giveweapon.base(name).parts(names)
        //EX: player.giveweapon.base(Default Gun).parts(Default Grip,Default Barrel)
        //ES: player.giveweapon.base(Default Melee).parts(Default Blade)
        private void GiveWeapon(string[] args)
        {
            WeaponBase weaponBase = null;
            List<WeaponPart> weaponParts = new List<WeaponPart>();

            foreach(string arg in args)
            {

                if (arg.Contains("base", System.StringComparison.OrdinalIgnoreCase))
                {
                    int nameStart = arg.IndexOf('(') + 1;
                    string name = arg.Substring(nameStart, arg.Length - nameStart - 1);
                    weaponBase = ServiceLocator.Resolve<ItemIndex>().Get<WeaponBase>(name);
                }
                else if(arg.Contains("parts", System.StringComparison.OrdinalIgnoreCase))
                {
                    int namesStart = arg.IndexOf('(') + 1;
                    string names = arg.Substring(namesStart, arg.Length - namesStart -1);
                    string[] partNames = names.Split(',');

                    foreach(string name in partNames)
                    {
                        WeaponPart part = ServiceLocator.Resolve<ItemIndex>().Get<WeaponPart>(name);

                        if (part != null)
                            weaponParts.Add(part);
                    }
                }
            }

            WeaponPart[] parts = new WeaponPart[weaponParts.Count];

            for(int i = 0; i < parts.Length; i++)
            {
                parts[i] = weaponParts[i];
            }

            Weapon weapon = ServiceLocator.Resolve<WeaponBuilder>().BuildWeapon(weaponBase, parts);

            if (weapon == null)
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Weapon not built, incorrect combination");
            else
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.LOG, "Weapon added to primary slot");

            _player.inventory.equipWeapon(0, weapon);
        }
    }
}
