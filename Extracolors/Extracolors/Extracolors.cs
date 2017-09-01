using MSCLoader;
using UnityEngine;

namespace Extracolors
{
    public class Extracolors : Mod
    {
        private bool loaded;
        public override string ID => "Extracolors";
        public override string Name => "Extracolors";
        public override string Author => "haverdaden";
        public override string Version => "1.0";

        //Called when mod is loading
        public override void OnLoad()
        {
        }

        // Update is called once per frame
        public override void Update()
        {
            if (!loaded && Application.loadedLevelName == "GAME")
            {
                if (GameObject.Find("PLAYER"))
                {
                    foreach (var transform in Resources.FindObjectsOfTypeAll<Transform>())
                        if (transform.name == "spray can(itemx)")
                        {
                            ModConsole.Print(transform.name + transform.gameObject.activeInHierarchy);
                            transform.gameObject.SetActive(true);
                        }

                    loaded = true;
                }
            }
            else if (Application.loadedLevelName != "GAME")
            {
                loaded = false;
            }
        }
    }
}
