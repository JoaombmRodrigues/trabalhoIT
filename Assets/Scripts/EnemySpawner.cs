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

    [SerializeField] private DifficultyManager difficultyManager;
    private DifficultyManager.Difficulty currentDifficulty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beeCooldown = beeCooldownStart;
        SpawnFly();
    }

    private void OnEnable()
    {
        Fly.OnFlyDestroyed += HandleFlyDestroyed;

        if(difficultyManager != null)
        {
            difficultyManager.OnDifficultyChanged += HandleDifficultyChanged;
            currentDifficulty = difficultyManager.CurrentDifficulty;
        }   
    }

    private void OnDisable()
    {
        Fly.OnFlyDestroyed -= HandleFlyDestroyed;
    }

    private void HandleFlyDestroyed(Fly fly, bool eaten)
    {
        SpawnFly();
        if (eaten) difficultyManager.ModifyComboValue(fly.comboPoints);
    }

    private void HandleDifficultyChanged(DifficultyManager.Difficulty newDif)
    {
        currentDifficulty = newDif;
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
        fly.modifiedScoreValue = fly.baseScoreValue + fly.baseScoreValue * (int)currentDifficulty;
        fly.ui = ui;
        fly.dieSound = dieSound;
        fly.spawnArea = spawnArea;

        switch(currentDifficulty)
        {
            case DifficultyManager.Difficulty.Easy:
                fly.isMoving = false;
                fly.timeToLive = Mathf.Infinity;
                break;
            case DifficultyManager.Difficulty.Medium:
                fly.isMoving = true;
                fly.moveSpeed = 2f;
                fly.timeToLive = 2f;
                break;
            case DifficultyManager.Difficulty.Hard:
                fly.isMoving = true;
                fly.moveSpeed = 10f;
                fly.timeToLive = 1.5f;
                break;
            case DifficultyManager.Difficulty.Extreme:
                fly.isMoving = true;
                fly.moveSpeed = 20f;
                fly.timeToLive = 1.5f;
                break;
        }
        enemy.SetActive(true);

        if (currentDifficulty == DifficultyManager.Difficulty.Extreme && beeCooldown <= 0)
        {
            Vector2 secondarySpawnPosition = GetClosePoint(spawnPosition);
            GameObject bee = Instantiate(enemyPrefabs[1], secondarySpawnPosition, Quaternion.identity);
            Fly beeFly = bee.GetComponent<Fly>();
            beeAnim = bee.GetComponent<Animator>();
            currentTTL = beeTTL;
            beeFly.ui = ui;
            beeFly.dieSound = dieSound;
            beeFly.spawnArea = spawnArea;
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
