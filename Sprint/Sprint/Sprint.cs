using System.Collections;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace Sprint
{
    public class Sprint : Mod
    {
        private bool _modLoaded;
        private Transform player;
        private cInput cInput;
        private Transform fpscamera;
        private CharacterMotor playerCharacterController;
        private FsmState runState;
        private SprintMono sprintMono;
        private bool gettingTired;
        private FsmFloat playerFatigue;
        private FsmFloat playerDirtiness;
        private bool checkingDoublePress;
        private bool nosprint;
        private float _timeFirstPress = -1.0f;
        private bool _pressedOnce;
        private bool _reset;
        private bool _sprint;
        private FsmFloat playerThirst;
        private FsmFloat playerHunger;
        public override string ID => "Sprint";
        public override string Name => "Sprint";
        public override string Author => "haverdaden";
        public override string Version => "1.0";

        //Called when mod is loading
        public override void OnLoad()
        {
        }

        // Update is called once per frame
        public override void Update()
        {
            //Load
            if (Application.loadedLevelName == "GAME" && !_modLoaded)
                if (GameObject.Find("PLAYER"))
                {
                    player = GameObject.Find("PLAYER").transform;
                    playerCharacterController = player.GetComponent<CharacterMotor>();
                    sprintMono = player.gameObject.AddComponent<SprintMono>();

                    foreach (var fsm in player.GetComponents<PlayMakerFSM>())
                        foreach (var state in fsm.FsmStates)
                            if (state.Name == "Run")
                                runState = state;

                    foreach (var fsm in PlayMakerGlobals.Instance.Variables.FloatVariables)
                        switch (fsm.Name)
                        {
                            case "PlayerFatigue":
                                playerFatigue = fsm;
                                break;
                            case "PlayerDirtiness":
                                playerDirtiness = fsm;
                                break;
                            case "PlayerThirst":
                                playerThirst = fsm;
                                break;
                            case "PlayerHunger":
                                playerHunger = fsm;
                                break;
                        }

                    _modLoaded = true;
                }

            //Check double tap
            DoubleTap();

            //Sprint
            Sprinting();

            //Unload
            if (Application.loadedLevelName != "GAME" && _modLoaded)
                _modLoaded = true;
        }

        private void DoubleTap()
        {
            if (_modLoaded && cInput.GetKeyDown("Run"))
                if (Time.time - _timeFirstPress < 0.5f)
                {
                    _timeFirstPress = Time.time;

                    _sprint = true;
                }
                else
                {
                    _timeFirstPress = Time.time;
                }
        }

        private void Sprinting()
        {
            if (_sprint && cInput.GetKey("Run"))
            {
                playerCharacterController.inputMoveDirection = playerCharacterController.inputMoveDirection * 2f;
                playerCharacterController.movement.maxForwardSpeed = 10f;
                if (!gettingTired)
                    sprintMono.StartCoroutine(GetTired());
            }
            else if (_sprint && !cInput.GetKey("Run"))
            {
                _sprint = false;
            }
        }

        //Get Tired
        private IEnumerator GetTired()
        {
            gettingTired = true;
            playerFatigue.Value += 0.2f;
            playerDirtiness.Value += 0.2f;
            playerHunger.Value += 0.025f;
            playerThirst.Value += 0.05f;
            yield return new WaitForSeconds(1);
            gettingTired = false;
        }
    }
}
