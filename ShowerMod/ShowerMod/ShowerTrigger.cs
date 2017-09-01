using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

//Standard unity MonoBehaviour class
namespace ShowerMod
{
    public class ShowerTrigger : MonoBehaviour
    {
        public static FsmFloat dirtiness;
        private bool cleanWait;

        // ReSharper disable once UnusedMember.Local
        private void OnTriggerStay(Collider collider)
        {
            if (collider.name == "PLAYER" && ShowerMod.ToggleShower)
                if (dirtiness.Value > 0 && !cleanWait)
                    StartCoroutine(getCleaner());
        }

        private IEnumerator getCleaner()
        {
            cleanWait = true;
            dirtiness.Value = dirtiness.Value - 2;
            yield return new WaitForSeconds(1);
            cleanWait = false;
        }
    }
}
