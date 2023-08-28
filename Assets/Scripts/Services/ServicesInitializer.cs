using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.services
{
    public class ServicesInitializer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> services = new List<GameObject>();

        void Start()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            foreach(GameObject service in services)
            {
                GameObject newObj = Instantiate(service);
                Service s = newObj.GetComponent<Service>();
                yield return new WaitUntil(() => s.Registered);
            }

            Destroy(gameObject);
        }
    }
}
