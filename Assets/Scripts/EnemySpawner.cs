using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] BoxCollider2D spawnArea;
    [SerializeField] UiManager ui;
    [SerializeField] AudioSource dieSound;
    [SerializeField] float beeCooldownStart;
    float beeCooldown;
    [SerializeField] float beeTTL;
    private float currentTTL;
    [SerializeField] private float minDistance = 10;
    [SerializeField] private float maxDistance = 20;
    Animator beeAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beeCooldown = beeCooldownStart;
        SpawnFly();
    }

    private void OnEnable()
    {
        Fly.OnFlyDestroyed += HandleFlyDestroyed;        
    }

    private void OnDisable()
    {
        Fly.OnFlyDestroyed -= HandleFlyDestroyed;
    }

    private void HandleFlyDestroyed(Fly fly)
    {
        SpawnFly();
        Debug.Log("Spawn Fly");
    }

    void Update()
    {
        beeCooldown -= Time.fixedDeltaTime;
        if (beeAnim != null)
        {
            beeAnim.SetFloat("Ttl", currentTTL);
            currentTTL -= Time.fixedDeltaTime;
        }
    }

    public void SpawnFly()
    {
        Vector2 spawnPosition = GetRandomPointInArea();

        GameObject enemy = Instantiate(enemyPrefabs[0], spawnPosition, Quaternion.identity);
        Fly fly = enemy.GetComponent<Fly>();
        fly.ui = ui;
        fly.dieSound = dieSound;
        enemy.SetActive(true);

        Vector2 secondarySpawnPosition = GetClosePoint(spawnPosition);

        if (beeCooldown<=0)
        {
            GameObject bee = Instantiate(enemyPrefabs[1], secondarySpawnPosition, Quaternion.identity);
            Fly beeFly = bee.GetComponent<Fly>();
            beeAnim = bee.GetComponent<Animator>();
            currentTTL = beeTTL;
            beeFly.ui = ui;
            beeFly.dieSound = dieSound;
            bee.SetActive(true);
            beeCooldown = beeCooldownStart;
        }
    }

    private Vector2 GetClosePoint(Vector2 pos)
    {
        Bounds bounds = spawnArea.bounds;
        Vector2 spawnPoint;

        do
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            //float minRadius2 = minDistance * minDistance;
            //float maxRadius2 = maxDistance * maxDistance;
            float randomDistance = Random.Range(minDistance, maxDistance);
            spawnPoint = pos + randomDirection * randomDistance;
        }
        while (spawnPoint.x < bounds.min.x ||
                spawnPoint.x > bounds.max.x ||
                spawnPoint.y < bounds.min.y ||
                spawnPoint.y > bounds.max.y);

        return spawnPoint;
    }

    //TODO: hook up beeTTL to animator property to despawn it after time elapses, maybe this should be in the fly script
    //TODO: know when flies are eaten so we can spawn more

    private Vector2 GetRandomPointInArea()
    {
        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }
}
