using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : Monster
{
    [Header("Lerping Settings")]
    [SerializeField] [Range(2,5)] float startingYPos = 2f;
    [SerializeField] Vector3 offset;
    [SerializeField] [Range(0, 1)] float movementFactor;
    [SerializeField] [Range(2, 20)] float period = 2f;

    private Vector3 position;

    const float tau = Mathf.PI * 2; // constant value of 6.283

    // Start is called before the first frame update
    void Start()
    {
        projectile = GetComponentInChildren<ParticleSystem>();
        playerRB = FindObjectOfType<PlayerMovementRB>().GetComponent<PlayerMovementRB>();
        transform.position = new Vector3(transform.position.x, 4, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        StartLerping();
        ManageBehaviour();
        CheckHealthStatus();
        ShotPlayer();
    }

    public override void MoveTowardsPlayer()
    {
        var playerPos = playerRB.transform.position;
        playerPos.y = offset.y;
        transform.position = Vector3.MoveTowards(
                                                transform.position,
                                                playerPos,
                                                moveSpeed * Time.deltaTime
                                                );
    }

    private void StartLerping()
    {
        float cycles = Time.time / period; // Continually growing over time

        float rawSinWave = Mathf.Sin(cycles * tau); // Sin wave goes from -1 to 1

        movementFactor = (rawSinWave + 1f) / 2f; // Recalculated to go from 0 to 1.

        offset = new Vector3(0,startingYPos,0) * movementFactor;

        transform.position = new Vector3(transform.position.x, startingYPos + offset.y,transform.position.z);
    }
}
