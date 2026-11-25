using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShuriken : MonoBehaviour {

    private const float MOVE_SPEED = 90f;

    public static EnemyShuriken Create(Enemy enemySpawner, Vector3 spawnPosition, Vector3 moveDir) {
        Transform enemyShurikenTransform = Instantiate(GameAssets.i.pfEnemyShuriken, spawnPosition, Quaternion.identity);

        EnemyShuriken enemyShuriken = enemyShurikenTransform.GetComponent<EnemyShuriken>();
        enemyShuriken.Setup(enemySpawner, moveDir);

        // Play throw sound when shuriken is created
        Sound_Manager.PlaySound(Sound_Manager.Sound.Arrow_Fire, spawnPosition);

        return enemyShuriken;
    }

    private Enemy enemySpawner;
    private Vector3 moveDir;
    private Rigidbody2D shurikenRigidbody2D;
    private Transform particleTransform;

    private void Awake() {
        shurikenRigidbody2D = GetComponent<Rigidbody2D>();
        shurikenRigidbody2D.AddTorque(30f, ForceMode2D.Impulse);
        particleTransform = transform.Find("ParticleSystem");
    }

    private void Setup(Enemy enemySpawner, Vector3 moveDir) {
        this.enemySpawner = enemySpawner;
        this.moveDir = moveDir;
    }

    private void FixedUpdate() {
        shurikenRigidbody2D.linearVelocity = moveDir * MOVE_SPEED;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        int hitLayerMask = ~(1 << GameAssets.i.enemyLayer | 1 << GameAssets.i.ignoreRaycastLayer);
        bool colliderInLayerMask = ((1 << collider.gameObject.layer) & hitLayerMask) != 0;
        if (colliderInLayerMask) {
            Player player = collider.GetComponent<Player>();
            if (player != null) {
                // Play hit sound when shuriken hits player
                Sound_Manager.PlaySound(Sound_Manager.Sound.Arrow_Hit, transform.position);
                player.Damage(enemySpawner, 1f);
            } else {
                // Play deflect sound when shuriken hits a wall
                Sound_Manager.PlaySound(Sound_Manager.Sound.Arrow_Deflect, transform.position);
            }
            particleTransform.SetParent(null);
            particleTransform.GetComponent<ParticleSystem>().Stop();
            Destroy(particleTransform.gameObject, 1f);
            Destroy(gameObject);
        }
    }

}