using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class FinishLine : Trap
    {
        public int currentInt;
        private bool canGoOpposite = true;

        private bool cBeforeOrAfter;

        protected override void StartCall(Transform cPlayer) { if (checkBeforeOrAfter == true) { if (!InGameController.Instance.whatPlace.Contains(cPlayer)) { InGameController.Instance.whatPlace.Add(cPlayer); } } }
    }
}
