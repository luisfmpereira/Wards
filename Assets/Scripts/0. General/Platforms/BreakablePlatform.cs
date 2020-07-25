using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{

    [SerializeField]
    private GameObject visual;
    [SerializeField]
    private SpriteRenderer visualSprite;
    [SerializeField]
    private Sprite fullPlatformSprite;
    [SerializeField]
    private Sprite[] breakingStates;
    [SerializeField]
    private float despawnTime;
    private float currentDespawnTime;
    [SerializeField]
    private float spawnTime;
    private float currentSpawnTime;
    private bool isDespawning;
    private bool isSpawning;
    private Animator animator;



    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        visual.SetActive(true);
        visualSprite = visual.GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (isDespawning)
        {
            animator.Play("BreakableShaking");
        
            currentDespawnTime -= Time.deltaTime;
            if (currentDespawnTime > 0)
                ChangeSprite();
            if (currentDespawnTime <= 0)
                Despawn();
        }

        if (isSpawning)
        {
            currentSpawnTime -= Time.deltaTime;
            if (currentSpawnTime <= 0)
                Spawn();
        }
    }



    void ChangeSprite() => visualSprite.sprite = breakingStates[Mathf.FloorToInt(despawnTime - currentDespawnTime)];




    private void Despawn()
    {
        visual.SetActive(false);
        animator.Play("Idle");
        currentSpawnTime = spawnTime;
        isSpawning = true;
        isDespawning = false;
    }


    private void Spawn()
    {
        visual.SetActive(true);
        visualSprite.sprite = fullPlatformSprite;
        isSpawning = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isDespawning && !isSpawning)
            {
                currentDespawnTime = despawnTime;
                isDespawning = true;
            }

        }
    }

}
