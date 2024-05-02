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
        private Tween targetMoveTween;
        private Tween targetRotateTween;
        private Coroutine rotateDelayCoroutine;
        private float distance;

        private Transform targetTransform;
        private Transform targetOrientation;

        public void Move( Action checkBeforeMove, Action checkAfterMove, Transform _targetTransform, Transform _targetOrientation, Vector3 targetPos, Vector3 targetRotate, float moveSpeed, float rotationSpeed, float delayRotation = 0, bool isAllowedToMove = true, bool isPlayer = true)
        {
            if (transform.gameObject.activeSelf == false) { KillDotweenCoroutine(); return; }

            targetTransform = _targetTransform;
            targetOrientation = _targetOrientation;

            checkBeforeMove?.Invoke();

            if (isAllowedToMove != true)
            {
                isMoving = false;
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
                targetMoveTween = _targetTransform.DOMove(targetPos, speedDuration)
                    .OnComplete(() =>
                    {
                        // Once movement is complete, set the final position
                        _targetTransform.position = targetPos;
                        isMoving = false;
                        checkAfterMove?.Invoke();

                        //Check if not moving, then rotate orientation
                        //Start the delay coroutine for orientation reset
                        if (!isMoving && rotateDelayCoroutine != null && transform.gameObject.activeSelf == true) { StopCoroutine(rotateDelayCoroutine); }
                        if (!isMoving && transform.gameObject.activeSelf == true) { rotateDelayCoroutine = StartCoroutine(RotateDelayCoroutine(_targetOrientation, rotateDuration, delayRotation)); }
                    });

                if (isPlayer)
                {
                    // Use DoTween for rotation
                    targetRotateTween = _targetOrientation.DORotate(targetRotate, rotateDuration)
                        .OnComplete(() =>
                        {
                        // Once rotation is complete, set the final rotation
                        _targetOrientation.rotation = Quaternion.Euler(targetRotate);
                        });
                }
            }

            if (transform.gameObject.activeSelf == false) { KillDotweenCoroutine(); return; }
        }

        private IEnumerator RotateDelayCoroutine(Transform targetOrientation, float rotateDuration, float rotationResetDelay)
        {
            yield return new WaitForSeconds(rotationResetDelay);
            targetRotateTween = targetOrientation.DORotate(Vector3.zero, rotateDuration);
        }

        public void PlayDotweenCoroutine()
        {
            if (targetMoveTween != null) { targetMoveTween.Play(); }
            if (targetRotateTween != null) { targetRotateTween.Play(); }
        }

        public void StopDotweenCoroutine()
        {
            if (rotateDelayCoroutine != null) { StopCoroutine(rotateDelayCoroutine); }
            if (targetMoveTween != null) { targetMoveTween.Pause(); }
            if (targetRotateTween != null) { targetRotateTween.Pause(); }
        }

        public void KillDotweenCoroutine()
        {
            if (rotateDelayCoroutine != null) { StopCoroutine(rotateDelayCoroutine); }
            if (targetMoveTween != null) { targetMoveTween.Kill(); }
            if (targetRotateTween != null) { targetRotateTween.Kill(); }
        }

        public void DestroyDotweenCoroutine()
        {
            if (rotateDelayCoroutine != null) { StopCoroutine(rotateDelayCoroutine); rotateDelayCoroutine = null; }
            if (targetMoveTween != null) { targetMoveTween.Kill(); targetMoveTween = null; }
            if (targetRotateTween != null) { targetRotateTween.Kill(); targetRotateTween = null; }
        }

        public bool ReturnCheckIsMoving()
        {
            return isMoving;
        }
    }
}
