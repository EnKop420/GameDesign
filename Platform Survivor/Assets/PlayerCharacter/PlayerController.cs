using Assets;
using System.Collections;
using TMPro;
using UnityEngine;
using System.IO;

public class PlayerController : MonoBehaviour
{
    private Player player;
    public GameObject attackProjectile;
    private Animator anim;
    private bool isAttackOnCooldown = false;
    private bool timerRunning = false;
    private float elapsedTime = 0f;
    public float attackCooldown = 1f;
    public TMP_Text healthText;
    public TMP_Text timerText;
    public TMP_Text highscoreText; // Public text component for displaying the highscore

    private float highscore = 0f;
    private string saveFilePath;

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
        if (highscoreText == null)
        {
            Debug.LogError("Highscore text component not found!");
        }

        healthText.text = $"HP: {player.HP}";
        StartTimer();

        saveFilePath = Path.Combine(Application.persistentDataPath, "highscore.txt");
        LoadHighscore();
        UpdateHighscoreText(); // Update the highscore text on start
    }

    void Update()
    {
        if (player == null) return;
        healthText.text = $"HP: {player.HP}";
        if (player.isDead)
        {
            StopTimer();
            healthText.text = "You died!";
            CheckAndSaveHighscore();
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        player.Move(movement, isSprinting);

        if (Input.GetMouseButtonDown(0) && !isAttackOnCooldown)
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
            yield return null;
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

    void CheckAndSaveHighscore()
    {
        if (elapsedTime > highscore)
        {
            highscore = elapsedTime;
            SaveHighscore();
            UpdateHighscoreText(); // Update the highscore text after saving
        }
    }

    void SaveHighscore()
    {
        try
        {
            File.WriteAllText(saveFilePath, highscore.ToString());
            Debug.Log("Highscore saved: " + highscore);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving highscore: " + e.Message);
        }
    }

    void LoadHighscore()
    {
        try
        {
            if (File.Exists(saveFilePath))
            {
                string savedHighscore = File.ReadAllText(saveFilePath);
                if (float.TryParse(savedHighscore, out highscore))
                {
                    Debug.Log("Highscore loaded: " + highscore);
                }
                else
                {
                    Debug.LogError("Invalid highscore data in file.");
                }
            }
            else
            {
                Debug.Log("No highscore file found.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading highscore: " + e.Message);
        }
    }

    void UpdateHighscoreText()
    {
        if (highscoreText != null)
        {
            int minutes = Mathf.FloorToInt(highscore / 60f);
            int seconds = Mathf.FloorToInt(highscore % 60f);
            highscoreText.text = string.Format("Highscore: {0:00}:{1:00}", minutes, seconds);
        }
    }
}