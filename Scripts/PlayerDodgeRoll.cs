using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDodgeRoll : MonoBehaviour {

    private const float ROLL_SPEED = 250f;

    private enum State {
        Normal,
        Rolling
    }

    private PlayerMain playerMain;

    private State state;
    private Vector3 rollDir;
    private float rollSpeed;
    private float rollCooldownTimer;

    private void Awake() {
        playerMain = GetComponent<PlayerMain>();
        state = State.Normal;
    }

    private void Update() {
        switch (state) {
        default:
        case State.Normal:
    if (rollCooldownTimer > 0f) rollCooldownTimer -= Time.deltaTime;
    HandleInput();
    break;
        case State.Rolling:
            HandleRolling();
            break;
        }
    }

    private void FixedUpdate() {
        switch (state) {
        case State.Rolling:
            playerMain.PlayerRigidbody2D.linearVelocity = rollDir * rollSpeed;
            break;
        }
    }

    private void HandleInput() {
    if (Keyboard.current.spaceKey.wasPressedThisFrame && rollCooldownTimer <= 0f) {
        rollDir = playerMain.PlayerMovementHandler.GetLastMoveDir();
        rollSpeed = ROLL_SPEED;
        state = State.Rolling;
        playerMain.PlayerMovementHandler.Disable();
        playerMain.PlayerSwapAimNormal.PlayDodgeAnimation(rollDir);
        playerMain.Player.Dodged();
    }
}

    private void HandleRolling() {
        float rollSpeedDropMultiplier = 5f;
        rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

        float rollSpeedMinimum = 50f;
       if (rollSpeed < rollSpeedMinimum) {
    rollCooldownTimer = GetDashCooldown(); 
    state = State.Normal;
    playerMain.PlayerMovementHandler.Enable();
    playerMain.PlayerSwapAimNormal.SetWeapon();
}
    }
    
    /*
    private void HandleDash() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            float dashDistance = 30f;
            Vector3 beforeDashPosition = transform.position;
            if (TryMove(lastMoveDir, dashDistance)) {
                Transform dashEffectTransform = Instantiate(pfDashEffect, beforeDashPosition, Quaternion.identity);
                dashEffectTransform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(lastMoveDir));
                float dashEffectWidth = 30f;
                dashEffectTransform.localScale = new Vector3(dashDistance / dashEffectWidth, 1f, 1f);
            }
        }
    }
    */

    private float GetDashCooldown() {
    Weapon weapon = playerMain.Player.GetWeapon();
    switch (weapon.GetWeaponType()) {
        case Weapon.WeaponType.Punch:  return 0.5f;
        default:                       return 1.5f;  // Pistol, Rifle, Shotgun (Marine)
    }
}

}
