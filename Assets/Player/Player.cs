using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float respawnDelay;
    public float acceleration;
    public float maxSpeed;
    public float rotationSpeed;
    private float rotationDirection;
    private float isAccelerating;
    private Rigidbody2D rb;
    private SpriteRenderer propulsionSpriteRenderer;
    private Animator animator;
    private PlayerInput playerInput;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            if (spriteRenderer.gameObject.name == "Propulsion")
            {
                propulsionSpriteRenderer = spriteRenderer;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            propulsionSpriteRenderer.enabled = isAccelerating > 0f;
        }
        
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            transform.Rotate(0, 0, rotationDirection * rotationSpeed * Time.deltaTime);
            if (isAccelerating > 0f && rb.velocity.magnitude < maxSpeed) rb.AddForce(transform.up * acceleration);
            if (Math.Abs(transform.position.x) > 15 || Math.Abs(transform.position.y) > 13)
            {
                StartCoroutine(RespawnAfterDelay());
            }
        }
        
    }

    void OnAccelerate(InputValue value)
    {
        Debug.Log("Acceleration: " + isAccelerating);
        isAccelerating = value.Get<float>();
    }

    void OnRotate(InputValue value)
    {
        rotationDirection = value.Get<float>();
        Debug.Log("Direction: " + rotationDirection);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("isDead");
        StartCoroutine(RespawnAfterDelay());
        isAlive = false;
    }

    IEnumerator RespawnAfterDelay()
    {
        playerInput.enabled = false; // Disable input controls
        animator.ResetTrigger("hasRespawned");
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(respawnDelay); // Wait for the respawn delay
        // Delete asteroids
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject obj in taggedObjects)
        {
            Destroy(obj);
        }
        Respawn();
        playerInput.enabled = true; // Re-enable input controls

    }

    void Respawn()
    {
        transform.position = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        rotationDirection = 0f;
        isAccelerating = 0f;
        rb.velocity = Vector3.zero;
        animator.ResetTrigger("isDead");
        animator.SetTrigger("hasRespawned");
        isAlive = true;
        rb.constraints = RigidbodyConstraints2D.None;
    }
}
