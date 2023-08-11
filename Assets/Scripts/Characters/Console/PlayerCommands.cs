using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.services;
using Scrapyard.items;
using Scrapyard.items.weapons;
using Scrapyard.core.character;
using System.Reflection;
using System.Globalization;

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

            base.Execute(command);

        }

        //player.giveweapon.base(name).parts(part name, part name, ...)
        //EX: player.giveweapon.base(default gun).parts(default grip, default barrel)
        private void GiveWeapon(string[] args)
        {
            WeaponBase weaponBase = null;
            List<WeaponPart> weaponParts = new List<WeaponPart>();

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            foreach(string arg in args)
            {

                if (arg.Contains("base", System.StringComparison.OrdinalIgnoreCase))
                {
                    int nameStart = arg.IndexOf('(') + 1;
                    string name = arg.Substring(nameStart, arg.Length - nameStart - 1);
                    name = name.Trim();
                    name = name.ToLower();
                    name = textInfo.ToTitleCase(name);

                    weaponBase = ServiceLocator.Resolve<ItemIndex>().Get<WeaponBase>(name);
                }
                else if(arg.Contains("parts", System.StringComparison.OrdinalIgnoreCase))
                {
                    int namesStart = arg.IndexOf('(') + 1;
                    string names = arg.Substring(namesStart, arg.Length - namesStart - 2);
                    string[] partNames = names.Split(',');

                    foreach(string name in partNames)
                    {
                        name.ToLower();
                        string prepName = textInfo.ToTitleCase(name);
                        prepName = prepName.Trim();

                        WeaponPart part = ServiceLocator.Resolve<ItemIndex>().Get<WeaponPart>(prepName);

                        if (part != null)
                            weaponParts.Add(part);
                    }
                }
            }

            Weapon weapon = ServiceLocator.Resolve<WeaponBuilder>().BuildWeapon(weaponBase, weaponParts.ToArray());

            if (weapon == null)
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Weapon not built, incorrect combination");
            else if(_player.inventory.equipWeapon(0, weapon))
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.LOG, "Weapon added to primary slot");

        }

        private void ClearInv(string[] args)
        {
            _player.inventory.Clear();
            ServiceLocator.Resolve<services.Console>().Log(services.LogType.LOG, "Player Inventory cleared");
        }
    }
}
