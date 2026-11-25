/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System;
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

/*
 * Enemy Minion with Aim
 * Handles Find Target, Move to it, Aim and Shoot
 * */
public class EnemyAimLogic : MonoBehaviour {

    private const float FIRE_RATE = .2f;
    private const float START_AIM_ANGLE = 20f;
    
    private enum State {
        Normal,
        AimingAtTarget,
        Attacking,
        Busy,
    }

    private EnemyMain enemyMain;
    private CharacterAim_Base characterBase;
    private float nextShootTime;
    private State state;
    private Enemy.IEnemyTargetable enemyTarget;
    private IAimShootAnims aimAnims;
    private AimingShoot aimingShoot;

    private void Awake() {
        enemyMain = GetComponent<EnemyMain>();
        characterBase = GetComponent<CharacterAim_Base>();
        aimAnims = GetComponent<IAimShootAnims>();
        aimingShoot = GetComponent<AimingShoot>();

        SetStateNormal();
    }

    private void Start() {
        characterBase.SetWeaponType(CharacterAim_Base.WeaponType.Pistol);
        enemyMain.OnDestroySelf += EnemyMain_OnDestroySelf;
        enemyMain.HealthSystem.SetHealthMax(100, true);
    }

    private void EnemyMain_OnDestroySelf(object sender, EventArgs e) {
        characterBase.DestroySelf();
    }

    private void Update() {
        switch (state) {
        case State.Normal:
            enemyTarget = enemyMain.EnemyTargeting.GetActiveTarget();
            if (enemyTarget != null) {
                state = State.AimingAtTarget;
                aimingShoot.StartAimingAtTarget(enemyTarget.GetPosition(), START_AIM_ANGLE, 0f, 10f, ShootTarget);
            }
            break;
        case State.AimingAtTarget:
            enemyTarget = enemyMain.EnemyTargeting.GetActiveTarget();
            if (enemyTarget != null) {
                Vector3 targetPosition = enemyTarget.GetPosition();
                aimAnims.SetAimTarget(targetPosition);
                aimingShoot.UpdateAimShootAtTarget(targetPosition);
            }
            break;
        case State.Attacking:
            break;
        case State.Busy:
            break;
        }
    }

    private void ShootTarget() {
        enemyTarget = enemyMain.EnemyTargeting.GetActiveTarget();
        if (enemyTarget != null) {
            Vector3 targetPosition = enemyTarget.GetPosition();
            Sound_Manager.PlaySound(Sound_Manager.Sound.Pistol_Fire, GetPosition());
            SetStateAttacking();
            aimAnims.ShootTarget(targetPosition, SetStateNormal);
        }
    }

    private void SetStateNormal() {
        state = State.Normal;
    }
    
    private void SetStateAttacking() {
        state = State.Attacking;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

}