using System;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace EngineFixer
{
    public class EngineFixer : Mod
    {
        //Mod info
        public override string ID => "EngineFixer";
        public override string Name => "EngineFixer";
        public override string Author => "haverdaden";
        public override string Version => "1.1";

        //Bool
        private bool _guiShow;
        private bool _getMotorValues;

        //GUIBOX
        private readonly Rect _guiBox = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 260, 300, 520);

        // Keybinds
        private readonly Keybind _showWearGui =
            new Keybind("EngineFixerGuiKey", "EngineFixer Gui", KeyCode.F, KeyCode.RightControl);

        //Fsmfloats
        private FsmFloat _wearAlternator;
        private FsmFloat _wearCrankshaft;
        private FsmFloat _wearGearbox;
        private FsmFloat _wearHeadgasket;
        private FsmFloat _wearPiston1;
        private FsmFloat _wearPiston2;
        private FsmFloat _wearPiston3;
        private FsmFloat _wearPiston4;
        private FsmFloat _wearRockershaft;
        private FsmFloat _wearStarter;
        private FsmFloat _wearWaterpump;
        private FsmFloat _oilLevel;
        private FsmFloat _wearFanbelt;
        private FsmFloat _satsumaFuel;
        private FsmFloat _racingWater;
        private FsmFloat _brakeFluidF;
        private FsmFloat _brakeFluidR;
        private FsmFloat _cluthFluid;


        //Called when mod is loading
        public override void OnLoad()
        {
            Keybind.Add(this, _showWearGui);
        }

        // Update is called once per frame
        public override void Update()
        {
            //Unload Mod
            UnloadMod();

            //Toggle GUI
            GuiToggler();

            //IF Gui is open update
            NewGameLoader();
        }

        private void NewGameLoader()
        {
            if (_guiShow && Application.loadedLevel == 3)
                GetMotorStatus();
        }

        private void GuiToggler()
        {
            if (_showWearGui.IsDown())
                _guiShow = !_guiShow;
        }

        private void UnloadMod()
        {
            if (Application.loadedLevel != 3)
                _getMotorValues = false;
        }

        //Called when mod is loading
        public override void OnGUI()
        {
            GUI.backgroundColor = Color.black;

            if (Application.loadedLevel == 3)
                if (_guiShow)
                    GUI.ModalWindow(887, _guiBox, GuiEngineTuner,
                        "<color=green><b>EngineFixer by haverdaden(DD)</b></color>");
        }

        private void GuiEngineTuner(int id)
        {
            //Gui Style
            GUIStyle style, buttonStyle;
            GuiStyle(out style, out buttonStyle);

            //All engine labels
            EngineLabels(style);

            //Gui buttons
            GuiButtons(style, buttonStyle);
        }

        private static void GuiStyle(out GUIStyle style, out GUIStyle buttonStyle)
        {
            //GUI STYLING
            style = new GUIStyle(GUI.skin.GetStyle("Label"));
            buttonStyle = new GUIStyle(GUI.skin.GetStyle("button"));
            style.alignment = TextAnchor.UpperCenter;
            style.normal.textColor = Color.green;
            buttonStyle.normal.textColor = Color.green;
        }

        private void GuiButtons(GUIStyle style, GUIStyle buttonStyle)
        {
            GUI.Label(new Rect(0, 420, 300, 30), "Removes Part Wear", style);
            if (GUI.Button(new Rect(50, 440, 200, 30), "Fix Engine", buttonStyle))
                FixEngine();

            if (GUI.Button(new Rect(50, 470, 200, 30), "Fill Fluids", buttonStyle))
                FillFluids();

            if (GUI.Button(new Rect(250, 20, 35, 35), "╳"))
                _guiShow = false;
        }

        private void EngineLabels(GUIStyle style)
        {
            if (_wearAlternator != null)
                GUI.Label(new Rect(0, 40, 300, 20), "WearAlternator: " + _wearAlternator.Value, style);
            else
                GUI.Label(new Rect(0, 40, 300, 20), "Error: Not found.", style);

            if (_wearCrankshaft != null)
                GUI.Label(new Rect(0, 60, 300, 20), "WearCrankshaft: " + _wearCrankshaft.Value, style);
            else
                GUI.Label(new Rect(0, 60, 300, 20), "Error: Not found.", style);

            if (_wearGearbox != null)
                GUI.Label(new Rect(0, 80, 300, 20), "WearGearbox: " + _wearGearbox.Value, style);
            else
                GUI.Label(new Rect(0, 80, 300, 20), "Error: Not found.", style);
            if (_wearHeadgasket != null)
                GUI.Label(new Rect(0, 100, 300, 20), "WearHeadgasket: " + _wearHeadgasket.Value, style);
            else
                GUI.Label(new Rect(0, 100, 300, 20), "Error: Not found.", style);
            if (_wearPiston1 != null)
                GUI.Label(new Rect(0, 120, 300, 20), "WearPiston1: " + _wearPiston1.Value, style);
            else
                GUI.Label(new Rect(0, 120, 300, 20), "Error: Not found.", style);
            if (_wearPiston2 != null)
                GUI.Label(new Rect(0, 140, 300, 20), "WearPiston2: " + _wearPiston2.Value, style);
            else
                GUI.Label(new Rect(0, 140, 300, 20), "Error: Not found.", style);
            if (_wearPiston3 != null)
                GUI.Label(new Rect(0, 160, 300, 20), "WearPiston3: " + _wearPiston3.Value, style);
            else
                GUI.Label(new Rect(0, 160, 300, 20), "Error: Not found.", style);
            if (_wearPiston4 != null)
                GUI.Label(new Rect(0, 180, 300, 20), "WearPiston4: " + _wearPiston4.Value, style);
            else
                GUI.Label(new Rect(0, 180, 300, 20), "Error: Not found.", style);
            if (_wearRockershaft != null)
                GUI.Label(new Rect(0, 200, 300, 20), "WearRockershaft: " + _wearRockershaft.Value, style);
            else
                GUI.Label(new Rect(0, 200, 300, 20), "Error: Not found.", style);
            if (_wearStarter != null)
                GUI.Label(new Rect(0, 220, 300, 20), "WearStarter: " + _wearStarter.Value, style);
            else
                GUI.Label(new Rect(0, 220, 300, 20), "Error: Not found.", style);
            if (_wearWaterpump != null)
                GUI.Label(new Rect(0, 240, 300, 20), "WearWaterpump: " + _wearWaterpump.Value, style);
            else
                GUI.Label(new Rect(0, 240, 300, 20), "Error: Not found.", style);
            if (_wearFanbelt != null)
                GUI.Label(new Rect(0, 260, 300, 20), "WearFanbelt: " + (int) _wearFanbelt.Value, style);
            else
                GUI.Label(new Rect(0, 260, 300, 20), "Error: Not found.", style);

            if (_oilLevel != null)
                GUI.Label(new Rect(0, 290, 300, 20), "OilLevel: " + _oilLevel, style);
            else
                GUI.Label(new Rect(0, 290, 300, 20), "Error: Not found.", style);
            if (_racingWater != null)
                GUI.Label(new Rect(0, 310, 300, 20), "WaterlevelRacing: " + _racingWater.Value, style);
            else
                GUI.Label(new Rect(0, 310, 300, 20), "Error: Not found.", style);
            if (_brakeFluidF != null)
                GUI.Label(new Rect(0, 330, 300, 20), "BrakeFluidF: " + _brakeFluidF.Value, style);
            else
                GUI.Label(new Rect(0, 330, 300, 20), "Error: Not found.", style);
            if (_brakeFluidR != null)
                GUI.Label(new Rect(0, 350, 300, 20), "BrakeFluidR: " + _brakeFluidR.Value, style);
            else
                GUI.Label(new Rect(0, 350, 300, 20), "Error: Not found.", style);
            if (_cluthFluid != null)
                GUI.Label(new Rect(0, 370, 300, 20), "ClutchFluid: " + _cluthFluid.Value, style);
            else
                GUI.Label(new Rect(0, 370, 300, 20), "Error: Not found.", style);
            if (_satsumaFuel != null)
                GUI.Label(new Rect(0, 390, 300, 20), "FuelLevel: " + _satsumaFuel.Value, style);
            else
                GUI.Label(new Rect(0, 390, 300, 20), "Error: Not found.", style);
        }

        private void FillFluids()
        {
            if (_satsumaFuel != null)
                _satsumaFuel.Value = 36;
            if (_oilLevel != null)
                _oilLevel.Value = 3;
            if (_racingWater != null)
                _racingWater.Value = 7;
            if (_cluthFluid != null)
                _cluthFluid.Value = 0.5f;
            if (_brakeFluidF != null)
                _brakeFluidF.Value = 1;
            if (_brakeFluidR != null)
                _brakeFluidR.Value = 1;
        }

        private void FixEngine()
        {
            var satsuma = GameObject.Find("SATSUMA(557kg)");

            foreach (var fsm in satsuma.GetComponentsInChildren<PlayMakerFSM>())
            foreach (var floats in fsm.FsmVariables.FloatVariables)
                if (floats.Name == "WearAlternator" || floats.Name == "WearCrankshaft" ||
                    floats.Name == "WearGearbox" || floats.Name == "WearHeadgasket"
                    || floats.Name == "WearPiston1" || floats.Name == "WearPiston2" || floats.Name == "WearPiston3"
                    || floats.Name == "WearPiston4" || floats.Name == "WearRockershaft" ||
                    floats.Name == "WearStarter" || floats.Name == "WearWaterpump")
                    floats.Value = 100f;

            if (_wearFanbelt != null)
                _wearFanbelt.Value = 0;
        }

        private void GetMotorStatus()
        {
            if (!_getMotorValues)
            {
                if (GameObject.Find("SATSUMA(557kg)") != null)
                {
                    foreach (var fsm in Resources.FindObjectsOfTypeAll<PlayMakerFSM>())
                    {
                        var fsmfloat = new Func<string, FsmFloat>(fsm.FsmVariables.FindFsmFloat);

                        if (fsm.FsmVariables.FindFsmFloat("WearAlternator") != null &&
                            fsm.FsmVariables.FindFsmFloat("WearGearbox") != null &&
                            fsm.FsmVariables.FindFsmFloat("WearCrankshaft") != null && fsmfloat("WearHeadgasket")
                            != null && fsmfloat("WearPiston1") != null
                            && fsmfloat("WearPiston2") != null && fsmfloat("WearPiston3") != null &&
                            fsmfloat("WearPiston4") != null && fsmfloat("WearRockershaft") != null
                            && fsmfloat("WearStarter") != null && fsmfloat("WearWaterpump") != null)
                        {
                            _wearAlternator = fsm.FsmVariables.FindFsmFloat("WearAlternator");
                            _wearCrankshaft = fsm.FsmVariables.FindFsmFloat("WearCrankshaft");
                            _wearGearbox = fsm.FsmVariables.FindFsmFloat("WearGearbox");
                            _wearHeadgasket = fsm.FsmVariables.FindFsmFloat("WearHeadgasket");
                            _wearPiston1 = fsm.FsmVariables.FindFsmFloat("WearPiston1");
                            _wearPiston2 = fsm.FsmVariables.FindFsmFloat("WearPiston2");
                            _wearPiston3 = fsm.FsmVariables.FindFsmFloat("WearPiston3");
                            _wearPiston4 = fsm.FsmVariables.FindFsmFloat("WearPiston4");
                            _wearRockershaft = fsm.FsmVariables.FindFsmFloat("WearRockershaft");
                            _wearStarter = fsm.FsmVariables.FindFsmFloat("WearStarter");
                            _wearWaterpump = fsm.FsmVariables.FindFsmFloat("WearWaterpump");

                            _wearFanbelt = GameObject.Find("Fanbelt").GetComponent<PlayMakerFSM>().FsmVariables
                                .FindFsmFloat("Wear");
                        }
                        if (fsm.gameObject.transform.root.name == "Database")
                        {
                            if (fsm.gameObject.name == "FuelTank")
                                _satsumaFuel = fsm.FsmVariables.FindFsmFloat("FuelLevel");
                            if (fsm.gameObject.name == "Oilpan")
                                _oilLevel = fsm.FsmVariables.FindFsmFloat("Oil");
                            if (fsm.gameObject.name == "Racing Radiator")
                                _racingWater = fsm.FsmVariables.FindFsmFloat("Water");
                            if (fsm.gameObject.name == "BrakeMasterCylinder")
                            {
                                _brakeFluidF = fsm.FsmVariables.FindFsmFloat("BrakeFluidF");
                                _brakeFluidR = fsm.FsmVariables.FindFsmFloat("BrakeFluidR");
                            }
                            if (fsm.gameObject.name == "ClutchMasterCylinder")
                                _cluthFluid = fsm.FsmVariables.FindFsmFloat("ClutchFluid");
                        }
                    }
                    _getMotorValues = true;
                }
            }
        }
    }
}



