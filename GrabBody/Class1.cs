using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MSCLoader;

namespace BodyGrab
{
    using System.Collections;

    public class DisplayLabel : MonoBehaviour
    {

        private bool show = true;

        public void showText()
        {

        }

        public void OnGUI()
        {

            if (show)
            {
                try
                {
                    StartCoroutine(DisapearBoxAfter());
                }
                catch (Exception e)
                {
                    ModConsole.Print(e);
                    throw;
                }
               
               // GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 100, 20), "Hello Worldasdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa!");
                
            }
        }

        IEnumerator DisapearBoxAfter()
        {
            
            ModConsole.Print("YES");
            // suspend execution for waitTime seconds
            
            show = false;
            yield return new WaitForSeconds(5);
        }


    }
}
