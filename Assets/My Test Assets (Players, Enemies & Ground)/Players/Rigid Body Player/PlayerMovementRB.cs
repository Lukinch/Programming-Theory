using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementRB : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private LayerMask groundMaks;
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject bullet;
    [Space]
    [SerializeField] public float health = 100f;
    [SerializeField] TextMeshProUGUI healthText;

    [Header("Camera settings")]
    [SerializeField] private float cameraSensitivity = 3f;

    [Header("Speed settings")]
    [SerializeField] private float speedModifier = 5f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float crouchSpeed = 5f;

    [Header("Jump settings")]
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private float jumpModifier = 1f;
    [SerializeField] private bool isGrounded;

    private Vector3 playerMovementInput;
    private Vector2 playerMouseInput;
    private Vector3 moveVector;
    private MainManager mainManager;
    private bool isPaused;

    private float xRotation;
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerBody.freezeRotation = true;
    }

    private void Start()
    {
        mainManager = GameObject.FindObjectOfType<MainManager>();
        isPaused = mainManager.IsPaused;
    }

    void Update()
    {
        isPaused = mainManager.IsPaused;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMaks);

        if (!isPaused)
        {
            PlayerMovement();
            CameraMovement();
            PlayerShooting();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.name == "Enemy Bullet")
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        health -= 20;
        healthText.text = $"Health : {health}";
    }

    private void PlayerMovement()
    {
        if (isGrounded)
        {
            ManageCrouchAndSprint();
            playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            moveVector = transform.TransformDirection(playerMovementInput) * speedModifier * sprintSpeed * crouchSpeed;
            playerBody.velocity = new Vector3(moveVector.x, playerBody.velocity.y, moveVector.z);

            if (Input.GetKey(KeyCode.Space))
            {
                playerBody.AddForce(Vector3.up * jumpModifier, ForceMode.Impulse);
            }
        }
    }

    private void CameraMovement()
    {
        playerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        xRotation -= playerMouseInput.y * cameraSensitivity;
        xRotation = Mathf.Clamp(xRotation, - 45f, 45f);

        transform.Rotate(0f, playerMouseInput.x, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void PlayerShooting()
    {
        bool isFiring = Input.GetButton("Fire1");
        var particle = bullet.GetComponent<ParticleSystem>().emission;

        if (isFiring) particle.enabled = true;
        else particle.enabled = false;
    }

    private void ManageCrouchAndSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprintSpeed = 2f;
        }
        else
        {
            sprintSpeed = 1f;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            crouchSpeed = 0.5f;
        }
        else
        {
            crouchSpeed = 1f;
        }
    }
}
