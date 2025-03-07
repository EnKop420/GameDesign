using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 1f; // Time between spawns (in seconds)
    //public int spawnAmount = 5;

    private BoxCollider2D spawnArea;
    private int spawnedMonsters = 0;
    void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        if (spawnArea == null)
        {
            Debug.LogError("BoxCollider2D component not found on the spawner!");
            return;
        }

        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            Vector2 randomPosition = new Vector2(
                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y)
            );

            Vector3 spawnPosition = new Vector3(randomPosition.x, randomPosition.y, 0f);

            if (enemyPrefab != null)
            {
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                spawnedMonsters++;
            }
            else
            {
                Debug.LogError("Monster prefab is not assigned!");
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (spawnArea != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.bounds.size);
        }
    }
}
