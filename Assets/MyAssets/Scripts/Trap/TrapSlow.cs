using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class TrapSlow : Trap
    {
        public float slowFactor = 0.5f; // How much to slow down (e.g., 0.5f = half speed)
        public float duration = 2.0f; // How long the slowdown lasts in seconds

        protected override void StartCall(Transform cPlayer) 
        { 
            StartCoroutine(SlowdownEffect(cPlayer)); 
        }

        IEnumerator SlowdownEffect(Transform player)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null) {
                // Store original speed
                float originalSpeed = playerController.moveSpeed;  

                // Apply slowdown
                playerController.moveSpeed *= slowFactor;

                // Wait for duration
                yield return new WaitForSeconds(duration);

                // Restore original speed
                playerController.moveSpeed = originalSpeed;
            }
        }
    }
}
