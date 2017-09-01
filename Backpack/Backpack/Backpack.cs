using System;
using System.Collections.Generic;
using MSCLoader;
using UnityEngine;

namespace Backpack
{
    public class Backpack : Mod
    {
        public override string ID
        {
            get { return "Backpack"; }
        }

        public override string Name
        {
            get { return "Backpack"; }
        }

        public override string Author
        {
            get { return "haverdaden"; }
        }

        public override string Version
        {
            get { return "1.0"; }
        }

        List<Transform> backpack = new List<Transform>();
        private bool found;
        private PlayMakerFSM _hand;
        private GameObject _player;

        //Called when mod is loading
        public override void OnLoad()
        {

        }

        // Update is called once per frame
        public override void Update()
        {
            if (!found)
            {
                _player = GameObject.Find("PLAYER");
                found = true;
            }

            RaycastHit raycastHit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 1f))
            {
                if (raycastHit.rigidbody && Input.GetKeyDown(KeyCode.X))
                {

                    raycastHit.rigidbody.isKinematic = true;
                    raycastHit.transform.position = new Vector3(0,100000,0);
          
                    PlayMakerFSM.BroadcastEvent("PROCEED Drop");
                    backpack.Add(raycastHit.transform);
           
                }


            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                foreach (var VARIABLE in backpack)
                {

                     //   VARIABLE.gameObject.SetActive(true);

                    VARIABLE.GetComponent<Rigidbody>().isKinematic = false;
                    VARIABLE.position = GameObject.Find("PLAYER").transform.position + Vector3.up;
                   
                }
                backpack.Clear();
            }
        }
    }
}
