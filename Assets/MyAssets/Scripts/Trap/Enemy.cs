using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    [RequireComponent(typeof(CharacterMovement))]
    public class Enemy : MonoBehaviour
    {
        public Transform orientation;
        public Transform orientationCheat;

        [Header("MOVEMENT & ROTATION")]
        public float moveSpeed = 5f;
        public float rotateSpeed = 10f;
        public enum MovementType { None, LeftDown, LeftUp, RightDown, RightUp };
        public MovementType moveType;
        public bool rotateForward;
        public bool scaleFlip;

        [Header("DETECT & DAMAGE")]
        public float detectPlayer = 0f;
        public float damageToPlayer = 1f;

        [Header("GIZMO")]
        public float gizmoLength = 15f;

        private bool setActive;
        private bool setDeath;

        private Vector2 _inputM;
        private float _inputR;
        private float _inputR2;
        private float _inputS;

        private Vector3 targetPos;

        CharacterMovement characterMovement;

        void Start()
        {
            characterMovement = GetComponent<CharacterMovement>();
            targetPos = transform.position;
            IsCheckTile(targetPos);

            #region InputType
            switch (moveType)
            {
                case MovementType.LeftDown:
                    // Code to execute when moveType is LeftDown
                    InputLeftDownUpdate();
                    break;
                case MovementType.LeftUp:
                    // Code to execute when moveType is LeftUp
                    InputLeftUpUpdate();
                    break;
                case MovementType.RightDown:
                    // Code to execute when moveType is RightDown
                    InputRightDownUpdate();
                    break;
                case MovementType.RightUp:
                    // Code to execute when moveType is RightUp
                    InputRightUpUpdate();
                    break;
                default:
                    // Optional: Code to execute if none of the above cases match
                    break;
            }

            orientationCheat.rotation = Quaternion.Euler(new Vector3(rotateForward ? _inputR2 : 0, _inputR, 0));
            if (scaleFlip == true) { orientationCheat.localScale = new Vector3(_inputS, 1, 1); }

            #endregion
            orientationCheat.rotation = Quaternion.Euler(new Vector3(rotateForward ? _inputR2 : 0, _inputR, 0));
            if (scaleFlip == true) { orientationCheat.localScale = new Vector3(_inputS, 1, 1); }
        }

        #region InputMovementControl
        public void InputLeftDownUpdate()
        {
            _inputM = new Vector2(-1, -1);
            _inputR = 90f;
            _inputR2 = -45f;
            _inputS = 1f;
        }

        public void InputLeftUpUpdate()
        {
            _inputM = new Vector2(-1, 1);
            _inputR = 90f;
            _inputR2 = 45f;
            _inputS = 1f;
        }

        public void InputRightDownUpdate()
        {
            _inputM = new Vector2(1, -1);
            _inputR = -90f;
            _inputR2 = -45f;
            _inputS = -1f;
        }

        public void InputRightUpUpdate()
        {
            _inputM = new Vector2(1, 1);
            _inputR = -90f;
            _inputR2 = 45f;
            _inputS = -1f;
        }
        #endregion

        void Update()
        {
            if (setDeath == false)
            {
                for (int i = 0; i < InGameController.Instance.playerEach.Count; i++)
                {
                    if ((InGameController.Instance.playerEach[i].playerController.gameObject.transform.position.y + detectPlayer) < this.transform.position.y)
                    {
                        setActive = true;
                    }
                }

                if (setActive == true)
                {
                    #region InputType
                    switch (moveType)
                    {
                        case MovementType.LeftDown:
                            // Code to execute when moveType is LeftDown
                            InputLeftDownUpdate();
                            break;
                        case MovementType.LeftUp:
                            // Code to execute when moveType is LeftUp
                            InputLeftUpUpdate();
                            break;
                        case MovementType.RightDown:
                            // Code to execute when moveType is RightDown
                            InputRightDownUpdate();
                            break;
                        case MovementType.RightUp:
                            // Code to execute when moveType is RightUp
                            InputRightUpUpdate();
                            break;
                        default:
                            // Optional: Code to execute if none of the above cases match
                            break;
                    }

                    orientationCheat.rotation = Quaternion.Euler(new Vector3(rotateForward ? _inputR2 : 0, _inputR, 0));
                    if (scaleFlip == true) { orientationCheat.localScale = new Vector3(_inputS, 1, 1); }

                    #endregion

                    #region Move
                    if (!characterMovement.ReturnCheckIsMoving())
                    {
                        if (_inputM != Vector2.zero) // I only put this so it could stop when pause
                        {
                            // Calculate the direction based on the current rotation
                            Vector3 direction = _inputM;

                            // Update targetPos based on the direction
                            targetPos = transform.position + direction;
                            targetPos = new Vector3Int(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y), Mathf.RoundToInt(targetPos.z));

                            // Check Before Move
                            IsCheckTile(targetPos);

                            // Move
                            //StartCoroutine(characterMovement.Move(null, null, transform, orientation, targetPos, new Vector3(0, _inputR, 0), moveSpeed, rotateSpeed));
                            characterMovement.Move(null, CheckAfterMove, transform, orientation, targetPos, new Vector3(0, _inputR, 0), moveSpeed, rotateSpeed, 0, true, false);
                        }

                        SetEnemyDestroy();
                    }
                    #endregion
                }

            }
            else
            {
                return;
            }
        }

        void CheckAfterMove()
        {
            IsCheckTargetPos(targetPos);
        }

        void IsCheckTargetPos(Vector3 targetPos)
        {
            Collider[] colliders = Physics.OverlapSphere(targetPos, 0.3f);

            foreach (Collider c in colliders)
            {
                if (c.GetComponent<PlayerController>() != null)
                {
                    PlayerController coll = c.GetComponent<PlayerController>();
                    coll.TakeDamage(damageToPlayer);
                }

                if (c.GetComponent<Trap>() != null)
                {
                    Trap coll = c.GetComponent<Trap>();
                    Destroy(coll.gameObject);
                }
            }
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

        void SetEnemyDestroy()
        {
            // Assuming inGameCamera, horizontal, vertical, and _screenSpace are already defined

            // Center position based on the inGameCamera
            Vector3 centerPosition = InGameController.Instance.inGameCamera.position;

            // Inner boundaries
            float innerLeft = centerPosition.x - InGameController.Instance.horizontal;
            float innerRight = centerPosition.x + InGameController.Instance.horizontal;
            float innerBottom = centerPosition.y - InGameController.Instance.vertical;
            float innerTop = centerPosition.y + InGameController.Instance.vertical;

            // Outer boundaries (expanded by _screenSpace)
            float outerLeft = innerLeft - InGameController.Instance._screenSpace.x;
            float outerRight = innerRight + InGameController.Instance._screenSpace.x;
            float outerBottom = innerBottom - InGameController.Instance._screenSpace.y;
            float outerTop = innerTop + InGameController.Instance._screenSpace.y;

            if (transform.position.x < outerLeft || transform.position.x > outerRight || transform.position.y > outerTop)
            {
                // Kill DOTween tweens
                characterMovement.KillDotweenCoroutine();

                // Set death flag and deactivate GameObject
                setDeath = true;
                characterMovement.KillDotweenCoroutine();
                gameObject.SetActive(false);
                characterMovement.KillDotweenCoroutine();
            }
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

            Gizmos.color = new Color(0, 0, 1, 0.45f);
            Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y - detectPlayer, transform.position.z), new Vector3(gizmoLength, 0.25f, 0.5f));
            Gizmos.color = new Color(0.5f, 0.5f, 1, 1f);
            Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y - detectPlayer, transform.position.z), new Vector3(gizmoLength, 0.25f, 0.5f));
        }
    }
}
