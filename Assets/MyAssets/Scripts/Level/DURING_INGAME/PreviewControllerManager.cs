using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Rewired;

namespace MoleSurvivor
{
    public class PreviewControllerManager : MonoBehaviour
    {
        [InlineEditor]
        public InputManager inputManager;

        [InlineEditor]
        public InGameController inGameController;
    }
}
