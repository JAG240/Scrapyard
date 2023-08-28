using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scrapyard.core.character;
using Scrapyard.services.commands;
using UnityEngine.InputSystem;

namespace Scrapyard.services
{
    public class Console : Service
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private TMP_InputField _commandConsole;
        [SerializeField] private RectTransform _scroll;

        private List<ConsoleCommand> commands = new List<ConsoleCommand>();
        private string[] _commandMemory = new string[10];
        private int _memoryIndex = 0;

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

            int childHeight = _content.childCount * 50;

            if (childHeight > _scroll.rect.height)
                _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, childHeight - 500);

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

        public void OnNavConsoleMemory(InputValue value)
        {
            int newIndex = (int)value.Get<float>();

            if (_memoryIndex + newIndex > 9)
                _memoryIndex = 0;
            else if (_memoryIndex + newIndex < 0)
                _memoryIndex = 9;

            _memoryIndex += (int)value.Get<float>();

            _commandConsole.text = _commandMemory[_memoryIndex];
        }

        public void EnterCommand()
        {
            string command = _commandConsole.text;

            if (command.EndsWith("\n"))
                command.Remove(command.Length - 1);
            else
                return;

            if (command == string.Empty)
                return;

            _commandMemory[_memoryIndex] = command.TrimEnd('\n');

            if (_memoryIndex == 9)
                _memoryIndex = 0;
            else
                _memoryIndex++;

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
                    return;
                }
            }

            Log(LogType.ERROR, $"Command {args[0].Trim('\n')} not found");
        }
    }
}
