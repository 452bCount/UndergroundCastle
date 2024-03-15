using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class TrapWeb : Trap
    {
        public int min, max;

        public int currentInt;
        private Transform player;
        private bool canGoOpposite = true;

        private bool cBeforeOrAfter;

        void Start()
        {
            IsCheckTile(transform.position);
        }

        protected override void StartCall(Transform cPlayer) { if (checkBeforeOrAfter == true) { currentInt = Random.Range(min, max + 1); player = cPlayer; StartCoroutine(CorUpdate()); } }

        public IEnumerator CorUpdate()
        {
            while (currentInt > 0)
            {
                player.GetComponent<PlayerController>().isAllowedToMove = false;
                player.GetComponent<PlayerController>().orientation.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                if (player.GetComponent<PlayerController>()._inputM.x < 0 && canGoOpposite)
                {
                    currentInt--;
                    canGoOpposite = false; // Set to false so the player can't go right next time
                }
                else if (player.GetComponent<PlayerController>()._inputM.x > 0 && !canGoOpposite)
                {
                    currentInt--;
                    canGoOpposite = true; // Set to true so the player can go right next time
                }

                yield return null;
            }

            player.GetComponent<PlayerController>().isAllowedToMove = true;
            player = null;
            this.gameObject.SetActive(false);
        }

        void IsCheckTile(Vector3 targetPos)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPos, 0.3f);

            foreach (Collider2D c in colliders)
            {
                if (c.GetComponent<TileDestroyer>() != null)
                {
                    TileDestroyer coll = c.GetComponent<TileDestroyer>();
                    coll.DestroyTile(targetPos);
                }
            }
        }
    }
}
