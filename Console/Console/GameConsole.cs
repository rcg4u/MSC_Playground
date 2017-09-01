using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace Console
{
    public class Console : Mod
    {
        public override string ID { get { return "GameConsole"; } }
        public override string Name { get { return "GameConsole"; } }
        public override string Author { get { return "haverdaden"; } }
        public override string Version { get { return "1.0"; } }

        private bool loaded;

        private readonly Keybind _consoleKey =
            new Keybind("GameConsoleKey", "GameConsoleKey", KeyCode.C, KeyCode.LeftAlt);

        private Transform console;
        private FsmBool _playerInMenu;

        //Called when mod is loading
        public override void OnLoad()
        {
            Keybind.Add(this, _consoleKey);
        }

        // Update is called once per frame
        public override void Update()
        {
            if (!loaded && Application.loadedLevelName == "GAME")
            {
                foreach (var transform in Resources.FindObjectsOfTypeAll<Transform>())
                {
                    if (transform.name == "Console")
                    {
                        console = transform;
                        console.gameObject.SetActive(false);
                    }
                }
                _playerInMenu = PlayMakerGlobals.Instance.Variables.GetFsmBool("PlayerInMenu");
                loaded = true;
            }
            else if (Application.loadedLevelName != "GAME")
            {
                loaded = false;
            }

            if (loaded)
            {
                if (_consoleKey.IsDown())
                {
                    console.gameObject.SetActive(!console.gameObject.activeSelf);
                    foreach (var VARIABLE in console.GetComponentsInChildren<PlayMakerFSM>())
                    {
                        VARIABLE.SendEvent("SHIT");
                    }
                }

                if (_playerInMenu.Value == false)
                {
                    console.gameObject.SetActive(false);
                }
            }
        }
    }
}
