using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class FinishLine : Trap
    {
        public int currentInt;
        Vector2 inputM;

        protected override void StartCall(Transform cPlayer) 
        {
            if (checkBeforeOrAfter == false)
            {
                if (!InGameController.Instance.playersFinishPlace.Contains(cPlayer))
                {
                    inputM = cPlayer.GetComponent<PlayerController>()._inputM;
                    cPlayer.GetComponent<PlayerController>()._inputM = inputM;
                }
            }

            if (checkBeforeOrAfter == true) 
            { 
                if (!InGameController.Instance.playersFinishPlace.Contains(cPlayer)) 
                {
                    InGameController.Instance.playersFinishPlace.Add(cPlayer); 
                    cPlayer.GetComponent<PlayerController>().finishLine = true;
                } 
            } 
        }
    }
}
