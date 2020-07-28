using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public TrailRenderer trail;         // This should be set in the Prefab, alternatively in the Start-Method

    private Vector3 target;             // Position the arrow is trying to hit
    private Collider ownerCollider;     // Collider of the GameObject that shoots the Arrow
    private float flightSpeed;          // Speed of the Arrow
    private float hightMultiplier;      // The Parabola-Height of the flightpath (Arrows fly in a higher arc)
    private float lifeTime = 120f;      // Time until death (in seconds)  

    private float flightTimer;          // Timer used for flight-calculation
    private Vector3 startPoint;         // Position from where the Arrow was shot
    private float targetDistance;       // Distance from startPoint to the target
    private float speedToDistance;      // The speed of the Arrow relative to the distance (this gives the Arrow a constant speed, no matter the distance)
    private Vector3 lastPosition;       // Position of the Arrow from last FixedUpdate step
    private bool arrived;               // Whether or not the Arrow has hit something

    public int arrowDamage;
    public GameObject hitEffect;

    public GameObject[] arrowEffects;

    private bool isSpecialAttack;
    public float effectRadius = 6f;
    public float effectForceNormal = 12f;
    public float effectForceSkill02 = 200f;

    /// <summary>
    /// Shoot the Arrow
    /// </summary>
    /// <param name="target">The target the Arrow should be shot at</param>
    /// <param name="owner">The GameObject shooting the Arrow</param>
    /// <param name="flightSpeed">The Speed of the Arrow</param>
    /// <param name="hightMultiplier">The Parabola-hight of the flightpath</param>
    /// <param name="lifeTime">Time until the Arrow gets destroyed</param>
    public void Shoot(Vector3 target, GameObject owner, float flightSpeed, float hightMultiplier, float lifeTime, bool _isSpecialAttack)
    {
        this.target = target;
        ownerCollider = owner.GetComponent<Collider>();
        this.flightSpeed = flightSpeed;
        this.hightMultiplier = hightMultiplier;
        this.lifeTime = lifeTime;
        this.isSpecialAttack = _isSpecialAttack;
    }

    private void Awake()
    {
        // Initialize the Arrow
        target = Vector3.zero;
        startPoint = transform.position;
        lastPosition = transform.position;
    }

    private void Start()
    {
        // Make the Arrow destroy itself in x(LifeTime) seconds
        Invoke("DestroySelf", lifeTime);

        // Calculate the distance to the target
        targetDistance = Vector3.Distance(transform.position, target);

        // Calculate the flight-speed relative to the distance
        speedToDistance = flightSpeed / targetDistance * flightSpeed;

    }

    private void Update()
    {
        if (!arrived)
        {
            flightTimer += Time.deltaTime;

            // Move the Arrow along the Parabola
            transform.position = MathParabola.Parabola(startPoint, target, (targetDistance / 5f) * hightMultiplier, flightTimer * speedToDistance);

            // The direction the Arrow is currently flying
            Vector3 direction = transform.position - lastPosition;

            // Collision detection with raycast
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(new Ray(lastPosition, direction), out hit, direction.magnitude))
            {
                // Only collide with objects that are not tagged with "Projectile" and not the owner
                // Here you might want to add more things the Arrow should ignore
                if (hit.collider.tag != "Projectile" && hit.collider != ownerCollider && hit.collider.tag != "collectable")
                {
                    Arrive(hit);
                    GameObject hitPrefab = Instantiate(hitEffect, hit.point, Quaternion.identity);
                    float destroyTime = isSpecialAttack ? 4f : 2.5f;
                    Destroy(hitPrefab, destroyTime);
                    
                    Collider[] colliders = Physics.OverlapSphere(transform.position, effectRadius);

                    foreach(Collider nearByObject in colliders)
                    {
                        // add force
                        Rigidbody rb = nearByObject.transform.root.GetComponent<Rigidbody>();
                        if(rb != null)
                        {
                            float forceToUse = isSpecialAttack ? effectForceSkill02 : effectForceNormal;
                            rb.AddExplosionForce(forceToUse, transform.position, effectRadius);

                            EnemyHealth health = rb.GetComponent<EnemyHealth>();
                            if(health != null)
                            {
                                if (health.health <= 0) { return; }

                                int damageToUse = isSpecialAttack ? 20 : 5;
                                health.DamageEnemy(damageToUse);
                            }
                        }
                    }

                    if (arrowEffects.Length > 0)
                    {
                        foreach (GameObject effect in arrowEffects) { Destroy(effect); }
                    }

                    //Debug.Log("hit collider tag: " + hit.collider.transform);
                    EnemyHealth enemyHealth = hit.collider.transform.root.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        //we hitted the enemy somewhere
                        if (hit.collider.tag == "torso")
                        {
                            arrowDamage = 40;
                            //deal 40 damage
                            enemyHealth.DamageEnemy(arrowDamage);
                        }
                        else if(hit.collider.tag == "head")
                        {
                            arrowDamage = 80;
                            //deal 80 damage
                            enemyHealth.DamageEnemy(arrowDamage);
                        }
                        else if (hit.collider.tag == "arms")
                        {
                            arrowDamage = 20;
                            //deal 20 damage
                            enemyHealth.DamageEnemy(arrowDamage);
                        }
                    }
                    //if(hit.collider.transform.root.GetComponent<Rigidbody>() != null)
                    //{
                    //    if (isSpecialAttack)
                    //    {
                    //        hit.collider.transform.root.GetComponent<Rigidbody>().AddExplosionForce(30f, hit.point, 6f, 3f, ForceMode.Impulse);
                    //    }
                    //    else
                    //    {
                    //        hit.collider.transform.root.GetComponent<Rigidbody>().AddExplosionForce(4f, hit.point, 2f, 1.5f, ForceMode.Impulse);
                    //    }
                    //}
                    return;
                }
            }

            // Rotate the arrow
            transform.rotation = Quaternion.LookRotation(direction);

            // Update the lastPosition
            lastPosition = transform.position;
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void Arrive(RaycastHit hit)
    {
        // Mark the Arrow as arrived
        arrived = true;

        // Stop emmitting the trail when stuck (so stuck arrows moving with its parent i.e. the enemy, do not emmit a trail)
        // This is done with a short delay to avoid unwated artifacts
        Invoke("DisableTrailEmission", 0.01f);
        // Make the trail fade out fast and then disable it
        trail.time = 0.4f;
        Invoke("DisableTrail", 0.8f);

        // Make the arrow stuck at a random depth
        // Modify these values to change how deep into the object the arrows gets stuck (0 is the arrow-tip)
        transform.position = hit.point += transform.forward * Random.Range(0.25f, 0.6f);

        // Make the Arrow a child of the object it has hit (this causes the stuck arrow to move with its parent)
        MakeChildOfHitObject(hit.transform);
    }

    // Disable the arrow trail emission
    private void DisableTrailEmission()
    {
        trail.emitting = false;
    }

    // Disable the arrow trail
    private void DisableTrail()
    {
        trail.enabled = false;
    }

    private void MakeChildOfHitObject(Transform parentTransform)
    {
        // Only make the arrow a child if the object is suited to 'become a parent'
        // For more info this see the documentation
        if (IsSuitedParent(parentTransform))
        {
            Vector3 originalPosition = transform.position;
            Quaternion originalRotation = transform.rotation;

            // Reset the rotation of the arrow to get rid of mesh-deformation when parenting
            transform.rotation = new Quaternion();

            // Set the parent and keep the world position
            transform.SetParent(parentTransform, true);

            // Change the rotation of the individual LOD-models
            transform.GetChild(0).transform.rotation = originalRotation;
            transform.GetChild(1).transform.rotation = originalRotation;
            transform.GetChild(2).transform.rotation = originalRotation;

            // Rotate the trail around the original position to move it to end of the arrow (where it was before parenting)
            float angle;
            Vector3 axis;
            originalRotation.ToAngleAxis(out angle, out axis);
            transform.GetChild(3).transform.RotateAround(originalPosition, axis, angle);
        }
    }

    // Check whether the transform is suited to become the parent of the arrow
    private bool IsSuitedParent(Transform parent)
    {
        if (IsUniformScaled(parent))
            return true;
        else
        {
            if (IsUniformRotated(parent))
            {
                return true;
            }
            else
                //When the parent is non uniform scaled and rotated giving it a child will result in wierd mesh-deformation of the arrow
                return false;
        }
    }

    private bool IsUniformScaled(Transform parent)
    {
        // When x,y and z are all the same it means the scale is uniform
        if (parent.localScale.x == parent.localScale.y && parent.localScale.x == parent.localScale.z)
            return true;
        else
            return false;
    }

    private bool IsUniformRotated(Transform parent)
    {
        var rotation = parent.rotation.eulerAngles;

        // When x,y and z are all the same it means the rotation is uniform
        if (parent.rotation.x == parent.rotation.y && parent.rotation.x == parent.rotation.z)
            return true;
        else
        {
            // When each axis is a multiple of 90° it is uniform aswell (or atleast suitable)
            if (Mathf.Round(rotation.x) % 90f == 0 && Mathf.Round(rotation.y) % 90f == 0 && Mathf.Round(rotation.z) % 90f == 0)
                return true;
            else
                return false;
        }
    }
}