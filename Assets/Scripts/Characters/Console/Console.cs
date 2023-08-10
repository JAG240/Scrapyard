using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scrapyard.core.character;

namespace Scrapyard.services
{
    public class Console : Service
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private TMP_InputField _commandConsole;
        [SerializeField] private RectTransform _scroll;

        protected override void Awake()
        {
            base.Awake();
        }

        public void OnConsole()
        {
            GameObject root = transform.GetChild(0).gameObject;
            root.SetActive(!root.activeInHierarchy);
        }

        protected override void Register()
        {
            ServiceLocator.Register<Console>(this);
        }

        public void Log(LogType type, string message)
        {
            GameObject textLog = new GameObject();
            textLog.transform.parent = _content.transform;
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
            string command = _commandConsole.text;
            Log(LogType.LOG, command);
            _commandConsole.text = string.Empty;
            _commandConsole.ActivateInputField();
            _content.localPosition = new Vector3(_content.localPosition.x, _content.sizeDelta.y - 100f, _content.localPosition.z);

            //TODO: Write command handler
            if (command == "GiveWeapon")
            {
                GameObject.Find("Player").GetComponent<Character>().inventory.GiveWeapon();
            }
        }
    }
}
