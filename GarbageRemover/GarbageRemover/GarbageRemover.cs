using System.Collections.Generic;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace GarbageRemover
{
    public class GarbageRemover : Mod
    {
        private bool created;
        public override string ID { get { return "GarbageRemover"; } }
        public override string Name { get { return "GarbageRemover"; } }
        public override string Author { get { return "haverdaden"; } }
        public override string Version { get { return "1.0"; } }
        private List<FsmState> stateList = new List<FsmState>();
        private bool itemsTxtDeleted;

        //Called when mod is loading
        public override void OnLoad()
        {
            
        }

        // Update is called once per frame
        public override void Update()
        {
            if (Application.loadedLevelName == "GAME" && !created)
            {
                SetgarbageTrigger();

                GetSavegameStates();

                itemsTxtDeleted = false;
                created = true;
            }
            if (Application.loadedLevelName != "GAME")
            {
                created = false;
            }

            CheckIfSaving();
        }

        private static void SetgarbageTrigger()
        {
            foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.name == "GarbageTrigger")
                {
                    go.AddComponent<GarbageTrigger>();
                }
            }
        }

        private void GetSavegameStates()
        {
            foreach (PlayMakerFSM fsm in Resources.FindObjectsOfTypeAll(typeof(PlayMakerFSM)))
            {
                foreach (var state in fsm.FsmStates)
                {
                    if (state.Name == "Save")
                    {
                        stateList.Add(state);
                    }
                }
            }
        }

        private void CheckIfSaving()
        {
            if (created)
            {
                if (stateList != null)
                {
                    foreach (var VARIABLE in stateList)
                    {
                        if (VARIABLE.Active)
                        {
                            if (ES2.Exists("items.txt") && !itemsTxtDeleted)
                            {
                                ES2.Delete("items.txt");
                                ModConsole.Print("items.txt Deleted!");
                                itemsTxtDeleted = true;
                            }
                             
                        }
                    }
                }
            }
        }
    }
}
