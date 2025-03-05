using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float sprintSpeed = 2f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }

    void Move()
    {
        // Get the input from the player
        float calculatedMovement = 0f;


        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Check if the Shift key is pressed. This means the player is sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            calculatedMovement = moveSpeed + sprintSpeed;
            anim.SetBool("isRunning", true);
        }
        else
        {
            calculatedMovement = moveSpeed;
            anim.SetBool("isRunning", false);
        }

        // Move the player
        rb.linearVelocity = new Vector2(horizontal * calculatedMovement, vertical * calculatedMovement);

        if (anim != null)
        {
            bool isWalking = horizontal != 0 || vertical != 0;
            anim.SetBool("isWalking", isWalking);
        }

        // Flip the player sprite if he's moving in the opposite direction
        if (horizontal != 0)
        {
            sr.flipX = horizontal < 0;
        }
    }

    void Attack()
    {
        
    }

    void TakeDamage()
    {

    }

    void Die()
    {

    }
}
