using Assets;
using System.Numerics;
using UnityEngine;

public class Gorgon1 : MonoBehaviour, ICharacter
{
    public int HP { get; set; } = 50;
    public int Damage { get; set; } = 10;
    public float MoveSpeed { get; set; } = 3f;
    public float SprintSpeed { get; set; } = 0f; // Monster doesn't sprint

    public bool isDead { get; set; } = false;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(UnityEngine.Vector2 vector, bool isSprinting)
    {
        // Ignore isSprinting, as the monster doesn't sprint
        rb.linearVelocity = vector;

        bool isWalking = vector.x != 0 || vector.y != 0;
        anim.SetBool("isWalking", isWalking);
    }

    public void Attack()
    {
        
    }

    public void TakeDamage(int damage)
    {
        anim.Play("Hurt");
        HP -= damage;
        Debug.Log($"Monster Enemy took {damage} damage. Current HP: {HP}");

        if (HP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Monster Enemy died!");
        Destroy(gameObject);
    }
}
