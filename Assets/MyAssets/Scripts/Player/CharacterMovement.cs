using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace MoleSurvivor
{
    public class CharacterMovement : MonoBehaviour
    {
        private bool isMoving;
        private Coroutine rotateDelayCoroutine;
        private float distance;

        public void Move( Action checkBeforeMove, Action checkAfterMove, Transform _targetTransform, Transform _targetOrientation, Vector3 targetPos, Vector3 targetRotate, float moveSpeed, float rotationSpeed, float delayRotation = 0, bool isAllowedToMove = true)
        {
            checkBeforeMove?.Invoke();

            if (isAllowedToMove != true)
            {
                isMoving = false;
                checkAfterMove?.Invoke();

                return;
            }
            else
            {
                isMoving = true;

                // Calculate the distance to the target
                distance = Vector3.Distance(_targetTransform.position, targetPos);

                // Calculate the duration it should take to move to the target at the given speed
                float speedDuration = distance / moveSpeed;
                float rotateDuration = rotationSpeed;

                // Use DoTween for movement
                _targetTransform.DOMove(targetPos, speedDuration)
                    .OnComplete(() =>
                    {
                        // Once movement is complete, set the final position
                        _targetTransform.position = targetPos;
                        isMoving = false;
                        checkAfterMove?.Invoke();

                        // Check if not moving, then rotate orientation
                        // Start the delay coroutine for orientation reset
                        if (!isMoving && rotateDelayCoroutine != null) { StopCoroutine(rotateDelayCoroutine); }
                        if (!isMoving) { rotateDelayCoroutine = StartCoroutine(RotateDelayCoroutine(_targetOrientation, rotateDuration, delayRotation)); }

                    });

                // Use DoTween for rotation
                _targetOrientation.DORotate(targetRotate, rotateDuration)
                    .OnComplete(() =>
                    {
                    // Once rotation is complete, set the final rotation
                    _targetOrientation.rotation = Quaternion.Euler(targetRotate);
                    });
            }
        }

        private IEnumerator RotateDelayCoroutine(Transform targetOrientation, float rotateDuration, float rotationResetDelay)
        {
            yield return new WaitForSeconds(rotationResetDelay);
            targetOrientation.DORotate(Vector3.zero, rotateDuration);
        }

        public bool ReturnCheckIsMoving()
        {
            return isMoving;
        }
    }
}
