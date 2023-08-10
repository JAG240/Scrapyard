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
            //Console MUST be first registered
            Instantiate(Resources.Load("Services/Console"));

            Instantiate(Resources.Load("Services/WeaponBuilder"));
            Instantiate(Resources.Load("Services/ItemIndex"));
            Instantiate(Resources.Load("Services/BulletInfo"));

            Destroy(gameObject);
        }
    }
}
