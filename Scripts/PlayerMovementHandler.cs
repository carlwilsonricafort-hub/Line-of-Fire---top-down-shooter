using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementHandler : MonoBehaviour {

    private const float SPEED = 50f;
    private const float FOOTSTEP_INTERVAL = 0.50f;

    private PlayerMain playerMain;

    private Vector3 moveDir;
    private Vector3 lastMoveDir;
    private float footstepTimer;
    private bool wasMoving = false;
    private AudioSource footstepAudioSource; // store reference to stop it

    private void Awake() {
        playerMain = GetComponent<PlayerMain>();
    }

    private void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        float moveX = 0f;
        float moveY = 0f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveY = +1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveY = -1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = +1f;

        moveDir = new Vector3(moveX, moveY).normalized;

        bool isIdle = moveX == 0 && moveY == 0;

        if (isIdle) {
            playerMain.PlayerSwapAimNormal.PlayIdleAnim();

            // Just transitioned to idle — stop the looping footstep
            if (wasMoving) {
                StopFootsteps();
            }

            wasMoving = false;
        } else {
            lastMoveDir = moveDir;
            playerMain.PlayerSwapAimNormal.PlayMoveAnim(moveDir);

            // Just started moving — play immediately
            if (!wasMoving) {
                StopFootsteps(); // safety: stop any leftover
                footstepAudioSource = Sound_Manager.PlaySound(Sound_Manager.Sound.Footsteps_Hero, transform.position);
                footstepTimer = FOOTSTEP_INTERVAL;
            } else {
                // Already moving — tick the interval for the next footstep
                footstepTimer -= Time.deltaTime;
                if (footstepTimer <= 0f) {
                    StopFootsteps(); // stop the previous looping source
                    footstepAudioSource = Sound_Manager.PlaySound(Sound_Manager.Sound.Footsteps_Hero, transform.position);
                    footstepTimer = FOOTSTEP_INTERVAL;
                }
            }

            wasMoving = true;
        }
    }

    private void StopFootsteps() {
        if (footstepAudioSource != null && footstepAudioSource.isPlaying) {
            Sound_Manager.StopSound(footstepAudioSource);
            footstepAudioSource = null;
        }
    }

    private void FixedUpdate() {
        playerMain.PlayerRigidbody2D.linearVelocity = moveDir * SPEED;
    }

    public void Enable() {
        enabled = true;
    }

    public void Disable() {
        enabled = false;
        StopFootsteps(); // stop sound when player is disabled
        playerMain.PlayerRigidbody2D.linearVelocity = Vector3.zero;
    }

    public Vector3 GetLastMoveDir() {
        return lastMoveDir;
    }
}