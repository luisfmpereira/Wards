using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDespawner : MonoBehaviour
{
    private Animator animator;
    private PowerUpSpawnerManager manager;

    [Header("Time to Despawn")]
    [SerializeField]
    private float timeToDespawnMin;
    [SerializeField]
    private float timeToDespawnMax;
    private float timeToDespawn;



    private void Awake()
    {
        manager = GameObject.Find("PowerUpsManager").GetComponent<PowerUpSpawnerManager>();
        animator = GetComponent<Animator>();
        RandomizeDespawnCooldown();
    }

    private void Update()
    {
        timeToDespawn -= Time.deltaTime;
        if (timeToDespawn <= 0)
            Despawn();
    }

    void RandomizeDespawnCooldown() => timeToDespawn = Random.Range(timeToDespawnMin, timeToDespawnMax);

    public void Despawn() => animator.Play("PowerUpDespawn");

    public void Destroy()
    {
        manager.activePowerUps--;
        Destroy(this.gameObject);
    }


}
