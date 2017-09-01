using System.IO;

namespace GodMode
{
    using HutongGames.PlayMaker;

    using MSCLoader;

    using UnityEngine;
       

    public class GodMode : Mod
    {
        // The ID of the mod - Should be unique
        public override string ID => "GodMode";

        // The name of the mod that is displayed
        public override string Name => "MSC GodMode";

        // The name of the author
        public override string Author => "haverdaden";

        // The version of the mod
        public override string Version => "1.2.0";

        // Keybinds
        readonly Keybind _godKey =
            new Keybind("GodKey", "GodMode Settings Key", KeyCode.G, KeyCode.RightControl);

        bool _guiShow;

        readonly Rect _guiBox = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 120, 400, 230);

        bool _partsLocked;
        bool _removedDeform;
        bool _deathDisabled;
        bool _deathDisabled2;
        bool _deathDisabled3;
        bool _deathDisabled4;
        FsmState _playerstate;
        bool _playerstateSaved;
        bool _playerParentedtoCar;
        bool _deformableDestroyed;
        GameObject _satsuma;
        GameObject _player;
        bool _foundGameobjects;



        public override void OnLoad()
        {
            Keybind.Add(this, this._godKey);

        }

        public override void OnGUI()
        {

            if (this._guiShow)
            {
                GUI.backgroundColor = Color.black;
                GUI.skin.window.fontSize = 12;
                GUI.ModalWindow(200, this._guiBox, this.GodModeWindow, "GodMode Settings | GodMode by haverdaden (DD)");
            }
        }

        void GodModeWindow(int id)
        {

            // STYLING
            GUIStyle godLabelStyle = new GUIStyle();
            godLabelStyle.alignment = TextAnchor.UpperCenter;
            godLabelStyle.fontSize = 14;
            godLabelStyle.fontStyle = FontStyle.Bold;
            godLabelStyle.normal.textColor = Color.white;

            //BUTTON TO LOCK SATSUMA PARTS
            GUI.Label(
                new Rect(50, 50, 300, 30),
                "This will make it harder for parts to fall off. \n " + "WARNING! Save before you do this!",
                godLabelStyle);
            //BUTTON TO UNLOCK SATSUMA PARTS



            if (GUI.Button(new Rect(100, 90, 200, 30), "Lock Satsuma Parts"))
            {
                foreach (var VARIABLE in Resources.FindObjectsOfTypeAll<FixedJoint>())
                {
                    if (VARIABLE.transform.root.name == "SATSUMA(557kg)")
                    {
                        Object.Destroy(VARIABLE.gameObject.GetComponent<Rigidbody>());
                        ModConsole.Print(VARIABLE.transform.name);


                    }
                }
/*                bool done = false;
                StreamWriter writer = new StreamWriter("PlaymakerExperimentalFSM.txt");
                foreach (var VARIABLE in Resources.FindObjectsOfTypeAll<PlayMakerFSM>())
                {
                  writer.WriteLine(VARIABLE.Fsm.GameObject.transform.root.name + " | " + VARIABLE.FsmName + " | " + VARIABLE.Active + " | Enabled: " + VARIABLE.enabled + " | " + VARIABLE.Fsm.GameObject.name + " | " + VARIABLE.Fsm.GameObject.activeInHierarchy);
                    if (VARIABLE.Fsm.GameObject.name == "fender right(Clone)")
                    {
                        foreach (var varia in VARIABLE.FsmVariables.BoolVariables)
                        {
                            ModConsole.Print(varia.Name);
                            varia.UseVariable = false;
                        }
                    }
//                    if (VARIABLE.FsmName == "CarDebug")
//                    {
//                        VARIABLE.enabled = true;
//                    }
                }*/
                //writer.Close();
            }
            if (GUI.Button(new Rect(100, 120, 200, 30), "Unlock Satsuma Parts"))
            {
                foreach (var VARIABLE in Resources.FindObjectsOfTypeAll<PlayMakerFSM>())
                {
                    ModConsole.Print(VARIABLE.Fsm.EventTarget.target);
                }
            }

            //SHOW LABEL THAT PARTS ARE LOCKED
            if (this._partsLocked) GUI.Label(new Rect(50, 140, 300, 30), "PARTS LOCKED", godLabelStyle);

            //Close Window
            if (GUI.Button(new Rect(125, 195, 150, 30), "Close")) this._guiShow = false;
        }

