using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerUpSpawnerManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] powerUps;

    [Header("Spawn variables")]
    [SerializeField]
    private float spawnCooldown;
    private float currentCooldown;
    [SerializeField]
    private GameObject spawnMaster;
    private Transform[] spawnPivots;
    public int activePowerUps;
    [SerializeField]
    private int maxActivePowerUps;



    private void Start()
    {
        currentCooldown = spawnCooldown;
        GetSpawnPivots();
        activePowerUps = 0;
    }


    private void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0)
            SpawnNewPowerUp();
    }

    void SpawnNewPowerUp()
    {
        if (activePowerUps <= maxActivePowerUps)
        {
            InstantiateRandomPowerUp();
            currentCooldown = spawnCooldown;
        }
    }

    private void InstantiateRandomPowerUp()
    {
        var x = powerUps[Random.Range(0, powerUps.Length)];
        var pivot = spawnPivots[Random.Range(0, spawnPivots.Length)];
        Instantiate(x, pivot.position, x.transform.rotation);
        activePowerUps++;
    }

    private void GetSpawnPivots()
    {
        spawnPivots = spawnMaster.GetComponentsInChildren<Transform>().Where(x => x != spawnMaster.transform).ToArray();
    }
}
