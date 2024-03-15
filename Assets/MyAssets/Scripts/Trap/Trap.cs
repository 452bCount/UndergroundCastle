using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class Trap : MonoBehaviour
    {
        // Before = false / After = true
        protected bool checkBeforeOrAfter;

        public virtual void SetStart(Transform cPlayer, bool cBeforeOrAfter) { checkBeforeOrAfter = cBeforeOrAfter; StartCall(cPlayer); }
        protected virtual void StartCall(Transform cPlayer) { /* Implement in child class */ }
    }
}
