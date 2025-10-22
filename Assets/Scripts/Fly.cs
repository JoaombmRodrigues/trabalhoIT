using System;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public int baseScoreValue = 10;
    public int modifiedScoreValue = 0;
    public int comboPoints = 10;
    public bool isMoving;
    public float timeToLive = Mathf.Infinity;
    private float ttlTimer;
    public float moveSpeed;
    [SerializeField] private float minMovementDistance;
    [SerializeField] private float idleTime = 1.5f;
    private float idleTimer;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private Animator animator;
    public UiManager ui;
    public AudioSource dieSound;
    public BoxCollider2D spawnArea;

    public static event Action<Fly,bool> OnFlyDestroyed; //bool = true if eaten, false if despawned

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ttlTimer = timeToLive;
        animator.SetFloat("Ttl", ttlTimer);
    }

    void Update()
    {
        if (timeToLive != Mathf.Infinity)
        {
            ttlTimer -= Time.deltaTime;
            animator.SetFloat("Ttl", ttlTimer);
        }

        if (isMoving && spawnArea != null)
        //TODO: add movement code
        {
            WanderWithinArea();
        }

    }

    private void WanderWithinArea()
    {
        if (idleTimer > 0)
        {
            rb.linearVelocity = Vector2.zero;
            idleTimer -= Time.deltaTime;
            return;
        }

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        float distance = Vector2.Distance(transform.position, targetPosition);
        if (distance < minMovementDistance)
        {
            idleTimer = UnityEngine.Random.Range(0.5f, idleTime);
            ChooseNewTarget();
        }
    }
    
    private void ChooseNewTarget()
    {
        if (spawnArea == null) return;

        Bounds bounds = spawnArea.bounds;

        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        targetPosition = new Vector2(x, y);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            dieSound.Play();
            Destroy(gameObject);
            ui.AddPoints(modifiedScoreValue);
            if (baseScoreValue > 0) OnFlyDestroyed?.Invoke(this,true);
        }
    }

    public void DestroyEntity()
    {
        Destroy(gameObject);
        if (baseScoreValue > 0) OnFlyDestroyed?.Invoke(this,false);
    }
}
