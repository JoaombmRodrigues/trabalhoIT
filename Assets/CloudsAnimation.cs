using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CloudsAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cloudsPrefabs;
    [SerializeField]
    private BoxCollider2D areaToSpawnClouds;
    [SerializeField]
    private BoxCollider2D initialSpawnArea;
    [SerializeField]
    private float minMoveSpeed;
    [SerializeField]
    private float maxMoveSpeed;
    [SerializeField]
    private float timePerSpawn = 3f;
    [SerializeField]
    private float cloudsToSpawnOnStart = 10f;

    private float timePassedSinceLastSpawn = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < cloudsToSpawnOnStart; i++) {
            SpawnCloud(initialSpawnArea);
        }
        timePassedSinceLastSpawn = timePerSpawn;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timePassedSinceLastSpawn >= timePerSpawn) 
        {
            SpawnCloud(areaToSpawnClouds);
            timePassedSinceLastSpawn = 0f;
        }
        timePassedSinceLastSpawn += Time.deltaTime; 
    }
    private void SpawnCloud(BoxCollider2D area)
    {
        int prefabNumber = Random.Range(0, cloudsPrefabs.Length);
        float ms = Random.Range(minMoveSpeed, maxMoveSpeed);
        Vector2 spawnPosition = GetRandomPointInArea(area);

        GameObject cloud = Instantiate(cloudsPrefabs[prefabNumber], spawnPosition, Quaternion.identity, this.transform);
        cloud.AddComponent<CloudsController>();
        CloudsController cloudScript = cloud.GetComponent<CloudsController>();
        cloudScript.SetMovementSpeed(ms);
    }
    private Vector2 GetRandomPointInArea(BoxCollider2D area)
    {
        Bounds bounds = area.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }
}
