using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class TrapElec : Trap
    {
        public BoxCollider colliderCenter;
        public BoxCollider colliderMoveAround;
        public Transform colliderTransform;
        public List<Vector3> colliderMove;
        public float waitTime = 1.0f; // Delay between moves, in seconds

        public float damageToPlayer = 1f;

        Transform currentPlayer;
        private Coroutine storeCoroutine;

        protected override void StartCall(Transform cPlayer)
        {
            currentPlayer = cPlayer;

            // Apply Logic based on which collider was triggered
            if (colliderCenter != null && colliderCenter.bounds.Contains(cPlayer.GetComponent<PlayerController>().targetPos))
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

        public void Start()
        {
            storeCoroutine = StartCoroutine(CorUpdate());
        }

        public IEnumerator CorUpdate()
        {
            int index = 0; // Start at the first position
            while (true) // Loop indefinitely
            {
                SetEnemyDestroy();

                if (colliderMoveAround != null && currentPlayer != null && colliderMoveAround.bounds.Contains(currentPlayer.position))
                {
                    currentPlayer.GetComponent<PlayerController>().TakeDamage(damageToPlayer/2);
                }

                if (colliderMove.Count == 0)
                {
                    yield break; // Exit if there are no positions to move to
                }

                colliderTransform.position = transform.position + colliderMove[index % colliderMove.Count]; // Move to the next position

                if (colliderMoveAround != null && currentPlayer != null && colliderMoveAround.bounds.Contains(currentPlayer.position))
                {
                    currentPlayer.GetComponent<PlayerController>().TakeDamage(damageToPlayer/2);
                }

                index++; // Increment index for the next position

                yield return new WaitForSeconds(waitTime); // Wait for the specified time before continuing
            }
        }

        void SetEnemyDestroy()
        {
            // Assuming inGameCamera, horizontal, vertical, and _screenSpace are already defined
            Vector3 centerPosition = InGameController.Instance.inGameCamera.position;
            float innerTop = centerPosition.y + InGameController.Instance.vertical;
            float outerTop = innerTop + InGameController.Instance._screenSpace.y;

            if (transform.position.y > outerTop)
            {
                if (storeCoroutine != null)
                {
                    StopCoroutine(storeCoroutine);
                    storeCoroutine = null; // Nullify the reference after stopping
                }

                Destroy(this.gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < colliderMove.Count; i++)
            {
                Gizmos.DrawWireSphere(transform.position + colliderMove[i], 0.2f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(colliderTransform.position, 0.35f);
        }
    }
}
