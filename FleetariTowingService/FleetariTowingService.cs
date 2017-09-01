

using System.Collections;
using System.IO;
using Boo.Lang;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace FleetariTowingService
{
    public class FleetariTowingService : Mod
    {
        //Mod Info
        public override string ID => "FleetariTowingService";

        public override string Name => "FleetariTowingService";
        public override string Author => "haverdaden";
        public override string Version => "0.9.1 alpha";

        //Objects
        internal Texture2D HandUseTexture;

        internal GameObject FleetariMonoGo;
        internal FleetariTowingMono Mono;
        internal CharacterJoint OriginalJoint;
        internal GameObject Player;
        internal GameObject TowingPaper;



        //Bool
        internal bool DeliveryInProgress;

        internal bool CheckBoxSatsumaExtraEnabled;
        internal bool NearByItemsSelected;
        internal bool ShowTowInProgressLabel;
        internal bool ShowNoMoneyLabel;
        internal bool ShowTowMenu;
        internal bool ShowMouse;
        internal bool ShowGui;
        internal bool BrouchureFound;
        internal bool GetGameoobjects;
        internal bool FindPlayer;
        internal bool NewStart;


        //Float
        internal const float TpHeight = 0.1f;
        internal const float ButtonHeight = 60;
        internal const float ButtonWidth = 500;



        //String
        internal const string TowingLanguagesString = "Hinauta\nBogsera\nTow";

        internal const string TowingLanguagesFerndaleString =
            "Hinauta<size=12>\t(Hinaan Ferndaleni takaisin.)</size>\n" +
            "Bogsera<size=12>\t(Jag tar tillbaka min Ferndale.)</size>\n" +
            "Tow<size=12>\t(I'll take my Ferndale back.)</size>";

        internal string TowDestination = "your home.";
        internal object TowDestinationSwedish = "ditt hem.";
        internal object TowDestinationFinnish = "kotiin.";

        //CHECKBOXES
        internal string CheckBoxSatsuma = "☐";

        internal string CheckBoxFerndale = "☐";
        internal string CheckBoxVan = "☐";
        internal string CheckBoxTruck = "☐";
        internal string CheckBoxTractor = "☐";
        internal string CheckBoxFlatbed = "☐";
        internal string CheckBoxMoped = "☐";
        internal string CheckBoxSatsumaExtra = "☐";
        internal string CheckBoxItems = "☐";
        internal readonly GuiClass GuiClass;
        internal int Total;


        public FleetariTowingService()
        {
            GuiClass = new GuiClass(this);
        }

        //To show the paper and GUI
        public override void OnGUI()
        {
            //Gui styling
            GUIStyle textStyle, serviceStyle, noMoneyStyle, modalStyle;
            GuiClass.GuiStyling(out textStyle, out serviceStyle, out noMoneyStyle, out modalStyle);

            //Show hand texture
            GuiClass.ShowUseGui(textStyle, serviceStyle);

            //Show the tow paper
            GuiClass.ShowPaper(modalStyle);

            //Show not enough money
            GuiClass.ShowNotEnoughMoney(noMoneyStyle);

            //Show tow in progress
            GuiClass.ShowTowInProgress(noMoneyStyle);
        }

        //Called when mod is loading
        public override void OnLoad()
        {
        }

        // Update is called once per frame
        public override void Update()
        {
            if (Application.loadedLevel == 3)
            {
                if (!NewStart)
                {
                    FleetariMonoGo = new GameObject();
                    Mono = FleetariMonoGo.AddComponent<FleetariTowingMono>();
                    Mono.StartCoroutine(GetGameobjects());
                    NewStart = true;
                }

                RayCastPlayer();
                CheckPlayerBrochureDistance();
            }

            if (Application.loadedLevel != 3)
                ResetMod();

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                var writer = new StreamWriter("REPAIRSHOPEVENTS.txt");

                writer.Close();
            }
        }

        //Check What kind of vehicle on paper
        internal void CheckSelectedVehicle()
        {
            if (CheckBoxSatsuma == "☑" && CheckBoxSatsumaExtra == "☑")
                Mono.StartCoroutine(CheckMoney("SATSUMA(557kg)", new Vector3(1551, 5, 727), 5000));
            if (CheckBoxSatsuma == "☑" && CheckBoxSatsumaExtra == "☐")
                Mono.StartCoroutine(CheckMoney("SATSUMA(557kg)", new Vector3(6, TpHeight, 30), 3000));

            if (CheckBoxFerndale == "☑")
                Mono.StartCoroutine(CheckMoney("FERNDALE(1630kg)", new Vector3(1554, 5, 724), 4000));
            if (CheckBoxVan == "☑")
                Mono.StartCoroutine(CheckMoney("HAYOSIKO(1500kg)", new Vector3(3, TpHeight, 30), 5000));
            if (CheckBoxTruck == "☑")
                Mono.StartCoroutine(CheckMoney("GIFU(750/450psi)", new Vector3(0, TpHeight, 30), 10000));
            if (CheckBoxTractor == "☑")
                Mono.StartCoroutine(CheckMoney("KEKMET(350-400psi)", new Vector3(-3, TpHeight, 30), 6000));
            if (CheckBoxFlatbed == "☑")
                Mono.StartCoroutine(CheckMoney("FLATBED", new Vector3(-6, TpHeight, 30), 2000));
            if (CheckBoxMoped == "☑")
                Mono.StartCoroutine(CheckMoney("JONNEZ ES(Clone)", new Vector3(-9, TpHeight, 30), 2000));
        }

        //Check Money before proceed.
        internal IEnumerator CheckMoney(string vehicle, Vector3 towPosition, int price)
        {
            var towWaitTime = 600;

            if (NearByItemsSelected)
                price += 500;


            HidePaper();

            var money = FsmVariables.GlobalVariables.FindFsmFloat("PlayerMoney").Value;


            if (money >= price)
            {
                if (!DeliveryInProgress)
                {
                    FsmVariables.GlobalVariables.FindFsmFloat("PlayerMoney").Value = money - price;
                    DeliveryInProgress = true;



                    yield return new WaitForSeconds(towWaitTime);

                    var playerSleeping = PlayMakerGlobals.Instance.Variables.GetFsmBool("PlayerSleeps");

                    while (playerSleeping.Value == false)
                        yield return null;

                    CheckVehicle(vehicle, towPosition);
                }

                else
                {
                    ShowTowInProgressLabel = true;
                    yield return new WaitForSeconds(2);

                    ShowTowInProgressLabel = false;
                }
            }
            else
            {
                HidePaper();
                Mono.StartCoroutine(NotEnoughMoney());
            }
        }

        //Display Not Enough Money
        internal IEnumerator NotEnoughMoney()
        {
            ShowNoMoneyLabel = true;

            yield return new WaitForSeconds(2);

            ShowNoMoneyLabel = false;
        }

        //Check what kind of vehicle was selected
        internal void CheckVehicle(string vehicleToTow, Vector3 towPosition)
        {
            var vehicle = GameObject.Find(vehicleToTow);
            if (vehicleToTow == "GIFU(750/450psi)")
                Mono.StartCoroutine(TruckFix(towPosition, vehicle));
            else if (vehicleToTow == "FLATBED")
                FlatBedTow(vehicleToTow, towPosition, vehicle);
            else
                Mono.StartCoroutine(NormalTow(towPosition, vehicle));
        }

        private IEnumerator TruckFix(Vector3 towPosition, GameObject vehicle)
        {
            vehicle.GetComponent<PlayMakerFSM>().FsmVariables.GetFsmBool("ParkingOn").Value = false;
            yield return new WaitForSeconds(2);
            vehicle.transform.Translate(Vector3.forward * -0.02f);
            Mono.StartCoroutine(NormalTow(towPosition, vehicle));
        }

        //Special Flatbed teleport
        internal void FlatBedTow(string vehicleToTow, Vector3 towPosition, GameObject vehicle)
        {
            OriginalJoint = GameObject.Find(vehicleToTow).GetComponent<CharacterJoint>();
            Object.Destroy(OriginalJoint);
            Mono.StartCoroutine(NormalTow(towPosition, vehicle));
        }

        //Normal teleport of vehicles
        internal IEnumerator NormalTow(Vector3 towPosition, GameObject vehicle)
        {
            yield return new WaitForSeconds(1);

            vehicle.transform.Translate(Vector3.forward * 0.02f);

            yield return new WaitForSeconds(3);

            //Deliver Items in car
            if (NearByItemsSelected)
                ItemDelivery(towPosition, vehicle);

            var vehicleRb = vehicle.GetComponent<Rigidbody>();
            vehicleRb.isKinematic = true;
            vehicleRb.useGravity = false;
            vehicleRb.detectCollisions = false;

            vehicle.transform.position = towPosition;

            vehicle.transform.rotation = Quaternion.Euler(0, 180, 0);


            //Check for collision on towposition
            CollisionDetector(towPosition, vehicle);

            yield return new WaitForSeconds(1);


            vehicleRb.useGravity = true;
            vehicleRb.isKinematic = false;
            vehicleRb.detectCollisions = true;
            NearByItemsSelected = false;
            DeliveryInProgress = false;

            yield return new WaitForSeconds(3);

            if (vehicle.name == "FLATBED")
                FlatBedFix();
        }

        //Delivery of items
        internal void ItemDelivery(Vector3 towPosition, GameObject vehicle)
        {
            var hitColliders = Physics.OverlapSphere(vehicle.transform.position, 5);
            var satsumaTransforms = new List<string>();
            var itemheight = 0;

            foreach (var satstrans in GameObject.Find("SATSUMA(557kg)").GetComponentsInChildren<Transform>())
                satsumaTransforms.Add(satstrans.name);

            foreach (var closebyItem in hitColliders)
                if (closebyItem.GetComponent<Rigidbody>())
                    if (!ItemTowingClass.Itemlist.Contains(closebyItem.name) &&
                        !satsumaTransforms.Contains(closebyItem.name))
                    {
                        closebyItem.gameObject.transform.position = towPosition - new Vector3(0, itemheight, 5);
                        itemheight--;
                    }
        }

        //Detection of towingcollision
        internal void CollisionDetector(Vector3 towPosition, GameObject vehicle)
        {
            var towheight = 3;
            var colliding = true;

            while (colliding)
                foreach (var vehicleUnBlock in ItemTowingClass.VehicleList)
                    if (vehicleUnBlock != vehicle.name)
                        if (Vector3.Distance(vehicle.transform.position,
                                GameObject.Find(vehicleUnBlock).transform.position) < 4)
                        {
                            vehicle.transform.position = towPosition + new Vector3(0, towheight, 0) +
                                                         vehicle.transform.forward * 7;

                            towheight++;
                        }
                        else
                        {
                            colliding = false;
                        }
        }

        //Fixes FlatBed for teleporting
        internal void FlatBedFix()
        {
            //Readd Flatbed Joint
            var test2 = GameObject.Find("FLATBED").AddComponent<CharacterJoint>();

            test2.enableProjection = OriginalJoint.enableProjection;
            test2.highTwistLimit = OriginalJoint.highTwistLimit;
            test2.lowTwistLimit = OriginalJoint.lowTwistLimit;
            test2.projectionAngle = OriginalJoint.projectionAngle;
            test2.projectionDistance = OriginalJoint.projectionDistance;
            test2.swing1Limit = OriginalJoint.swing1Limit;
            test2.swing2Limit = OriginalJoint.swing2Limit;
            test2.swingAxis = OriginalJoint.swingAxis;
            test2.swingLimitSpring = OriginalJoint.swingLimitSpring;
            test2.twistLimitSpring = OriginalJoint.twistLimitSpring;
            test2.anchor = OriginalJoint.anchor;
            test2.autoConfigureConnectedAnchor = OriginalJoint.autoConfigureConnectedAnchor;
            test2.axis = OriginalJoint.axis;
            test2.breakForce = OriginalJoint.breakForce;
            test2.breakTorque = OriginalJoint.breakTorque;
            test2.connectedAnchor = OriginalJoint.connectedAnchor;
            test2.connectedBody = OriginalJoint.connectedBody;
            test2.enableCollision = OriginalJoint.enableCollision;
            test2.enablePreprocessing = OriginalJoint.enablePreprocessing;
        }

        //Hide paper on far distance
        internal void CheckPlayerBrochureDistance()
        {
            if (ShowTowMenu && GetGameoobjects)
                if (Vector3.Distance(Player.transform.position, TowingPaper.transform.position) > 3 && ShowTowMenu)
                    HidePaper();
        }

        //Raycast check for paper
        internal void RayCastPlayer()
        {
            if (BrouchureFound)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 1f))
                    if (raycastHit.collider.name == "TowingService")
                    {
                        ShowGui = true;

                        if (Input.GetMouseButtonDown(0) && ShowGui)
                        {
                            ShowTowMenu = true;
                            UnCheckAll();
                        }
                    }
                    else
                    {
                        ShowGui = false;
                    }
                else
                    ShowGui = false;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                HidePaper();
        }

        //Get gameobjects
        internal IEnumerator GetGameobjects()
        {
            while (!GetGameoobjects)
            {
                if (GameObject.Find("PLAYER"))
                {


                    if (!FindPlayer)
                        if (GameObject.Find("PLAYER"))
                        {
                            Player = GameObject.Find("PLAYER");
                            FindPlayer = true;
                        }
                    if (!BrouchureFound)
                    {
                        //Get handtexture
                        foreach (var texture in Resources.FindObjectsOfTypeAll<Texture2D>())
                            if (texture.name == "gui_uset")
                                HandUseTexture = texture;

                        foreach (var transform in Resources.FindObjectsOfTypeAll<Transform>())
                            if (transform.name == "Brochure")
                            {

                                //Copy paper
                                TowingPaper = Object.Instantiate(transform.gameObject);

                                //Paper name, position, rotation and remove 
                                TowingPaper.name = "TowingService";
                                TowingPaper.transform.position = new Vector3(1555.0f, 5.48f, 737.9f);
                                TowingPaper.transform.localEulerAngles = new Vector3(0, 270, 0);
                                Object.DestroyImmediate(TowingPaper.GetComponent<PlayMakerFSM>());
                            }


                        BrouchureFound = true;
                    }
                    if (FindPlayer && BrouchureFound)
                    {
                        ModConsole.Print(
                            "<color=lime><b>[Alpha TowingService]</b></color> <color=orange><b>Please report all bugs to RD or haverdaden</b></color>");
                        GetGameoobjects = true;
                    }

                }
                yield return new WaitForSeconds(2);
            }
        }

        //Reset mod in menu
        internal void ResetMod()
        {
            //Objects
            HandUseTexture = null;
            Mono = null;
            OriginalJoint = null;
            Player = null;
            TowingPaper = null;

            //Bool
            DeliveryInProgress = false;
            CheckBoxSatsumaExtraEnabled = false;
            ShowTowInProgressLabel = false;
            ShowNoMoneyLabel = false;
            ShowTowMenu = false;
            ShowMouse = false;
            ShowGui = false;
            BrouchureFound = false;
            GetGameoobjects = false;
            FindPlayer = false;
            NewStart = false;
        }

        //Hide the paper
        internal void HidePaper()
        {
            ShowTowMenu = false;
            ShowMouse = false;
            PlayMakerGlobals.Instance.Variables.GetFsmBool("PlayerInMenu").Value = false;
        }

        //Unmark all on paper
        internal void UnCheckAll()
        {
            CheckBoxSatsuma = "☐";
            CheckBoxFerndale = "☐";
            CheckBoxVan = "☐";
            CheckBoxTruck = "☐";
            CheckBoxTractor = "☐";
            CheckBoxFlatbed = "☐";
            CheckBoxMoped = "☐";
            CheckBoxSatsumaExtra = "☐";
            CheckBoxItems = "☐";
            TowDestination = "your home.";
            TowDestinationSwedish = "ditt hem.";
            TowDestinationFinnish = "kotiin.";
            CheckBoxSatsumaExtraEnabled = false;
            NearByItemsSelected = false;
            Total = 0;
        }
    }
}
