using System;
using Unity.VisualScripting;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public int scoreValue = 10;
    public UiManager ui;
    public EnemySpawner spawner;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            Destroy(gameObject);
            ui.AddPoints(scoreValue);
            spawner.SpawnEnemy();
        }
    }
}
