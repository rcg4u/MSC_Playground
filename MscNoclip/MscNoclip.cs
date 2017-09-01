using MSCLoader;
using UnityEngine;

namespace MscNoclip

{
    using System;
    using System.Timers;

    using HutongGames.PlayMaker;

    public class MscNoclip : Mod

    {

        // Keybinds
        private readonly Keybind _flyKey = new Keybind("ToggleFlyKey", "Toggle Noclip ON/OFF", KeyCode.N,
            KeyCode.LeftControl);

        private readonly Keybind _noclipGuiKey = new Keybind("showNoclipGui", "Toggle Noclip GUI", KeyCode.B,
            KeyCode.LeftControl);

        //GUI
        private readonly Rect _guiBox = new Rect(Screen.width / 2 - 190, Screen.height / 2 - 200, 380, 400);
        private readonly Rect _guiSlider = new Rect(20, 120, 340, 30);
        private readonly Rect _sliderWindowBox = new Rect(10, 80, 360, 80);
        private readonly Rect _sliderKeyWindowBox = new Rect(10, 220, 360, 80);
        private readonly Rect _guiKeySlider = new Rect(20, 260, 340, 30);

        //Variables
        private bool _flyEnabled; //Boolean Noclip Toggle

        private bool _guiShow;
        private float _speed = 10;

        private bool _highSpeedWindow;
        private bool _lowSpeedWindow;
        private bool _midSpeedWindow = true;
        private bool _turboKeyWindow = true;
        private bool _snailKeyWindow;
        private float _turboSpeed = 100;
        private float _snailSpeed = 1;
        private Vector3 _defaultRotation;

        private bool cameraFollow = true;
        private readonly Timer showToggleTextTimer = new Timer(1000);

        private string _followCameraEnabled = "Yes";

        private bool justEnabled;

        private bool justDisabled;

        // The ID of the mod - Should be unique
        public override string ID => "MscNoclip";

        // The name of the mod that is displayed
        public override string Name => "Msc Noclip";

        // The name of the author
        public override string Author => "haverdaden";

        // The version of the mod
        public override string Version => "1.3.1";


        // Called when the mod is loaded
        public override void OnLoad()
        {
            // Do your initialization here

            Keybind.Add(this, _flyKey);
            Keybind.Add(this, _noclipGuiKey);     

            ModConsole.Print("[MscNoclip] has been loaded! Toggle ON/OFF [Ctrl + N]");
            ModConsole.Print("[MscNoclip] Toggle Settings Window [Ctrl + B]");
            ModConsole.Print("[MscNoclip]: Made by haverdaden (DD). See RaceDepartment for help!");
        }

        // Called to draw the GUI
        public override void OnGUI()
        {
            var warningStyle = new GUIStyle();
            warningStyle.alignment = TextAnchor.UpperCenter;
            warningStyle.normal.textColor = Color.yellow;
            warningStyle.fontSize = 20;
            // Draw your GUI here
            if (_flyEnabled)
            {
                if (justEnabled)
                {
                    GUI.Label(
                        new Rect(Screen.width / 2 - 250, Screen.height - Screen.height / 2 + 40, 500, 40),
                        "Noclip Enabled!",
                        warningStyle);
                }



                if (_guiShow) { 
                    GUI.ModalWindow(888, _guiBox, GuiSettingsWindow, "Noclip Speed | Noclip mod by haverdaden(DD)");
                }

            }

            if (justDisabled)
            {

                GUI.Label(
                    new Rect(Screen.width / 2 - 250, Screen.height - Screen.height / 2 + 40, 500, 40),
                    "Noclip Disabled!",
                    warningStyle);
            }
        }

        // Called every tick
        public override void Update()
        {
            if (_flyKey.IsDown()) NoclipToggle();
            if (_flyEnabled) NoclipMove();
            if (_noclipGuiKey.IsDown()) GuiToggle();

        }

        //Toggle Gui
        private void GuiToggle()
        {
            _guiShow = _guiShow == false;
        }

