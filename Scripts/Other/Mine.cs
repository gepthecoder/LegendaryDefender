using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject explodeEffect;
    public int explosion_damage = 30;
    public float damage_radius = 12f;
    public float explosion_force = 500f;

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.gameObject.tag == "Enemy")
        {
            // DETONATE EXPLOSION
            DetonateExplosion();
        }
    }

    private void DetonateExplosion()
    {
        MineManager.instance.PLAY_BOOM_SOUND();
        // SPAWN EFFECT
        EXPLOSION_EFFECT();
        Destroy(gameObject, .5f);

        // ADD FORCE TO OBJECTS

        Collider[] colliders = Physics.OverlapSphere(transform.position, damage_radius);
        foreach(Collider collider in colliders)
        {
            Rigidbody rb = collider.transform.root.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosion_force, transform.position, damage_radius);

                EnemyHealth health = rb.GetComponent<EnemyHealth>();
                health.DamageEnemy(explosion_damage);
            }
        }
    }

    void EXPLOSION_EFFECT()
    {
        GameObject effect = Instantiate(explodeEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
    }

}
