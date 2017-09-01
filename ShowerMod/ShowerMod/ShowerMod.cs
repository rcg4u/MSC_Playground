using MSCLoader;
using UnityEngine;

namespace ShowerMod
{
    public class ShowerMod : Mod
    {
        private bool _loaded;
        private bool _waterParticlesFound;
        private bool _useShowerTexture;
        internal static bool ToggleShower;
        private GameObject _showerHandleTrigger;
        private GameObject _showerTrigger;
        private GameObject _showerTap;
        private GameObject _showerParticles;
        private Texture2D _handUseTexture;
        private ParticleRenderer _waterTapRenderer;
        private PlayMakerFSM _waterTapPlaymaker;

        public override string ID { get { return "ShowerMod"; } }
        public override string Name { get { return "ShowerMod"; } }
        public override string Author { get { return "haverdaden"; } }
        public override string Version { get { return "1.0"; } }

        //GUI
        public override void OnGUI()
        {
            UseShowerGui();
        }

        //Show "use" shower
        private void UseShowerGui()
        {
            var showertextStyle = GUI.skin.GetStyle("label");
            showertextStyle.alignment = TextAnchor.MiddleCenter;

            if (_useShowerTexture)
            {
                GUI.Label(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 25, 50, 50),
                    _handUseTexture, showertextStyle);
            }
        }

        // Update is called once per frame
        public override void Update()
        {
            //Checkins and creates shower
            ShowerModCreator();

            //Runs the shower methods.
            ShowerExecuter();

        }

        //Runs shower
        private void ShowerExecuter()
        {
            if (_loaded)
            {
                //Raycast
                RayCastTriggers();
                //Disable shower when water is turned off.
                WaterOff();
            }
        }

        //Raycast
        private void RayCastTriggers()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1))
            {
                if (hit.collider.name == "showerHandle")
                {
                    _useShowerTexture = true;

                    if (Input.GetMouseButtonDown(0) && _useShowerTexture)
                    {
                        if (_waterTapRenderer.enabled)
                        {

                            _showerParticles.SetActive(true);
                            ToggleShower = !ToggleShower;
                            _waterTapRenderer.enabled = false;
                            _waterTapPlaymaker.enabled = false;
                        }
                        else if (!_waterTapRenderer.enabled && ToggleShower)
                        {
                            ToggleShower = !ToggleShower;
                            _showerParticles.SetActive(false);
                            _waterTapRenderer.enabled = true;
                            _waterTapPlaymaker.enabled = true;

                        }

                    }
                }
                else
                {
                    _useShowerTexture = false;
                }

            }
            else
            {
                _useShowerTexture = false;
            }
        }

        //Game checker
        private void ShowerModCreator()
        {
            if (Application.loadedLevelName == "GAME" && !_loaded)
            {
                CreateShower();

                GetUseTexture();

                CreateTriggers();

                _loaded = true;
            }
            else if (Application.loadedLevelName != "GAME" && _loaded)
            {

                _waterParticlesFound = false;
                _loaded = false;

            }
        }

        private void CreateShower()
        {
            ShowerTrigger.dirtiness = PlayMakerGlobals.Instance.Variables.FindFsmFloat("PlayerDirtiness");

            _waterParticlesFound = false;
            foreach (var resource in Resources.FindObjectsOfTypeAll<Transform>())
            {
                if (resource.name == "Particle" && !_waterParticlesFound &&
                    resource.transform.parent.name == "Shower")
                {
                    //save showertap
                    _showerTap = resource.gameObject;
                    _showerParticles = Object.Instantiate(resource.gameObject);


                    //Remove unneeded stuff
                    foreach (var component in _showerParticles.gameObject.GetComponents<Component>())
                    {
                        if (component is PlayMakerFSM)
                            Object.Destroy(component);
                        if (component is AudioSource)
                            Object.Destroy(component);
                    }

                    //Get all needed stuff
                    _showerParticles.name = "ShowerWater";
                    _showerParticles.gameObject.SetActive(false);
                    _waterTapRenderer = resource.GetComponent<ParticleRenderer>();
                    _waterTapPlaymaker = resource.GetComponent<PlayMakerFSM>();

                    var particleEmitter = _showerParticles.GetComponent<EllipsoidParticleEmitter>();
                    var particleAnimator = _showerParticles.GetComponent<ParticleAnimator>();
                    var particleRenderer = _showerParticles.GetComponent<ParticleRenderer>();

                    //Physical and graphical changes for particles
                    particleAnimator.damping = 1;
                    particleAnimator.force = new Vector3(3, -10, 0);
                    particleAnimator.doesAnimateColor = true;
                    particleAnimator.autodestruct = true;

                    //Physical and graphical changes for particles
                    particleEmitter.rndVelocity = new Vector3(0.5f, 0.5f, 0);
                    particleEmitter.minEnergy = particleEmitter.minEnergy = 1f;
                    particleEmitter.maxEnergy = particleEmitter.maxEnergy = 1f;
                    particleEmitter.minEmission = particleEmitter.minEmission * 20;
                    particleEmitter.maxEmission = particleEmitter.maxEmission * 20;
                    particleEmitter.maxSize = particleEmitter.maxSize * 0.05f;
                    particleEmitter.minSize = particleEmitter.minSize * 0.05f;

                    //Create water texture
                    Texture2D blueTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    blueTexture.SetPixel(0, 0, new Color32(163, 210, 250, 100));
                    blueTexture.Apply();

                    //More visual changes for particles
                    particleRenderer.particleRenderMode = ParticleRenderMode.Stretch;
                    particleRenderer.lengthScale = 50;
                    particleRenderer.material.mainTexture = blueTexture;

                    //Shower Particle Position
                    _showerParticles.transform.position = new Vector3(-13.82f, 1.62f, 0.5f);
                    _waterParticlesFound = true;
                }
            }
        }

        //Get USE texture
        private void GetUseTexture()
        {
            foreach (var texture in Resources.FindObjectsOfTypeAll<Texture2D>())
            {
                if (texture.name == "gui_uset")
                {
                    _handUseTexture = texture;
                    break;
                }
            }
        }

        //Create Triggers
        private void CreateTriggers()
        {
            //Trigger Creation
            _showerHandleTrigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _showerTrigger = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _showerHandleTrigger.name = "showerHandle";
            _showerTrigger.name = "showerTrigger";

            //Trigger Scale
            _showerHandleTrigger.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

            //Trigger Position
            _showerHandleTrigger.transform.position = new Vector3(-13.80f, 0.4f, 0.62f);
            _showerTrigger.transform.position = new Vector3(-13.5f, -0.1f, 0.6f);

            //Trigger enable
            var col1 = _showerHandleTrigger.GetComponent<Collider>();
            var col2 = _showerTrigger.GetComponent<Collider>();
            col1.isTrigger = true;
            col2.isTrigger = true;

            //Trigger invisible
            _showerHandleTrigger.GetComponent<MeshRenderer>().enabled = false;
            _showerTrigger.GetComponent<MeshRenderer>().enabled = false;

            //Add ShowerTrigger Script
            col2.gameObject.AddComponent<ShowerTrigger>();
        }

        //Turn off shower
        private void WaterOff()
        {
            if (_showerTap.activeSelf == false && ToggleShower && _showerParticles.activeSelf && !_waterTapRenderer.enabled)
            {
                ToggleShower = false;
                _showerParticles.SetActive(false);
                _waterTapRenderer.enabled = true;
                _waterTapPlaymaker.enabled = true;
            }
        }

    }
}
