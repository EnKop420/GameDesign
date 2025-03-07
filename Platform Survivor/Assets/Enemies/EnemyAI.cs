using Assets;
using System.Collections;
using System.Numerics;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform attackTrigger; // Assign the collider GameObject here
    public GameObject attackProjectile; // Assign your projectile prefab in the Inspector
    public float attackCooldown = 1f; // Time between attacks

    private Transform playerTransform;
    private ICharacter playerCharacter;
    private ICharacter enemyCharacter; // Reference to the enemy's ICharacter interface
    private bool isAttackOnCooldown = false;
    private Animator anim;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            playerCharacter = playerObject.GetComponent<ICharacter>();
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }

        enemyCharacter = GetComponent<ICharacter>();
        anim = GetComponent<Animator>();
        if (enemyCharacter == null)
        {
            Debug.LogError("Enemy component implementing ICharacter not found!");
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null || playerCharacter == null || enemyCharacter == null) return;

        if (playerCharacter.isDead)
        {
            enemyCharacter.Move(UnityEngine.Vector2.zero, false);
            return;
        }

        // Calculate the direction from the enemy to the player (now with X and Y)
        UnityEngine.Vector2 direction = playerTransform.position - transform.position;

        // Normalize the direction
        enemyCharacter.Move(direction.normalized, false);

        // Flip the entire enemy GameObject's local scale (2D)
        if (direction.x < 0) // Flip if going right and currently facing left
        {
            if (transform.localScale.x > 0) // Only flip if currently facing right
            {
                transform.localScale = new UnityEngine.Vector2(-transform.localScale.x, transform.localScale.y); // Flip to face left
            }
        }

        else if (direction.x > 0) // Flip if going left and currently facing right
        {
            if (transform.localScale.x < 0) // Only flip if currently facing left
            {
                transform.localScale = new UnityEngine.Vector2(-transform.localScale.x, transform.localScale.y); // Flip to face right
            }
        }

        // Check for attack using the trigger collider
        if (attackTrigger != null && IsPlayerInAttackRange() && !isAttackOnCooldown)
        {
            //enemyCharacter.Attack(playerCharacter);
            //anim.Play("Attack");
            if (attackProjectile != null)
            {
                anim.Play("Attack");
                Instantiate(attackProjectile, transform.position, transform.rotation);
                StartCoroutine(StartCooldown());
            }
        }
    }



    private bool IsPlayerInAttackRange()
    {
        // Check if the player is within the attack trigger's bounds.
        BoxCollider2D attackCollider = attackTrigger.GetComponent<BoxCollider2D>();
        if (attackCollider == null) return false;

        CapsuleCollider2D playerCollider = playerTransform.GetComponent<CapsuleCollider2D>();
        if (playerCollider == null) return false;

        return attackCollider.bounds.Intersects(playerCollider.bounds);
    }

    IEnumerator StartCooldown()
    {
        isAttackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }
}
