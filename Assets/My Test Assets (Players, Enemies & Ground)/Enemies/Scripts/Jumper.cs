using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : Monster
{
    [Header("Jumper Specific")]
    [SerializeField] private float jumpModifier = 1f;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMaks;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        projectile = GetComponentInChildren<ParticleSystem>();
        playerRB = FindObjectOfType<PlayerMovementRB>().GetComponent<PlayerMovementRB>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMaks);

        var particleEmission = projectile.emission;
        particleEmission.rateOverTime = projectilesPerSecond;

        ManageBehaviour();
        CheckHealthStatus();
        ShotPlayer();
    }

    public override void MoveTowardsPlayer()
    {
        if (isGrounded) rb.AddRelativeForce((Vector3.forward + Vector3.up).normalized * jumpModifier, ForceMode.Impulse);
    }
}
