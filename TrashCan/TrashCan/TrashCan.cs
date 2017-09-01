using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TrashCan
{
    public class TrashCan : Mod
    {
        private GameObject cube1;
        private bool notcreated;
        private GameObject monoGO;
        private TrashcanMono mono;
        private bool trashEmptied;
        private GameObject saving;
        private FsmState state;
        private List<FsmState> stateList = new List<FsmState>();
        public override string ID { get { return "TrashCan"; } }
        public override string Name { get { return "TrashCan"; } }
        public override string Author { get { return "haverdaden"; } }
        public override string Version { get { return "1.0"; } }

        //Called when mod is loading
        public override void OnLoad()
        {

        }

        // Update is called once per frame
        public override void Update()
        {
            TrashCreator();
            mono.StartCoroutine(TrashTeleporter());

            if (Application.loadedLevelName != "GAME")
            {              
                notcreated = false;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                foreach (var fsm in saving.transform.parent.GetComponentsInChildren<PlayMakerFSM>())
                {
                    foreach (var VARIABLE in fsm.FsmVariables.GetAllNamedVariables())
                    {
                        ModConsole.Print(VARIABLE.Name);
                    }
                }
            }

            if (stateList != null)
            {
                foreach (var VARIABLE in stateList)
                {
                    if (VARIABLE.Active)
                    {
                        ModConsole.Print("SAVING");
                    }
                }
            }


        }

        private void TrashCreator()
        {
            if (Application.loadedLevelName == "GAME" && !notcreated)
            {
                monoGO = new GameObject();
                mono = monoGO.AddComponent<TrashcanMono>();

                foreach (PlayMakerFSM fsm in Resources.FindObjectsOfTypeAll(typeof(PlayMakerFSM)))
                {
                    foreach (var state in fsm.FsmStates)
                    {
                        if (state.Name == "Save")
                        {
                            ModConsole.Print("SAVE ALL");
                            stateList.Add(state);
                        }
                    }
                        

                    
                }

                cube1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cube1.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                cube1.transform.position = GameObject.Find("Trashcan").transform.position + Vector3.down * 0.3f;

                var box = cube1.GetComponent<SphereCollider>();
                var render = cube1.GetComponent<MeshRenderer>();

                render.enabled = false;
                box.isTrigger = true;
                notcreated = true;
            }
        }

        private IEnumerator TrashTeleporter()
        {
            if (cube1 && notcreated && !trashEmptied)
            {
                trashEmptied = true;
                Collider[] hitColliders = Physics.OverlapSphere(cube1.transform.position, cube1.transform.localScale.x);

                foreach (var VARIABLE in hitColliders)
                {
                    if (VARIABLE.GetComponent<Rigidbody>())
                    {
                        
                        ModConsole.Print(string.Format("<color=lime><b>TRASHCAN: </b></color><color=#6495ED><b>" + VARIABLE.name.ToUpper() + " </b></color><color=orange><b>SENT TO LANDFILL!</b></color>"));
                    //    VARIABLE.transform.position = new Vector3(0,-50000,0);

                        foreach (var fsm in GameObject.Find("PLAYER").GetComponentsInChildren<PlayMakerFSM>())
                        {
                            foreach (var evento in fsm.FsmEvents)
                            {
                                if (evento.Name == "PROCEED Drop")
                                {
                                    fsm.SendEvent(evento.Name);
                                }
                            }
                        }

                        foreach (var vector3 in PlayMakerGlobals.Instance.Variables.GameObjectVariables)
                        {
                            ModConsole.Print(vector3.Name + " | " + vector3.Value);
                        }
                        

                        Object.Destroy(VARIABLE.gameObject);

                       File.Delete(Application.persistentDataPath + "/items.txt");



                    }

                }

                yield return new WaitForSeconds(10);

                trashEmptied = false;
            }
        }

    }
}
