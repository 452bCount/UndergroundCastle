using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class FinishLine : Trap
    {
        public int currentInt;

        protected override void StartCall(Transform cPlayer) { if (checkBeforeOrAfter == true) { if (!InGameController.Instance.playersFinishPlace.Contains(cPlayer)) { InGameController.Instance.playersFinishPlace.Add(cPlayer); } } }
    }
}
