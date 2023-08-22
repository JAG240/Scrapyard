using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.services
{
    public class CharacterIndex : Service
    {
        [SerializeField] private List<GameObject> characterList = new List<GameObject>();
        private Dictionary<string, GameObject> characters = new Dictionary<string, GameObject>();
        protected override void Register()
        {
            foreach(GameObject c in characterList)
            {
                string name = c.name;
                name = name.ToLower();
                characters.TryAdd(name, c);
            }

            ServiceLocator.Register<CharacterIndex>(this);
        }

        public GameObject Get(string name)
        {
            GameObject character;
            string search = name.ToLower();
            characters.TryGetValue(search, out character);
            return character;
        }

        public bool SpawnCharacter(Vector3 pos, string name)
        {
            GameObject character = Get(name);

            if (character == null)
                return false;

            Instantiate(character, pos, Quaternion.identity);

            return true;
        }
    }
}