        //Gui Window
        private void GuiSettingsWindow(int windowId)
        {

            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;

            if (GUI.Button(new Rect(20, 30, 100, 30), "Low speed"))
            {
                _midSpeedWindow = false;
                _highSpeedWindow = false;
                _lowSpeedWindow = true;
            }
            if (GUI.Button(new Rect(140, 30, 100, 30), "Mid speed"))
            {
                _midSpeedWindow = true;
                _highSpeedWindow = false;
                _lowSpeedWindow = false;
            }
            if (GUI.Button(new Rect(260, 30, 100, 30), "High speed"))
            {
                _midSpeedWindow = false;
                _highSpeedWindow = true;
                _lowSpeedWindow = false;
            }

            GUI.Label(new Rect(40, 300, 300, 30),"Following Camera: " + _followCameraEnabled,centeredStyle);
            if (GUI.Button(new Rect(90, 320, 200, 30), "Toggle Follow Camera"))
            {
                if (cameraFollow)
                {
                    this._followCameraEnabled = "No";
                    this.cameraFollow = false;
                }
                else
                {
                    this._followCameraEnabled = "Yes";
                    this.cameraFollow = true;
                }
            }
               



            if (GUI.Button(new Rect(90, 360, 100, 30), "Close"))
                _guiShow = false;
            if (GUI.Button(new Rect(190, 360, 100, 30), "Default"))
            {
                _speed = 10;
                _snailSpeed = 1;
                _turboSpeed = 100;

            }


            if (GUI.Button(new Rect(55, 170, 130, 30), "Snail key speed"))
            {
                _snailKeyWindow = true;
                _turboKeyWindow = false;

            }
            if (GUI.Button(new Rect(195, 170, 130, 30), "TURBO key speed"))
            {
                _turboKeyWindow = true;
                _snailKeyWindow = false;
            }

            if (_snailKeyWindow && !_turboKeyWindow)
            {

                GUI.Box(_sliderKeyWindowBox, "SNAIL 0.1-10 | SpeedNow: " + _snailSpeed);
                _snailSpeed = GUI.HorizontalSlider(_guiKeySlider, _snailSpeed,  0.1f, 10.0f);
            }

            if (_turboKeyWindow && !_snailKeyWindow)
            {

                GUI.Box(_sliderKeyWindowBox, "TURBO 10-1000 | SpeedNow: " + _turboSpeed);
                _turboSpeed = GUI.HorizontalSlider(_guiKeySlider, _turboSpeed,  10.0f, 1000.0f);
            }

            if (_lowSpeedWindow && !_highSpeedWindow && !_midSpeedWindow)
            {
                GUI.Box(_sliderWindowBox, "Speed 0.1-10 | SpeedNow: " + _speed);
                _speed = GUI.HorizontalSlider(_guiSlider, _speed, 1.0f, 10.0f);
            }

            if (_midSpeedWindow && !_lowSpeedWindow && !_highSpeedWindow)
            {
                GUI.Box(_sliderWindowBox, "Speed 10-100 | SpeedNow: " + _speed);
                _speed = GUI.HorizontalSlider(_guiSlider, _speed, 10.0f, 100.0f);
            }

            if (_highSpeedWindow && !_lowSpeedWindow && !_midSpeedWindow)
            {
                GUI.Box(_sliderWindowBox, "Speed 100-1000 | SpeedNow: " + _speed);
                _speed = GUI.HorizontalSlider(_guiSlider, _speed, 100.0f, 1000.0f);
            }

            


        }

        //Noclip Toggle
        private void NoclipToggle()
        {
            if (_flyEnabled == false)
            {
                justEnabled = false;
                justDisabled = false;
                showToggleTextTimer.Stop();
                showToggleTextTimer.Enabled = false;
                _flyEnabled = true;
                this.justEnabled = true;

                var player = GameObject.Find("PLAYER");
                foreach (var fsm in player.GetComponents<Collider>())
                {
                    
                    fsm.enabled = false;
                }


                    
                    this.showToggleTextTimer.Enabled = true;
                    this.showToggleTextTimer.Elapsed += this.HideNoclipEnabledLabel;
                
            }

            else
            {
                justEnabled = false;
                justDisabled = false;
                showToggleTextTimer.Stop();
                showToggleTextTimer.Enabled = false;
                _flyEnabled = false;
                this.justDisabled = true;
                ModConsole.Print(this.justDisabled);

                var player = GameObject.Find("PLAYER");
                
                foreach (var fsm in player.GetComponents<Collider>())
                {
                    
                    fsm.enabled = true;
                }
               

                    
                    this.showToggleTextTimer.Enabled = true;
                    this.showToggleTextTimer.Elapsed += this.HideNoclipDisabledLabel;
                
                
                
            }
        }

        private void HideNoclipDisabledLabel(object sender, ElapsedEventArgs e)
        {
            this.justDisabled = false;
            this.showToggleTextTimer.Stop();
            this.showToggleTextTimer.Enabled = false;
        }

        private void HideNoclipEnabledLabel(object sender, ElapsedEventArgs e)
        {
            this.justEnabled = false;
            this.showToggleTextTimer.Stop();
            this.showToggleTextTimer.Enabled = false;
        }

        //Noclip Movement
        private void NoclipMove()
        {
            var player = GameObject.Find("PLAYER");
            var fpscamera = GameObject.Find("FPSCamera");
            var tempSpeed = this._speed;

            if (Input.GetKey(KeyCode.LeftShift)) tempSpeed = this._turboSpeed;
            if (Input.GetKey(KeyCode.LeftAlt)) tempSpeed = this._snailSpeed;
            if (Input.GetKey(KeyCode.W) && this.cameraFollow)
                player.transform.Translate(fpscamera.transform.forward * tempSpeed * Time.deltaTime, Space.World);
            else if (Input.GetKey(KeyCode.W))
                player.transform.Translate(Vector3.forward * tempSpeed * Time.deltaTime, Space.Self);
            if (Input.GetKey(KeyCode.A))
                player.transform.Translate(Vector3.left * tempSpeed * Time.deltaTime, Space.Self);
            if (Input.GetKey(KeyCode.D))
                player.transform.Translate(Vector3.right * tempSpeed * Time.deltaTime, Space.Self);
            if (Input.GetKey(KeyCode.S) && this.cameraFollow)
                player.transform.Translate(fpscamera.transform.forward * -tempSpeed * Time.deltaTime, Space.World);
            else if (Input.GetKey(KeyCode.S))
                player.transform.Translate(Vector3.back * tempSpeed * Time.deltaTime, Space.Self);
            if (Input.GetKey(KeyCode.Q))
                player.transform.Translate(Vector3.up * tempSpeed * Time.deltaTime, Space.Self);
            if (Input.GetKey(KeyCode.E))
                player.transform.Translate(Vector3.down * tempSpeed * Time.deltaTime, Space.Self);
        }
    }
}