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
        public int playerId = 0;
        // The Rewired Player
        private Player player;

        public Transform orientation;

        [ReadOnly] public float currentHealth;
        [ReadOnly] public float maxHealth;
        [ReadOnly] public int lifeLeft;
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
        [HideInInspector] public float prevHorizontalInput;

        [HideInInspector] public float verticalInput;
        [HideInInspector] public float prevVerticalInput;

        [HideInInspector] public bool finishLine;
        [ReadOnly] public bool seeFinishLine;
        [HideInInspector] public float boundarySide;
        [HideInInspector] public float boundaryBottom;
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
                playerHud.playerText.text = $"PLAYER {playerId + 1}";
                playerHud.gameObject.SetActive(true);
            }
        }

        void CheckForInput()
        {
            horizontalInput = (finishLine == true) ? horizontalInput : player.GetAxis("LJ Horizontal");
            verticalInput = (finishLine == true) ? -1 : player.GetAxis("LJ Vertical");

            // Directly use horizontalInput for movement
            if (horizontalInput != 0 && verticalInput != 0)
            {
                // Update the lastDirection based on horizontalInput if it's not 0
                if (horizontalInput > 0) { horizontalInput = prevHorizontalInput = 1; } else if (horizontalInput < 0) { horizontalInput = prevHorizontalInput = -1; }
                int hDirection = (singleMovement == true) ? (int)horizontalInput : (int)prevHorizontalInput; // -1 for left, 1 for right

                if (verticalInput > 0) { verticalInput = prevVerticalInput = 1; } else if (verticalInput < 0) { verticalInput = prevVerticalInput = -1; }
                int vDirection = (singleMovement == true) ? (int)verticalInput : (int)prevVerticalInput; // -1 for down, 1 for up

                _inputM = new Vector2(hDirection * 1, vDirection);
                _inputR = hDirection * -90;
            }
            else if (horizontalInput == 0 && finishLine == true)
            {
                singleMovement = false;
                int hDirection = (int)prevHorizontalInput; // -1 for left, 1 for right
                _inputM = new Vector2(hDirection * 1, -1);
                _inputR = hDirection * -90;
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
                if (targetPos.x > boundarySide && finishLine == false || targetPos.x < -boundarySide && finishLine == false || targetPos.y < boundaryBottom && seeFinishLine == false)//stop moving when past the 12.8m border
                { return; }

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
                    if (targetPos.x > boundarySide && finishLine == false || targetPos.x < -boundarySide && finishLine == false || targetPos.y < boundaryBottom && seeFinishLine == false)//stop breaking blocks past the 12.8m border
                        return;
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

        public void TakeLife(int lifePlayer)
        {
            lifeLeft += lifePlayer;
            if (playerHud != null) { playerHud.UpdateLife(lifeLeft); }
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
