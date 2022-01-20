using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float healthPoints = 100f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float turnSpeed = 5f;

    [Header("Firing")]
    [SerializeField] protected ParticleSystem projectile;
    [SerializeField] protected float projectilesPerSecond = 10f;
    [SerializeField] protected bool isFiring = false;

    [Header("Player Detection")]
    [SerializeField] protected PlayerMovementRB playerRB;
    [SerializeField] public float detectionRange = 20f;
    [SerializeField] public float shootingRange = 15f;
    [SerializeField] public float approachLimit = 10f;
    [SerializeField] public bool isFacingPlayer = false;
    protected float distanceToPlayer;

    [Header("Gizmos")]
    [SerializeField] protected bool enableGizmos = false;
    [SerializeField] protected Color detectionRangeColor = Color.blue;
    [SerializeField] protected Color shootingRangeColor = Color.red;
    [SerializeField] protected Color approachRangeColor = Color.yellow;

    public virtual void CheckHealthStatus()
    {
        if(healthPoints < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.name == "Bullet")
        {
            OnTakeDamage();
        }
    }

    public virtual void OnTakeDamage()
    {
        healthPoints -= 35f;
    }

    public virtual void ManageBehaviour()
    {
        distanceToPlayer = Vector3.Distance(playerRB.transform.position, transform.position);

        if (distanceToPlayer < detectionRange)
        {
            isFacingPlayer = true;
            RotateTowardsPlayer();

            if (distanceToPlayer < detectionRange &&
                distanceToPlayer > approachLimit)
            {
                MoveTowardsPlayer();
            }

            if (distanceToPlayer < shootingRange) isFiring = true;
            else isFiring = false;
        }
        else
        {
            isFacingPlayer = false;
        }
    }

    public virtual void ShotPlayer()
    {
        if (isFiring)
        {
            var particle = projectile.GetComponent<ParticleSystem>().emission;

            particle.enabled = isFiring;
        }
        else
        {
            var particle = projectile.GetComponent<ParticleSystem>().emission;

            particle.enabled = isFiring;
        }
    }

    public virtual void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(
                                                transform.position,
                                                playerRB.transform.position,
                                                moveSpeed * Time.deltaTime
                                                );
    }

    public virtual void RotateTowardsPlayer()
    {
        Quaternion startDirection = transform.rotation;
        Quaternion endDirection = Quaternion.LookRotation(playerRB.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(startDirection, endDirection, Time.deltaTime * turnSpeed);
    }

    private void OnDrawGizmos()
    {
        if (enableGizmos)
        {
            Gizmos.color = detectionRangeColor;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = shootingRangeColor;
            Gizmos.DrawWireSphere(transform.position, shootingRange);
            Gizmos.color = approachRangeColor;
            Gizmos.DrawWireSphere(transform.position, approachLimit);
        }
    }

}
