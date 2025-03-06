using Assets;
using UnityEngine;

public class PlayerAttackProjectile : MonoBehaviour
{
    public float speed = 7f;
    public float destroyDelay = 2f; // Time until projectile is destroyed
    private ICharacter attacker; // Reference to the attacker's ICharacter
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
            Destroy(gameObject); // Destroy if no Rigidbody
            return;
        }

        AimAtMouse();
        Debug.Log("Projectile launched!");
        Destroy(gameObject, destroyDelay);
    }

    void AimAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;

        // Calculate rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Set velocity
        rb.linearVelocity = direction.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetAttacker(other.GetComponent<ICharacter>());
            return;
        }

        ICharacter target = other.GetComponent<ICharacter>();
        if (target != null)
        {
            if (attacker != null)
            {
                target.TakeDamage(attacker.Damage); // Get damage from attacker
            }
            else
            {
                Debug.LogWarning("Attacker ICharacter not assigned, using default damage.");
                target.TakeDamage(30); // Default damage if attacker is not set
            }

            Destroy(gameObject);
        }
    }

    public void SetAttacker(ICharacter attacker)
    {
        this.attacker = attacker;
    }
}
