using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scrapyard.core.character;
using System.Text.RegularExpressions;
using Scrapyard.services.commands;

namespace Scrapyard.services
{
    public class Console : Service
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private TMP_InputField _commandConsole;
        [SerializeField] private RectTransform _scroll;

        private List<ConsoleCommand> commands = new List<ConsoleCommand>();

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
            commands = CustomFunctions.CreateInstances<ConsoleCommand>();
            ServiceLocator.serviceRegistered += LogReserviceRegister;
            ServiceLocator.Register<Console>(this);
        }

        private void LogReserviceRegister(string serviceName)
        {
            Log(LogType.LOG, $"Service registered: {serviceName}");
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

            if (_content.rect.height + 100 > _scroll.rect.height)
                _content.localPosition = new Vector3(_content.localPosition.x, _content.localPosition.y + 50, _content.localPosition.z);

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

            if (command == string.Empty)
                return;

            Log(LogType.LOG, command);
            _commandConsole.text = string.Empty;
            _commandConsole.ActivateInputField();

            ExecuteCommand(command);
        }

        private void ExecuteCommand(string command)
        {
            string[] args = command.Split('.');

            foreach(ConsoleCommand com in commands)
            {
                if(string.Equals(com.keyWord, args[0], System.StringComparison.OrdinalIgnoreCase))
                {
                    com.Execute(command);
                }
            }
        }
    }
}
