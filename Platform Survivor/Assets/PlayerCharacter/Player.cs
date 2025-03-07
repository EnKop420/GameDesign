using Assets;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
    public int HP { get; set; } = 100;
    public int Damage { get; set; } = 20;
    public float MoveSpeed { get; set; } = 3f;
    public float SprintSpeed { get; set; } = 2f;
    public bool isDead { get; set; } = false;


    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    public AudioSource hurtAudioSource;
    public AudioSource deadAudioSource;
    public GameObject deathPanelUI;

    void Awake() // Use Awake to initialize components
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (rb == null || anim == null || sr == null)
        {
            Debug.LogError("Player components (Rigidbody2D, Animator, SpriteRenderer) not found!");
        }
    }

    public void Move(Vector2 vector, bool isSprinting)
    {
        if (isDead) return;

        float calculatedMovement = isSprinting ? MoveSpeed + SprintSpeed : MoveSpeed;
        rb.linearVelocity = vector * calculatedMovement;

        if (anim != null)
        {
            bool isWalking = vector.x != 0 || vector.y != 0;
            anim.SetBool("isWalking", isWalking);
            anim.SetBool("isRunning", isSprinting && isWalking);
        }

        if (sr != null && vector.x != 0)
        {
            sr.flipX = vector.x < 0;
        }
    }

    public void Attack()
    {
        // Attack for player is not used.
    }

    public void TakeDamage(int damageTaken)
    {
        HP -= damageTaken;
        if (HP <= 0)
        {
            Die();
            return;
        }
        Debug.Log($"Player took {damageTaken} damage. Current HP: {HP}");
        hurtAudioSource.Play();
        anim.Play("Hurt");
    }

    public void Die()
    {
        Debug.Log("Player died!");
        if (anim != null)
        {
            anim.Play("Dead");
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            deadAudioSource.Play();
            isDead = true;
            deathPanelUI.SetActive(true);
        }
    }
}