using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.services
{
    public class ServicesInitializer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> services = new List<GameObject>();

        void Start()
        {
            Init();
        }

        private void Init()
        {
            foreach(GameObject service in services)
            {
                Instantiate(service);
            }

            Destroy(gameObject);
        }
    }
}
