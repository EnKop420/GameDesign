using Assets;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    public GameObject attackProjectile; // Assign your projectile prefab in the Inspector
    private Animator anim;
    private bool isAttackOnCooldown = false;
    private bool timerRunning = false;
    private float elapsedTime = 0f;
    public float attackCooldown = 1f; // Time between attacks
    public TMP_Text healthText;
    public TMP_Text timerText;
    void Start()
    {
        player = GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player component not found!");
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component not found!");
        }

        if (healthText == null)
        {
            Debug.LogError("Health text component not found!");
        }

        if (timerText == null)
        {
            Debug.LogError("Time text component not found!");
        }

        healthText.text = $"HP: {player.HP}";
        StartTimer();
    }

    void Update()
    {
        if (player == null) return;
        healthText.text = $"HP: {player.HP}";
        if (player.isDead)
        {
            StopTimer();
            healthText.text = "You died!";
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        player.Move(movement, isSprinting);

        // Attack Input
        if (Input.GetMouseButtonDown(0) && !isAttackOnCooldown) // Left mouse button click
        {
            Attack();
            StartCoroutine(StartCooldown());
        }
    }

    public void StartTimer()
    {
        timerRunning = true;
        StartCoroutine(UpdateTimer());
    }

    public void StopTimer()
    {
        timerRunning = false;
        StopCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (timerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
            yield return null; // Wait for the next frame
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void Attack()
    {
        if (attackProjectile != null)
        {
            Instantiate(attackProjectile, transform.position, transform.rotation);
            anim.Play("Attack");
        }
        else
        {
            Debug.LogError("Projectile prefab not assigned!");
        }
    }
    IEnumerator StartCooldown()
    {
        isAttackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isAttackOnCooldown = false;
    }
}