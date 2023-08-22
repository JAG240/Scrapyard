using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Scrapyard.items.weapons;

namespace Scrapyard.services
{
    public class ReloadCanvas : Service
    {
        private GameObject player;
        private Vector3 offset = new Vector3(0f, 1.7f, 0f);
        private bool isVisible = false;

        public Weapon weapon;

        [SerializeField] private Image reloadBar;
        [SerializeField] private Image reloadFill;
        [SerializeField] private RectTransform fillAmount;
        [SerializeField] private float fadeSpeed = 1f;

        protected override void Register()
        {
            player = GameObject.Find("Player");

            ServiceLocator.Register<ReloadCanvas>(this);
        }

        private void Update()
        {
            transform.position = player.transform.position + offset;
            transform.LookAt(Camera.main.transform.position);
            CheckForUpdate();
        }

        private void CheckForUpdate()
        {
            if (weapon == null && isVisible)
                StartCoroutine(MakeVisible(false));

            if (weapon == null)
                return;

            if (!weapon.isReloading && isVisible)
                StartCoroutine(MakeVisible(false));

            if (!weapon.isReloading)
                return;

            float progress = weapon.reloadTimer / weapon.reloadSpeed;
            DisplayReload(progress);
        }

        public void DisplayReload(float progress)
        {
            if (!isVisible)
                StartCoroutine(MakeVisible(true));

            Vector3 scale = fillAmount.localScale;
            scale.x = progress;
            fillAmount.localScale = scale;
        }

        private IEnumerator MakeVisible(bool state)
        {
            isVisible = state;

            Color fade = Color.white;
            float time = 0f;

            while(time < fadeSpeed)
            {
                time += Time.deltaTime;
                float t = time / fadeSpeed;

                fade.a = state ? Mathf.Lerp(0f, 1f, t) : Mathf.Lerp(1f, 0f, t);
                reloadBar.color = fade;
                reloadFill.color = fade;

                yield return null;
            }
        }
    }
}
