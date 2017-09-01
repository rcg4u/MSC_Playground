using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace ShowWear
{
    public class ShowWear : Mod
    {
        private bool enabled = true;
        private FsmFloat WearAlternator;
        private FsmFloat WearCrankshaft;
        private FsmFloat WearGearbox;
        private FsmFloat WearHeadgasket;
        private FsmFloat WearPiston1;
        private FsmFloat WearPiston2;
        private FsmFloat WearPiston3;
        private FsmFloat WearPiston4;
        private FsmFloat WearRockershaft;
        private FsmFloat WearStarter;
        private FsmFloat WearWaterpump;
        private List<FsmFloat> fsmList;
        private bool labelsNotCreated;
        private float height;
        private string partName;
        private float pratValue;

        // Keybinds
        private readonly Keybind showWearKeybind =
            new Keybind("ShowTheWear", "Show the Wear", KeyCode.Z, KeyCode.LeftControl);

        private bool FoundGameobjects;
        private GameObject satsuma;
        private GameObject player;
        private bool notFound;


        public override string ID => "ShowWear";
        public override string Name => "ShowWear";
        public override string Author => "haverdaden";
        public override string Version => "1.1";

        //Called when mod is loading
        public override void OnLoad()
        {
            Keybind.Add(this, showWearKeybind);
        }

        public override void OnGUI()
        {
            if (enabled)
                if (fsmList.Any())
                {
                    height = 20;
                    partName = "NONE";
                    pratValue = 0;
                    var mystaly = new GUIStyle();
                    //   mystaly.fontStyle = FontStyle.Bold;
                    mystaly.normal.textColor = Color.yellow;
                    mystaly.fontSize = 12;
                    mystaly.alignment = TextAnchor.UpperRight;


                    foreach (var labels in fsmList)
                    {
                        GUI.Label(new Rect(Screen.width - 160, Screen.height - height, 150, 20),
                            labels.Name + " | " + "<color=orange><b>" + Math.Round(labels.Value, 1) + "</b></color>",
                            mystaly);
                        height = height + 20;
                    }
                }
        }

        // Update is called once per frame
        public override void Update()
        {
            GetGameobjects();
            GetMotorStatus();
            ResetDefault();
            ToggleWearGui();
        }

        private void ToggleWearGui()
        {
            if (showWearKeybind.IsDown())
                if (enabled)
                    enabled = false;
                else
                    enabled = true;
        }

        private void GetGameobjects()
        {
            if (!FoundGameobjects)
                if (GameObject.Find("SATSUMA(557kg)") && Application.loadedLevel == 3)
                {
                    satsuma = GameObject.Find("SATSUMA(557kg)");
                    enabled = true;
                    FoundGameobjects = true;
                }
        }

        private void ResetDefault()
        {
            if (Application.loadedLevel != 3)
            {
                FoundGameobjects = false;
                notFound = false;
                enabled = false;
            }
        }

        private void GetMotorStatus()
        {
            if (!notFound)
                if (satsuma)
                {
                    foreach (var fsm in satsuma.GetComponentsInChildren<PlayMakerFSM>())
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
                            WearAlternator = fsm.FsmVariables.FindFsmFloat("WearAlternator");
                            WearCrankshaft = fsm.FsmVariables.FindFsmFloat("WearCrankshaft");
                            WearGearbox = fsm.FsmVariables.FindFsmFloat("WearGearbox");
                            WearHeadgasket = fsm.FsmVariables.FindFsmFloat("WearHeadgasket");
                            WearPiston1 = fsm.FsmVariables.FindFsmFloat("WearPiston1");
                            WearPiston2 = fsm.FsmVariables.FindFsmFloat("WearPiston2");
                            WearPiston3 = fsm.FsmVariables.FindFsmFloat("WearPiston3");
                            WearPiston4 = fsm.FsmVariables.FindFsmFloat("WearPiston4");
                            WearRockershaft = fsm.FsmVariables.FindFsmFloat("WearRockershaft");
                            WearStarter = fsm.FsmVariables.FindFsmFloat("WearStarter");
                            WearWaterpump = fsm.FsmVariables.FindFsmFloat("WearWaterpump");

                            fsmList = new List<FsmFloat>();

                            fsmList.Add(WearAlternator);
                            fsmList.Add(WearCrankshaft);
                            fsmList.Add(WearGearbox);
                            fsmList.Add(WearHeadgasket);
                            fsmList.Add(WearPiston1);
                            fsmList.Add(WearPiston2);
                            fsmList.Add(WearPiston3);
                            fsmList.Add(WearPiston4);
                            fsmList.Add(WearRockershaft);
                            fsmList.Add(WearStarter);
                            fsmList.Add(WearWaterpump);
                        }
                    }
                    notFound = true;
                }
        }
    }
}
