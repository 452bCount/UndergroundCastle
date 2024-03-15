using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    [RequireComponent(typeof(CharacterMovement))]
    public class Enemy : MonoBehaviour
    {
        public Transform orientation;

        [Header("MOVEMENT & ROTATION")]
        public float moveSpeed = 5f;
        public float rotateSpeed = 10f;

        public enum MovementType { None, LeftDown, LeftUp, RightDown, RightUp };
        public MovementType moveType;

        private Vector2 _inputM;
        private float _inputR;

        private Vector3 targetPos;

        CharacterMovement characterMovement;

        void Start()
        {
            characterMovement = GetComponent<CharacterMovement>();
            targetPos = transform.position;
            IsCheckTile(targetPos);
        }

        #region InputMovementControl
        public void InputLeftDownUpdate()
        {
            _inputM = new Vector2(-1, -1);
            _inputR = 90f;
        }

        public void InputLeftUpUpdate()
        {
            _inputM = new Vector2(-1, 1);
            _inputR = 90f;
        }

        public void InputRightDownUpdate()
        {
            _inputM = new Vector2(1, -1);
            _inputR = -90f;
        }

        public void InputRightUpUpdate()
        {
            _inputM = new Vector2(1, 1);
            _inputR = -90f;
        }
        #endregion

        void Update()
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
                    characterMovement.Move(null, null, transform, orientation, targetPos, new Vector3(0, _inputR, 0), moveSpeed, rotateSpeed);
                }
            }
            #endregion
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
    }
}
