using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Rewired;

namespace MoleSurvivor
{
    [RequireComponent(typeof(CharacterMovement))]
    public class PlayerController : MonoBehaviour
    {
        // The Rewired player id of this character
        [HideInInspector] public int playerId = 0;
        // The Rewired Player
        private Player player;

        public Transform orientation;

        [ReadOnly] public float currentHealth;
        [ReadOnly] public float maxHealth;
        [ReadOnly] public float moveSpeed = 0f;
        [ReadOnly] public float rotateDuration = 0f;
        [ReadOnly] public float rotateDelay = 0f;

        [ReadOnly] public bool singleMovement;

        [HideInInspector] public Vector3 targetPos;
        [HideInInspector] public Vector2 _inputM;
        private float _inputR;

        [ReadOnly] public Color playerColor;
        [ReadOnly] public PlayerHud playerHud;
        [HideInInspector] public CharacterMovement characterMovement;
        [ReadOnly] public bool isAllowedToMove = true;

        [HideInInspector] public float horizontalInput;
        public float prevHorizontalInput;
        [HideInInspector] public bool finishLine;

        #region

        public void AssignGamepad(int pId)
        {
            playerId = pId;
            player = ReInput.players.GetPlayer(playerId);
        }

        public void SetStart()
        {
            characterMovement = GetComponent<CharacterMovement>();
            targetPos = transform.position;
            IsCheckTile(targetPos);

            if (playerHud != null)
            {
                playerHud.SetPlayerColor(playerColor);
                maxHealth = currentHealth;
                playerHud.UpdateHP(currentHealth, maxHealth);
                playerHud.playerText.text = $"P{playerId + 1}";
                playerHud.gameObject.SetActive(true);
            }
        }

        void CheckForInput()
        {
            horizontalInput = (finishLine == true) ? horizontalInput : player.GetAxis("LJ Horizontal");

            // Directly use horizontalInput for movement
            if (horizontalInput != 0)
            {
                // Update the lastDirection based on horizontalInput if it's not 0
                if (horizontalInput > 0) { horizontalInput = prevHorizontalInput = 1; } else if (horizontalInput < 0) { horizontalInput = prevHorizontalInput = -1; }
                int direction = (singleMovement == true) ? (int)horizontalInput : (int)prevHorizontalInput; // -1 for left, 1 for right
                _inputM = new Vector2(direction * 1, -1);
                _inputR = direction * -90;
            }
        }

        public void SetUpdate()
        {
            // Single Movement or Continous Movement
            _inputM = (singleMovement == true) ? Vector2.zero : _inputM;

            CheckForInput();

            if (!isAllowedToMove) { return; }

            #region Move
            if (!characterMovement.ReturnCheckIsMoving() && isAllowedToMove == true && _inputM != Vector2.zero)
            {
                // Calculate the direction based on the current rotation
                Vector3 direction = _inputM;

                // Update targetPos based on the direction
                targetPos = transform.position + direction;
                targetPos = new Vector3Int(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y), Mathf.RoundToInt(targetPos.z));

                CheckBeforeMove();

                // Check Before Move
                IsCheckTile(targetPos);

                // Move
                characterMovement.Move(null, CheckAfterMove, transform, orientation, targetPos, new Vector3(0, _inputR, 0), moveSpeed, rotateDuration, rotateDelay, isAllowedToMove);
            }
            #endregion
        }

        void CheckBeforeMove()
        {
            IsCheckTargetPos(targetPos, false);
        }

        void CheckAfterMove()
        {
            IsCheckTargetPos(targetPos, true);
        }

        void IsCheckTargetPos(Vector3 targetPos, bool cBeforeOrAfter)
        {
            Collider[] colliders = Physics.OverlapSphere(targetPos, 0.3f);

            foreach (Collider c in colliders)
            {
                if (c.GetComponent<Trap>() != null)
                {
                    Trap coll = c.GetComponent<Trap>();
                    coll.SetStart(transform, cBeforeOrAfter);
                }
            }
        }

        public void IsCheckTile(Vector3 targetPos)
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

        public void TakeDamage(float damagePlayer)
        {
            currentHealth -= damagePlayer;
            if (currentHealth <= 0) { currentHealth = 0; }

            if (playerHud != null) { playerHud.UpdateHP(currentHealth, maxHealth); }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (Application.isPlaying) { Gizmos.DrawWireSphere(targetPos, 0.3f); }

            Gizmos.DrawWireCube(transform.position, new Vector3(1f, 1f, 1f));
            Gizmos.DrawWireCube(transform.position + new Vector3(1, 1, 0), new Vector3(1f, 1f, 1f));
            Gizmos.DrawWireCube(transform.position + new Vector3(1, -1, 0), new Vector3(1f, 1f, 1f));
            Gizmos.DrawWireCube(transform.position + new Vector3(-1, 1, 0), new Vector3(1f, 1f, 1f));
            Gizmos.DrawWireCube(transform.position + new Vector3(-1, -1, 0), new Vector3(1f, 1f, 1f));

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + new Vector3(1, 1, 0), 0.2f);
            Gizmos.DrawWireSphere(transform.position + new Vector3(1, -1, 0), 0.2f);
            Gizmos.DrawWireSphere(transform.position + new Vector3(-1, 1, 0), 0.2f);
            Gizmos.DrawWireSphere(transform.position + new Vector3(-1, -1, 0), 0.2f);
        }
        #endregion
    }
}
