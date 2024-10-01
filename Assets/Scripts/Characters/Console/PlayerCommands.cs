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

            foreach(string arg in args)
            {

                if (arg.Contains("base", System.StringComparison.OrdinalIgnoreCase))
                {
                    int nameStart = arg.IndexOf('(') + 1;
                    string name = arg.Substring(nameStart, arg.Length - nameStart - 1);
                    name = name.Trim();
                    name = name.ToLower();

                    weaponBase = ServiceLocator.Resolve<ItemIndex>().Get<WeaponBase>(name);
                }
                else if(arg.Contains("parts", System.StringComparison.OrdinalIgnoreCase))
                {
                    int namesStart = arg.IndexOf('(') + 1;
                    string names = arg.Substring(namesStart, arg.Length - namesStart - 2);
                    string[] partNames = names.Split(',');

                    foreach(string name in partNames)
                    {
                        string prepName = name.ToLower();
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

        private void GiveDefault(string[] args)
        {
            if(args.Length < 2)
            {
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Must pass default weapon type: EX: player.givedefault.gun");
                return;
            }

            string type = args[1];
            type = type.ToLower();

            if(type.Contains("gun"))
            {
                ItemIndex index = ServiceLocator.Resolve<ItemIndex>();
                Weapon weapon = ServiceLocator.Resolve<WeaponBuilder>().BuildWeapon(index.Get<WeaponBase>("default gun") , new WeaponPart[] { index.Get<WeaponPart>("default grip"), index.Get<WeaponPart>("default barrel") });
                _player.inventory.equipWeapon(0, weapon);
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.LOG, "Weapon added to primary slot");
            }
            else if (type.Contains("melee"))
            {
                ItemIndex index = ServiceLocator.Resolve<ItemIndex>();
                Weapon weapon = ServiceLocator.Resolve<WeaponBuilder>().BuildWeapon(index.Get<WeaponBase>("default melee"), new WeaponPart[] { index.Get<WeaponPart>("default blade") });
                _player.inventory.equipWeapon(0, weapon);
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.LOG, "Weapon added to primary slot");
            }
            else
            {
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Default weapon type not found");
            }

        }

        private void ClearInv(string[] args)
        {
            _player.inventory.Clear();
            ServiceLocator.Resolve<services.Console>().Log(services.LogType.LOG, "Player Inventory cleared");
        }

        private void GiveItem(string[] args)
        {
            if (args.Length < 2)
            {
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Must pass item name: EX: player.giveitem.rusty pipe");
                return;
            }

            string ItemName = args[1];
            ItemName = ItemName.Trim('\n', ' ');
            ItemName.ToLower();

            Item item = ServiceLocator.Resolve<ItemIndex>().Get<Item>(ItemName);
            if (item == null || !_player.inventory.AddToInventory(item))
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, "Cannot Add Item To Inventory");
            else
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.LOG, $"Added {item.name} To Player Inventory");

        }

        private void ShowInv(string[] args)
        {
            int i = 0;

            foreach(object o in _player.inventory.inventory)
            {
                string name = o != null ? o.ToString() : "Empty";
                ServiceLocator.Resolve<services.Console>().Log(services.LogType.LOG, $"{i} : {name}");
                i++;
            }
        }
    }
}
