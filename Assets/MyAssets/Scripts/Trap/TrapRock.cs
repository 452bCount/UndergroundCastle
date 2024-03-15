using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class TrapRock : Trap
    {
        protected override void StartCall(Transform cPlayer)
        {
            if (checkBeforeOrAfter != true)
            {
                cPlayer.GetComponent<PlayerController>().isAllowedToMove = false;
                cPlayer.GetComponent<PlayerController>()._inputM = new Vector2(0, 0);
                cPlayer.GetComponent<PlayerController>().targetPos = cPlayer.transform.position;
                cPlayer.GetComponent<PlayerController>().isAllowedToMove = true;
            }
        }
    }
}
