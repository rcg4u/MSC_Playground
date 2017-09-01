namespace GrabBody
{
    using System;
    using System.Collections;
    using System.Timers;

    using MSCLoader;

    using UnityEngine;

    public class GrabBody : Mod
    {
        private bool _guiShow;

        private bool grabbedBody;

        // Keybinds
        private readonly Keybind pickKey = new Keybind("PickBody", "PickBodyKey", KeyCode.Mouse0, KeyCode.None);

        private Ray ray;

        private GameObject saveBody;

        private readonly Keybind throwKey = new Keybind("ThrowBody", "ThrowBodyKey", KeyCode.Mouse1, KeyCode.None);

        private readonly Timer Timer = new Timer(10000);

        private bool _warningMessageEnabled = true;

        // The name of the author
        public override string Author => "haverdaden";

        // The ID of the mod - Should be unique
        public override string ID => "BodyGrab";

        // The name of the mod that is displayed
        public override string Name => "BodyGrab";

        // The version of the mod
        public override string Version => "1.0.1";

        // Called to draw the GUI
        public override void OnGUI()
        {
            var myStyle = new GUIStyle();
            var warningStyle = new GUIStyle();
            myStyle.fontSize = 20;
            myStyle.fontStyle = FontStyle.Bold;
            myStyle.normal.textColor = Color.yellow;
            warningStyle = GUI.skin.GetStyle("Label");
            warningStyle.alignment = TextAnchor.UpperCenter;
            warningStyle.fontSize = 20;
            warningStyle.normal.background = Texture2D.blackTexture;
            GUI.backgroundColor = Color.black;;


            if (this._guiShow)
            {
                GUI.Label(
                    new Rect(Screen.width / 2 - 43, Screen.height - Screen.height / 8 - 20, 150, 20),
                    "Grab Body",
                    myStyle);
            }

            if (_warningMessageEnabled)
            {
                GUI.Label(
                    new Rect(Screen.width / 2 - 250, Screen.height - Screen.height / 2 - 80, 500, 40),
                    "WARNING: Do not use this mod with Grabanything!",
                    warningStyle);
                
            }
        }

        // Called when the mod is loaded
        public override void OnLoad()
        {
            // Do your initialization here
            Keybind.Add(this, this.pickKey);
            Keybind.Add(this, this.throwKey);

            ModConsole.Print("BodyGrab by haverdaden (DD) has been loaded!");

            this.Timer.Enabled = true;
            this.Timer.Elapsed += this.HideLablel;
           

            //   this.timePlayedCheckTimer.Interval = 10000;






        }

        private void HideLablel(object source, ElapsedEventArgs e)
        {
            ModConsole.Print("HEEEEEEEEEEEEEEEEEY");
            this.Timer.Stop();
            this.Timer.Enabled = false;
            this._warningMessageEnabled = false;
        }

        // Called every tick
        public override void Update()
        {

          
            // Do your updating here
            if (this.pickKey.IsDown()) this.BodyFinder();

            if (this.throwKey.IsDown()) this.ThrowBody();

            this.rotateBody();

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1.0f) && this.grabbedBody == false)
            {
                if (hit.transform.name == "head" || hit.transform.name == "shoulder(right)"
                    || hit.transform.name == "arm(right)" || hit.transform.name == "shoulders(xxxxx)"
                    || hit.transform.name == "shoulder(leftx)" || hit.transform.name == "RagDoll"
                    || hit.transform.name == "arm(leftx)" || hit.transform.name == "thigh(right)"
                    || hit.transform.name == "thigh(leftx)" || hit.transform.name == "leg(right)"
                    || hit.transform.name == "leg(leftx)") this._guiShow = true;
            }
            else
            {
                this._guiShow = false;
            }
        }

        private void BodyFinder()
        {
            var ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            var cam = GameObject.Find("FPSCamera");
            RaycastHit hit;

            if (this.grabbedBody == false)
            {
                if (Physics.Raycast(ray2, out hit, 1.0f))
                    if (hit.transform.name == "head" || hit.transform.name == "shoulder(right)"
                        || hit.transform.name == "arm(right)" || hit.transform.name == "shoulders(xxxxx)"
                        || hit.transform.name == "shoulder(leftx)" || hit.transform.name == "RagDoll"
                        || hit.transform.name == "arm(leftx)" || hit.transform.name == "thigh(right)"
                        || hit.transform.name == "thigh(leftx)" || hit.transform.name == "leg(right)"
                        || hit.transform.name == "leg(leftx)")
                    {
                        this.grabbedBody = true;
                        this.saveBody = hit.transform.gameObject;
                        var rb = this.saveBody.GetComponent<Rigidbody>();
                        rb.isKinematic = true;
                        rb.useGravity = false;
                        this.saveBody.transform.parent = cam.transform;
                    }
            }
            else if (this.grabbedBody)
            {
                var rb = this.saveBody.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                this.grabbedBody = false;
                this.saveBody.transform.parent = null;
            }
        }

        private void rotateBody()
        {
            if (this.grabbedBody)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f) this.saveBody.transform.Rotate(Vector3.forward * 5);

                if (Input.GetAxis("Mouse ScrollWheel") < 0f) this.saveBody.transform.Rotate(Vector3.back * 5);
            }
        }

        private void ThrowBody()
        {
            var player = GameObject.Find("PLAYER");
            var rb = this.saveBody.GetComponent<Rigidbody>();
            if (this.grabbedBody)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                this.grabbedBody = false;
                this.saveBody.transform.parent = null;
                rb.AddForce(player.transform.forward * 5000);
            }
        }
    }


}