using System;
using MSCLoader;
using UnityEngine;

namespace SlowMotion
{
    public class SlowMotion : Mod
    {
        public override string ID => "SlowMotion";
        public override string Name => "SlowMotion";
        public override string Author => "haverdaden";
        public override string Version => "1.0";


        // Keybinds
        private readonly Keybind _slowMotionKey2 =
            new Keybind("SlowMoKey2", "Slow Motion Key2", KeyCode.Alpha4, KeyCode.None);

        private readonly Keybind _slowMotionKey =
            new Keybind("SlowMoKey", "Slow Motion Key", KeyCode.KeypadEnter, KeyCode.None);

        private readonly Keybind _freezeKey =
            new Keybind("FreezeKey", "Freeze Time Key", KeyCode.KeypadMinus, KeyCode.None);

        private readonly Keybind _slowMotionGui =
            new Keybind("SlowMoGUIKey", "Slow Motion GUI Key", KeyCode.T, KeyCode.RightControl);


        private const float _modalHeight = 240;
        private const float _modalWidth = 300;
        private const float _freezeTimescale = 0.000001f;

        private float _timescale = 0.3f;
        private float _changedTimeScale = 0.3f;

        private bool _slowMotionEnabled;
        private bool _slowMotionGUI;
        private bool _timeFrozen;
        private bool _modLoaded;

        private string _slowMotionEnabledString = "<color=red><b>Disabled</b></color>";
        private string FreezeStatus = "Freeze Time";
        private string _ToggleString = "Enable";

        //Called when mod is loading
        public override void OnLoad()
        {
            Keybind.Add(this, _slowMotionKey);
            Keybind.Add(this, _slowMotionKey2);
            Keybind.Add(this, _freezeKey);
            Keybind.Add(this, _slowMotionGui);
        }

        public override void OnGUI()
        {
            if (_slowMotionGUI)
                GUI.ModalWindow(903,
                    new Rect(Screen.width / 2 - _modalWidth / 2, Screen.height / 2 - _modalHeight / 2, _modalWidth,
                        _modalHeight),
                    ModalWindow, "<color=green><b>SlowMotionMod by haverdaden(DD)</b></color>");
        }

        private void ModalWindow(int id)
        {
            var labelStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                normal = {textColor = Color.white}
            };

            GUI.Label(new Rect(20, 30, 260, 20), "Slow motion amount: <color=yellow><b>" + _timescale + "</b></color>",
                labelStyle);
            _timescale = (float) Math.Round(GUI.HorizontalSlider(new Rect(20, 60, 260, 30), _timescale, 0.1f, 2f), 1);
            GUI.Label(new Rect(20, 80, 260, 20), "Slow Motion is: " + _slowMotionEnabledString, labelStyle);

            if (GUI.Button(new Rect(150, 105, 100, 30), FreezeStatus))
                FreezeTime();
            if (GUI.Button(new Rect(50, 105, 100, 30), _ToggleString))
            {
                _slowMotionEnabled = !_slowMotionEnabled;
                SetTimescale();
            }

            GUI.Label(new Rect(20, 140, 260, 20), "<color=lime><b>Notice</b></color>: You can also use the hotkeys.",
                labelStyle);
            GUI.Label(new Rect(20, 160, 260, 40),
                "<color=red><b>WARNING</b></color>: Changing timescale can cause\nunexpected behaviors.", labelStyle);
            if (GUI.Button(new Rect(50, 200, 100, 30), "Default"))
            {
                _changedTimeScale = 1;
                _timescale = 1;
                ResetDefaultTime();
            }
            if (GUI.Button(new Rect(150, 200, 100, 30), "Close"))
            {
                PlayMakerGlobals.Instance.Variables.GetFsmBool("PlayerInMenu").Value = false;
                _slowMotionGUI = false;
            }
        }

        // Update is called once per frame
        public override void Update()
        {
            if (Application.loadedLevelName == "GAME" && !_modLoaded)
                _modLoaded = true;

            if (_modLoaded)
            {
                CheckKeyDown();
                CheckSliderChange();
            }
            if (Application.loadedLevelName != "GAME" && _modLoaded)
            {
                ResetDefaultTime();
                _slowMotionEnabled = false;
                _slowMotionGUI = false;
                _timeFrozen = false;
                _modLoaded = false;
            }
        }

        private void CheckKeyDown()
        {
            if (_slowMotionGui.IsDown())
            {
                _slowMotionGUI = !_slowMotionGUI;
                var showMouse = PlayMakerGlobals.Instance.Variables.GetFsmBool("PlayerInMenu");
                showMouse.Value = !showMouse.Value;
            }
            if (_slowMotionKey.IsDown() || _slowMotionKey2.IsDown())
            {
                _slowMotionEnabled = !_slowMotionEnabled;
                SetTimescale();
            }
            if (_freezeKey.IsDown())
                FreezeTime();
        }

        private void CheckSliderChange()
        {
            if (_slowMotionEnabled)
                if (_changedTimeScale != _timescale)
                {

                    SetTimescale();
                }
        }

        private void SetTimescale()
        {
            _changedTimeScale = _timescale;

            ResetDefaultTime();

            if (_slowMotionEnabled)
            {
                Time.timeScale = _timescale;
                Time.fixedDeltaTime = _timescale * 0.02f;
                ModConsole.Print(Time.maximumDeltaTime);
                _ToggleString = "Disable";
            }
        }

        private void FreezeTime()
        {
            if (!_timeFrozen)
            {
                Time.fixedDeltaTime = 0.002f;
                Time.timeScale = _freezeTimescale;
                _slowMotionEnabledString = "<color=aqua><b>Frozen</b></color>";
                FreezeStatus = "Unfreeze Time";
                _timeFrozen = true;
            }
            else
            {
                ResetDefaultTime();
                FreezeStatus = "Freeze Time";
                _timeFrozen = false;
            }
        }

        private void ResetDefaultTime()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
            _slowMotionEnabledString = "<color=red><b>Disabled</b></color>";
            _ToggleString = "Enable";

            if (_slowMotionEnabled)
            {
                _slowMotionEnabledString = "<color=lime><b>Enabled</b></color>";
                _ToggleString = "Disable";
            }
        }
    }
}
