using MSCLoader;
using UnityEngine;

//Standard unity MonoBehaviour class
namespace GarbageRemover
{
    public class GarbageTrigger : MonoBehaviour
    {

        void OnTriggerEnter(Collider garbageItem)
        {
            if (garbageItem.transform.parent == null)
            {
                GameObject.Destroy(garbageItem.gameObject);
            }
        }


    }
}
