using MSCLoader;
using UnityEngine;
using HutongGames;
namespace WheelieMod
{
    using System;

    public class WheelieMod : Mod
	{
		// The ID of the mod - Should be unique
		public override string ID { get { return "WheelieMod"; } }

		// The name of the mod that is displayed
		public override string Name { get { return "WheelieMod"; } }
		
		// The name of the author
		public override string Author { get { return "haverdaden"; } }

		// The version of the mod
		public override string Version { get { return "1.0.0"; } }


        // Keybinds
        private Keybind wheelieGui = new Keybind("WheelieKey", "Wheelie Key", KeyCode.V, KeyCode.LeftControl);
	    private readonly Rect _guiBox = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 120, 400, 230);
	    private readonly Rect _sliderKeyWindowBox = new Rect(135, 20, 195, 20);
	    private readonly Rect _guiKeySlider = new Rect(5, 50, 390, 30);

	    private KeyCode wheelieKey = KeyCode.Mouse2;

	    private bool _showWheelieGui;

	    private float _wheelieSensitivity = 10.0f;

	    private bool _wheelieEnabled = true;

	    private string _wheelieEnabledString = "Enabled";

	    private string _wheelieKeyString = "Mouse 2";

	    private bool _waitingForInput;

	    // Called when the mod is loaded
		public override void OnLoad()
		{
			// Do your initialization here

			Keybind.Add(this, this.wheelieGui);


			ModConsole.Print("WheelieMod by haverdaden (DD) has been loaded!");
		}

        // Called to draw the GUI
	    public override void OnGUI()
	    {
	        var myStyle = new GUIStyle();

	        myStyle.fontSize = 20;
	        myStyle.fontStyle = FontStyle.Bold;
	        myStyle.normal.textColor = Color.yellow;
	        GUI.backgroundColor = Color.black;
	        GUI.skin.window.fontSize = 12;

	        if (this._showWheelieGui) GUI.ModalWindow(123, this._guiBox, this.GuiGrabWindow, "WheelieMod Settings | WheelieMod by haverdaden (DD)");
	    }

	    private void GuiGrabWindow(int id)
	    {
            //Wheelie Sensitivity
            GUI.Label(this._sliderKeyWindowBox, "Wheelie Sensitivity: " + (int)this._wheelieSensitivity);
	        this._wheelieSensitivity = GUI.HorizontalSlider(this._guiKeySlider, this._wheelieSensitivity, 1f, 20f);
	        var keyDown = Event.current;

	        GUI.Label(new Rect(150, 70, 200, 30), "Wheelie Key");
	        if (GUI.Button(new Rect(100, 90, 200, 30), this._wheelieKeyString))
	        {
	            if (_waitingForInput == false)
	            {
	                this._waitingForInput = true;
	              
	            }
               
	        }
	        if (_waitingForInput)
	        {
	            this._wheelieKeyString = "Waiting for input!";
                if (keyDown.isKey && keyDown.keyCode != KeyCode.None)
	            {
	                this.wheelieKey = keyDown.keyCode;
	                this._wheelieKeyString = this.wheelieKey.ToString();
	                this._waitingForInput = false;
	            }
	        }
            


                //Toggle On/Off
                GUI.Label(new Rect(135, 125, 200, 30),
	            "Toggle MOD ON/OFF");
	        if (GUI.Button(new Rect(100, 145, 200, 30), this._wheelieEnabledString))
	        {
	            if (_wheelieEnabled)
	            {
	                this._wheelieEnabled = false;
	                this._wheelieEnabledString = "Disabled";
	            }
	            else
	            {
	                this._wheelieEnabled = true;
	                this._wheelieEnabledString = "Enabled";
	            }
	        }

	        //Close and Default
	        if (GUI.Button(new Rect(120, 195, 80, 30), "Close")) this._showWheelieGui = false;
	        if (GUI.Button(new Rect(200, 195, 80, 30), "Default"))
	        {
	            this._wheelieSensitivity = 10.0f;
	            this.wheelieKey = KeyCode.Mouse2;
	        }
	    }

        // Called every tick
        public override void Update()
		{
			// Do your updating here

		    if (this.wheelieGui.IsDown())
            {
                ToggleWheelieGUI();

            }

		    if (_wheelieEnabled)
            {
                CheckWheelieKeyDown();
            }

        }

        private void CheckWheelieKeyDown()
        {
            if (Input.GetKey(this.wheelieKey))
            {
                WheelieRun();
            
            }
                
        }

        private void WheelieRun()
        {
            //Find PV and front fork
            try
            {
                GameObject jonne = GameObject.Find("JONNEZ ES(Clone)");
                GameObject fork = GameObject.Find("ForkFront");
                Rigidbody jonneRB = jonne.GetComponent<Rigidbody>();

                //WheelieForce
                jonneRB.AddForceAtPosition(jonne.transform.up * this._wheelieSensitivity, fork.transform.position, ForceMode.Acceleration);
            }
            catch (Exception e)
            {
                ModConsole.Print(e);
                throw;
            }

     
        }

        private void ToggleWheelieGUI()
        {
            if (this._showWheelieGui == false)
            {
                _showWheelieGui = true;

            }
            else
            {
                _showWheelieGui = false;
            }
        }
    }
}
