using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] BoxCollider2D spawnArea;
    [SerializeField] UiManager ui;
    [SerializeField] AudioSource dieSound;
    private Transform spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {

        Vector2 spawnPosition = GetRandomPointInArea();

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Fly fly = enemy.GetComponent<Fly>();
        fly.spawner = this;
        fly.ui = ui;
        fly.dieSound = dieSound;
        enemy.SetActive(true);
    }

    private Vector2 GetRandomPointInArea()
    {
        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }
}
