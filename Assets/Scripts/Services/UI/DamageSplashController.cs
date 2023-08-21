using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.services
{
    public class DamageSplashController : MonoBehaviour
    {
        [SerializeField] private GameObject splashPrefab;

        public void NewSplash(Vector3 pos, float damage)
        {
            GameObject newSplash = Instantiate(splashPrefab, pos, Quaternion.identity);
            newSplash.GetComponent<SplashBehavior>().Init(damage);
        }
    }
}
