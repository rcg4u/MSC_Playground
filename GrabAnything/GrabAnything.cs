using System;
using System.IO;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using MSCLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GrabAnything
{
    public class GrabAnything : Mod
    {
        private bool _guiShow;

        private bool _grabbedObject;

        private readonly Rect _guiBox = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 120, 400, 320);

        private readonly Rect _throwStrengthLabel = new Rect(102.5f, 25, 195, 20);

        private readonly Rect _guiKeySlider = new Rect(5, 50, 390, 30);

        // Keybinds
        private readonly Keybind _pickKey = new Keybind("PickObject", "PickKey", KeyCode.Mouse0, KeyCode.None);

        private Ray _ray;

        private GameObject _saveObject;

        private string _hitName;

        private readonly Keybind _throwKey = new Keybind("ThrowObject", "ThrowKey", KeyCode.Mouse1, KeyCode.None);

        private readonly Keybind _guiGrabKey = new Keybind("GrabGui", "GrabGuiKey", KeyCode.P, KeyCode.LeftControl);
        private readonly Keybind _guiKeyToggler = new Keybind("ToggleKey", "ToggleKey", KeyCode.P, KeyCode.LeftAlt);
        private readonly Keybind _lockKey = new Keybind("Lockkey", "Lockkey", KeyCode.Mouse4, KeyCode.None);


        private readonly Keybind _rotateKey =
            new Keybind("RotatorChange", "RotateChangeKey", KeyCode.Mouse2, KeyCode.None);

        private bool _guiEnabled;

        private float _throwStrengt = 2f;

        private bool _grabEnabled;

        private string _grabEnabledString = "Disabled";

        private bool _rotateAxis;

        private string _rotationDirection = "Direction 1";
        private bool _loaded;
        internal GameObject _fpsCamera;
        private FsmBool _playerInMenu;
        private GameObject _optionsMenu;
        private GameObject lookTargetFleetari;
        private GameObject lookTargetTeimo;
        public int _colour = 1;
        internal string _colourString;
        internal string _colorformat;
        private readonly CreateItem _createItem;
        private GameObject _player;

        public GrabAnything()
        {
            _createItem = new CreateItem(this);
        }


        // The name of the author
        public override string Author => "haverdaden";

        // The ID of the mod - Should be unique
        public override string ID => "GrabAnything";

        // The name of the mod that is displayed
        public override string Name => "GrabAnything";

        // The version of the mod
        public override string Version => "1.3.1";

        // Called to draw the GUI
        public override void OnGUI()
        {
            var myStyle = new GUIStyle
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                normal = {textColor = Color.yellow}
            };

            GUI.backgroundColor = Color.black;
            GUI.skin.window.fontSize = 12;


            if (_guiShow && _grabEnabled)
                GUI.Label(
                    new Rect(Screen.width / 2 - 90, Screen.height - Screen.height / 8 - 20, 150, 20),
                    "Grab " + _hitName,
                    myStyle);

            if (_guiEnabled)
                GUI.ModalWindow(123, _guiBox, GuiGrabWindow, "Grab Settings | GrabAnything mod by haverdaden (DD)");
        }

        private void GuiGrabWindow(int id)
        {
            var LabelStyle = new GUIStyle();
            LabelStyle.normal.textColor = Color.white;
            LabelStyle.alignment = TextAnchor.UpperCenter;

            //ThrowStrength
            GUI.Label(_throwStrengthLabel,
                "<size=16>Throw Strength: " + "<b>" + ((int) _throwStrengt - 1 + "</b></size>"), LabelStyle);
            _throwStrengt = GUI.HorizontalSlider(_guiKeySlider, _throwStrengt, 2f, 201f);

            //Rotation Direction
            GUI.Label(new Rect(100, 70, 200, 30), "Rotation Direction", LabelStyle);
            if (GUI.Button(new Rect(100, 90, 200, 30), _rotationDirection))
                if (_rotateAxis == false)
                    _rotateAxis = true;
                else
                    _rotateAxis = false;

            if (_rotateAxis)
                _rotationDirection = "Direction 2";
            else
                _rotationDirection = "Direction 1";


            //Toggle On/Off
            GUI.Label(new Rect(100, 125, 200, 30),
                "Toggle MOD ON/OFF", LabelStyle);
            if (GUI.Button(new Rect(100, 145, 200, 30), _grabEnabledString))
                if (_grabEnabled)
                {
                    _grabEnabled = false;
                    _grabEnabledString = "Disabled";
                }
                else
                {
                    _grabEnabled = true;
                    _grabEnabledString = "Enabled";
                }

            GUI.Label(new Rect(102.5f, 185, 195, 30),
                "<size=16>Color: " + "<color=" + _colorformat + "<b>" + _colourString + "</b></color></size>",
                LabelStyle);
            _colour = (int) GUI.HorizontalSlider(new Rect(20, 210, 360, 20), _colour, 1, 10);

            _createItem.CheckColorName();

            if (GUI.Button(new Rect(40, 235, 60, 30), "<size=12>Box</size>"))
                _createItem.CreatePrimitive(PrimitiveType.Cube, "cube");
            if (GUI.Button(new Rect(105, 235, 60, 30), "<size=12>Board</size>"))
                _createItem.CreatePrimitive(PrimitiveType.Cube, "board");
            if (GUI.Button(new Rect(170, 235, 60, 30), "<size=12>Cylinder</size>"))
                _createItem.CreatePrimitive(PrimitiveType.Cylinder, "cylinder");
            if (GUI.Button(new Rect(235, 235, 60, 30), "<size=12>Ball</size>"))
                _createItem.CreatePrimitive(PrimitiveType.Sphere, "¨sphere");
            if (GUI.Button(new Rect(300, 235, 60, 30), "<size=12>Capsule</size>"))
                _createItem.CreatePrimitive(PrimitiveType.Capsule, "capsule");


            //Close and Default
            if (GUI.Button(new Rect(120, 275, 80, 30), "Close"))
            {
                _optionsMenu.SetActive(false);
                _playerInMenu.Value = false;
                _guiEnabled = false;
            }

            if (GUI.Button(new Rect(200, 275, 80, 30), "Default"))
            {
                _throwStrengt = 2f;
                _rotateAxis = false;
                _colour = 1;
                _saveObject.transform.parent = null;
                _grabbedObject = false;
            }
        }

        public override void OnLoad()
        {
            // Do your initialization here
            Keybind.Add(this, _pickKey);
            Keybind.Add(this, _throwKey);
            Keybind.Add(this, _guiGrabKey);
            Keybind.Add(this, _rotateKey);
            Keybind.Add(this, _guiKeyToggler);
            Keybind.Add(this, _lockKey);

            ModConsole.Print("<color=lime>GrabAnything by haverdaden (DD) has been loaded!</color>");
        }

        public override void Update()
        {
            if (Application.loadedLevelName == "GAME")
            {
                if (!_loaded)
                    if (GameObject.Find("FPSCamera"))
                    {
                        _fpsCamera = GameObject.Find("FPSCamera");
                        _player = GameObject.Find("PLAYER");
                        _playerInMenu = PlayMakerGlobals.Instance.Variables.GetFsmBool("PlayerInMenu");
                        foreach (var resource in Resources.FindObjectsOfTypeAll<GameObject>())
                        {
                            if (resource.name == "OptionsMenu")
                                _optionsMenu = resource;

                            if (resource.name == "SlotMachine" || resource.name == "SlotMachine 1")
                            {
                                var meshcol = resource.AddComponent<MeshCollider>();
                                meshcol.convex = true;
                                meshcol.isTrigger = true;
                                foreach (var VARIABLE in resource.GetComponentsInChildren<Transform>())
                                    if (VARIABLE.name == "slot_machine 1")
                                    {
                                        meshcol.sharedMesh = null;
                                        meshcol.sharedMesh = VARIABLE.GetComponent<MeshFilter>().mesh;
                                    }
                            }
                            if (resource.name == "ShopCashRegister")
                            {
                                var meshcol = resource.AddComponent<BoxCollider>();
                                meshcol.size = meshcol.size * 0.5f;
                            }
                            if (resource.name == "Teimo")
                            {
                                var box = resource.AddComponent<BoxCollider>();
                                box.size = new Vector3(0.5f, 1.6f, 0.5f);
                            }
                            if (resource.name == "Neighbour 2")
                            {
                                var box = resource.AddComponent<BoxCollider>();
                                box.size = new Vector3(0.2f, 1f, 0.8f);
                                box.center = new Vector3(0.0f, 0.4f, 0);
                            }
                            if (resource.name == "LookTarget" && resource.transform.root.name == "REPAIRSHOP")
                                lookTargetFleetari = resource;
                            if (resource.name == "LookTarget" && resource.transform.root.name == "STORE")
                                lookTargetTeimo = resource;
                        }

                        _loaded = true;
                    }

                if (_guiGrabKey.IsDown()) ShowGui();

                if (_guiKeyToggler.IsDown())
                    if (_grabEnabled)
                    {
                        if (_grabbedObject)
                            ProceedDrop();

                        _grabEnabledString = "Disabled";
                        _grabEnabled = false;
                    }
                    else
                    {
                        _grabEnabled = true;
                        _grabEnabledString = "Enabled";
                    }

                if (_grabEnabled)
                {
                    // Do your updating here
                    if (_pickKey.IsDown()) ObjectFinder();

                    if (_throwKey.IsDown()) ThrowObject();

                    RotateObject();

                    ShowItemToGrabOnHit();

                    ChangeRotation();
                }

                if (_lockKey.IsDown())
                    LockObject();
            }
            if (Application.loadedLevelName != "GAME" && _loaded)
            {
                _grabEnabled = false;
                _grabEnabledString = "Disabled";
                _grabbedObject = false;
                _guiEnabled = false;
                _playerInMenu.Value = false;
                _loaded = false;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {

                    _player.transform.parent = _saveObject.transform;
                _saveObject.transform.parent = null;
                _grabbedObject = false;

            }
        }

        private void ChangeRotation()
        {
            if (_rotateKey.IsDown())
                if (_rotateAxis)
                    _rotateAxis = false;
                else
                    _rotateAxis = true;
        }

        private void ShowGui()
        {
            if (_guiEnabled)
            {
                _playerInMenu.Value = false;
                _optionsMenu.SetActive(false);
                _guiEnabled = false;
                _guiShow = false;
            }
            else
            {
                _playerInMenu.Value = true;
                _guiEnabled = true;
            }
        }

        private void ShowItemToGrabOnHit()
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10.0f) && _grabbedObject == false)
                if (hit.rigidbody)
                {
                    _hitName = hit.rigidbody.name;
                    _guiShow = true;
                }
                else if (MoveableItems.MovableItems.Contains(hit.collider.name))
                {
                    _hitName = hit.collider.gameObject.name;
                    _guiShow = true;
                }
                else
                {
                    _guiShow = false;
                }
            else _guiShow = false;
        }

        private void ObjectFinder()
        {
            if (_grabbedObject == false)

            {
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(_ray, out hit, 10.0f))
                    if (hit.transform.name != "PLAYER" && hit.transform.name != "FPSCamera")
                        ProceedPick(hit);
            }
            else if (_grabbedObject && _saveObject != null)
            {
                ProceedDrop();
            }
        }

        private void ProceedPick(RaycastHit hit)
        {
            if (hit.rigidbody)
            {
                _saveObject = hit.transform.gameObject;
                var rb = _saveObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
                _saveObject.transform.parent = _fpsCamera.transform;
                _grabbedObject = true;
            }
            else if (MoveableItems.MovableItems.Contains(hit.collider.name))
            {
                if (hit.transform.name == "Teimo")
                    Object.Destroy(lookTargetTeimo);
                else if (hit.transform.name == "Neighbour 2")
                    Object.Destroy(lookTargetFleetari);
                _saveObject = hit.transform.gameObject;
                var rb = _saveObject.AddComponent<Rigidbody>();
                if (_saveObject.GetComponent<MeshCollider>())
                    _saveObject.GetComponent<MeshCollider>().convex = true;
                rb.isKinematic = true;
                rb.useGravity = false;
                _saveObject.transform.parent = _fpsCamera.transform;
                _grabbedObject = true;
            }
        }

        private void ProceedDrop()
        {
            var rb = _saveObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            _saveObject.transform.parent = null;
            _grabbedObject = false;
        }

        private void RotateObject()
        {
            if (_grabbedObject && _rotateAxis == false)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                    _saveObject.transform.Rotate(_fpsCamera.transform.right * 5, Space.World);

                if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                    _saveObject.transform.Rotate(_fpsCamera.transform.right * -5, Space.World);
            }
            else if (_grabbedObject && _rotateAxis)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                    _saveObject.transform.Rotate(_fpsCamera.transform.up * 5, Space.World);

                if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                    _saveObject.transform.Rotate(_fpsCamera.transform.up * -5, Space.World);
            }
        }

        private void LockObject()
        {
            if (_grabbedObject && _saveObject != null)
            {

                                _saveObject.transform.parent = null;
                                _grabbedObject = false;

            }
        }

        private void ThrowObject()
        {
            if (_grabbedObject && _saveObject != null)
            {
                var rb = _saveObject.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                _saveObject.transform.parent = null;
                rb.AddForce(_fpsCamera.transform.forward * _throwStrengt, ForceMode.VelocityChange);
                _grabbedObject = false;
                _saveObject = null;
            }
        }
    }
}