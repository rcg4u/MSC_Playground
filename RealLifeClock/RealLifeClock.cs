namespace RealLifeClock
{
    using System;
    using System.Timers;

    using MSCLoader;

    using UnityEngine;

    public class RealLifeClock : Mod
    {
        //Enabled/Disabled
        private bool guiEnabled;

        private readonly Rect _guiBox = new Rect(Screen.width / 2 - 107.5f, Screen.height / 2 - 85, 215, 170);

        private bool globalTime;

        private bool amPmTime;

        private string ampm;

        private string global;

        private int minute;

        private float timeSinceStart;

        private int seconds;

        private int hour;

        private bool playedTimeEnabled;

        private readonly Timer timePlayedCheckTimer = new Timer();

        private readonly Timer systemTimeCheckTimer = new Timer();

        private int hh;

        private int mm;

        // The ID of the mod - Should be unique
        public override string ID => "IRL Clock";

        // The name of the mod that is displayed
        public override string Name => "IRL Clock";

        // The name of the author
        public override string Author => "haverdaden (DD)";

        // The version of the mod
        public override string Version => "1.0.1";

        // Keybinds
        private readonly Keybind testKey = new Keybind("ClockKey", "ClockToggleGUI", KeyCode.T, KeyCode.LeftControl);

        // Called when the mod is loaded
        public override void OnLoad()
        {
            this.timePlayedCheckTimer.Elapsed += this.CheckTimeplayed;
            this.systemTimeCheckTimer.Elapsed += this.CheckRealTime;
            this.timePlayedCheckTimer.Interval = 10000;
            this.systemTimeCheckTimer.Interval = 10000;
            this.systemTimeCheckTimer.Enabled = true;
            this.timePlayedCheckTimer.Enabled = true;
            this.ampm = DateTime.Now.ToString("h:mm tt");
            this.global = DateTime.Now.ToString("HH:mm");

            Keybind.Add(this, this.testKey);

            ModConsole.Print("RealLifeClock has been loaded! Made by haverdaden(DD)!");
        }

        // Called to draw the GUI
        public override void OnGUI()
        {
            var myStyle = new GUIStyle();
            myStyle.fontStyle = FontStyle.Bold;
            myStyle.normal.textColor = Color.white;

            if (this.guiEnabled) GUI.ModalWindow(888, this._guiBox, this.GuiSettingsWindow, "Time Format");

            if (this.globalTime) GUI.Label(new Rect(Screen.width - 53, 10, 50, 20), this.global, myStyle);
            if (this.amPmTime) GUI.Label(new Rect(Screen.width - 75, 10, 80, 20), this.ampm, myStyle);
            if (this.playedTimeEnabled)
                GUI.Label(
                    new Rect(Screen.width - 108, 30, 80, 20),
                    "Played: " + this.hh + "h:" + this.mm + "m",
                    myStyle);
        }

        // Called every tick
        public override void Update()
        {
            if (this.testKey.IsDown())
                if (this.guiEnabled == false) this.guiEnabled = true;
                else this.guiEnabled = false;
        }

        private void GuiSettingsWindow(int id)
        {
            if (GUI.Button(new Rect(5, 30, 100, 30), "24-Hour Clock"))
            {
                this.globalTime = true;
                this.amPmTime = false;
            }
            if (GUI.Button(new Rect(110, 30, 100, 30), "12-Hour Clock"))
            {
                this.amPmTime = true;
                this.globalTime = false;
            }
            if (GUI.Button(new Rect(17.5f, 75, 180, 30), "Time Played Session"))
                if (this.playedTimeEnabled) this.playedTimeEnabled = false;
                else this.playedTimeEnabled = true;
            if (GUI.Button(new Rect(5, 120, 100, 30), "Close")) this.guiEnabled = false;
            if (GUI.Button(new Rect(110, 120, 100, 30), "Disable"))
            {
                this.amPmTime = false;
                this.globalTime = false;
                this.playedTimeEnabled = false;
            }
        }

        public void CheckTimeplayed(object source, ElapsedEventArgs e)
        {
            var secondConverter = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
            this.hh = secondConverter.Hours;
            this.mm = secondConverter.Minutes;

            //  this.minute = (int)Time.timeSinceLevelLoad / 60;
            // this.hour = (int)Time.timeSinceLevelLoad / 3600;
        }

        private void CheckRealTime(object source, ElapsedEventArgs e)
        {
            this.ampm = DateTime.Now.ToString("h:mm tt");
            this.global = DateTime.Now.ToString("HH:mm");
        }
    }
}
