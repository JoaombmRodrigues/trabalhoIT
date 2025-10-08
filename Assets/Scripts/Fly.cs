using System;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public int scoreValue = 10;
    public int hungerPoints = 10;
    public UiManager ui;
    public AudioSource dieSound;

    public static event Action<Fly> OnFlyDestroyed;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            dieSound.Play();
            Destroy(gameObject);
            ui.AddPoints(scoreValue);
            ui.AddHunger(hungerPoints);
            if (scoreValue > 0) OnFlyDestroyed?.Invoke(this);
        }
    }

    public void DestroyEntity()
    {
        Destroy(gameObject);
    }
}
