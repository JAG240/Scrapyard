using UnityEngine;

namespace Scrapyard.services
{
    public class ServicesInitializer : MonoBehaviour
    {
        void Start()
        {
            Init();
        }

        private void Init()
        {
            //Initialize any services that are needed in the order needed here

            Destroy(gameObject);
        }
    }
}
