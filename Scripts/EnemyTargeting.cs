using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles finding Enemy Targets
 * */
public class EnemyTargeting : MonoBehaviour {
    
    private EnemyMain enemyMain;
    private Func<Enemy.IEnemyTargetable> getEnemyTarget;
    private Enemy.IEnemyTargetable activeEnemyTarget;

    private void Awake() {
        enemyMain = GetComponent<EnemyMain>();
    }

    private void Start() {
        if (getEnemyTarget == null) {
            SetGetTarget(() => Player.Instance);
        }
    }

    private void Update() {
        FindTarget();
    }

    public void SetGetTarget(Func<Enemy.IEnemyTargetable> getEnemyTarget) {
        this.getEnemyTarget = getEnemyTarget;
    }

   private void FindTarget() {
    float targetRange = enemyMain.EnemyStats.targetRange;
    activeEnemyTarget = null;

    if (getEnemyTarget != null) {
        Enemy.IEnemyTargetable target = getEnemyTarget();

        if (target == null) return;

        // 🚫 Don't target dead player
        if (target is Player player && player.IsDead()) return;

        if (Vector3.Distance(target.GetPosition(), enemyMain.GetPosition()) < targetRange) {
            activeEnemyTarget = target;
        }
    }
}

    public Enemy.IEnemyTargetable GetActiveTarget() {
        return activeEnemyTarget;
    }

}
