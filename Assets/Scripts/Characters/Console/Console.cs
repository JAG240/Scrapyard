using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scrapyard.core.character;

namespace Scrapyard.services
{
    public class Console : Service
    {
        [SerializeField] private RectTransform logPanel;
        [SerializeField] private TMP_InputField commandConsole;
        [SerializeField] private RectTransform scroll;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {

        }

        protected override void Register()
        {
            ServiceLocator.Register<Console>(this);
        }

        public void Log(LogType type, string message)
        {
            GameObject textLog = new GameObject();
            textLog.transform.parent = logPanel.transform;
            textLog.transform.localScale = Vector3.one;

            TextMeshProUGUI text = textLog.AddComponent<TextMeshProUGUI>();
            text.text = message;
            text.color = GetTextColor(type);
            text.rectTransform.sizeDelta = new Vector2(1600f, 50f);

        }

        private Color GetTextColor(LogType type)
        {
            switch (type)
            {
                case LogType.LOG:
                    return Color.white;
                case LogType.ERROR:
                    return Color.yellow;
                case LogType.FATAL:
                    return Color.red;
                default:
                    return Color.white;
            }
        }

        public void EnterCommand()
        {
            string command = commandConsole.text;
            Log(LogType.LOG, command);
            commandConsole.text = string.Empty;
            commandConsole.ActivateInputField();
            logPanel.localPosition = new Vector3(logPanel.localPosition.x, logPanel.sizeDelta.y - 100f, logPanel.localPosition.z);

            //TODO: Write command handler
            if (command == "GiveWeapon")
            {
                GameObject.Find("Player").GetComponent<Character>().inventory.GiveWeapon();
            }
            else if (command == "ShowWeapon")
                Log(LogType.LOG, GameObject.Find("Player").GetComponent<Character>().inventory.primaryWeapon.bluntDamage.ToString());
        }
    }
}