        public override void Update()
        {
            GetGameobjects();
            this.RunGodMode();
            this.CheckIfInSatsuma();
            ResetToDefault();
            GuiToggleKeyDown();

        }

        void GuiToggleKeyDown()
        {
            if (this._godKey.IsDown()) this.GuiToggle();
        }

        void GetGameobjects()
        {
            if (!_foundGameobjects)
            {
                if (GameObject.Find("SATSUMA(557kg)") && GameObject.Find("PLAYER") && Application.loadedLevel == 3)
                {
                    _satsuma = GameObject.Find("SATSUMA(557kg)");
                    _player = GameObject.Find("PLAYER");
                    _foundGameobjects = true;
                }

            }
        }

        void ResetToDefault()
        {
            if (Application.loadedLevel != 3)
            {
                _playerstateSaved = false;
                _playerstate = null;
                _deathDisabled = false;
                _deathDisabled2 = false;
                _deathDisabled3 = false;
                _deathDisabled4 = false;
                _partsLocked = false;
                _removedDeform = false;
                _playerParentedtoCar = false;
                _deformableDestroyed = false;
                _foundGameobjects = false;



            }
        }

        void CheckIfInSatsuma()
        {
            if (!_playerstateSaved && _satsuma && _player)
            {
                foreach (var fsm in Fsm.FsmList)
                    foreach (var states in fsm.States)
                        if (fsm.Name == "PlayerTrigger" && fsm.GameObject.transform.root.name == "SATSUMA(557kg)"
                            && states.Name == "Player in car")
                        {
                            _playerstate = states;

                        }

                _playerstateSaved = true;
            }

            if (!_playerParentedtoCar && _playerstate != null && _playerstate.Active && _satsuma && _player)
            {

                _player.transform.parent = _satsuma.transform;


                _playerParentedtoCar = true;
            }
            else if (_playerParentedtoCar && _playerstate != null && !_playerstate.Active && _satsuma && _player)
            {

                _player.transform.parent = null;


                _playerParentedtoCar = false;
            }





        }

        void RunGodMode()
        {

            if (!_deathDisabled || !_deathDisabled2 || !_deathDisabled3 || !_deathDisabled4)
            {
                if (Application.loadedLevel == 3)
                {
                    foreach (var fsm in Fsm.FsmList)
                        foreach (var states in fsm.States)
                            foreach (var action in states.Actions)
                            {

                                if (fsm.Name == "Death" && action.Enabled)
                                {
                                    action.Enabled = false;
                                    _deathDisabled = true;
                                }
                                if (states.Name == "Death" && action.Enabled)
                                {
                                    action.Enabled = false;
                                    _deathDisabled2 = true;
                                }
                                if (states.Name == "Die 2" && action.Enabled)
                                {
                                    action.Enabled = false;
                                    _deathDisabled3 = true;
                                }
                                if (states.Name == "Die" && action.Enabled)
                                {
                                    action.Enabled = false;
                                    _deathDisabled4 = true;
                                }
                            }
                    ModConsole.Print("<color=lime><b>Godmode:</b></color><color=orange><b> Found and removed death! Have fun!</b></color>");
                }

            }


            if (_satsuma && !_deformableDestroyed)
            {
                foreach (var deform in _satsuma.GetComponentsInChildren<Deformable>())
                    UnityEngine.Object.DestroyImmediate(deform);

                _deformableDestroyed = true;
            }
        }

        void GuiToggle()
        {
            if (this._guiShow) this._guiShow = false;
            else this._guiShow = true;
        }
    }
}
