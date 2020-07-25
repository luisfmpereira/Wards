using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CollectibleSpawnManager : MonoBehaviour
{
    private LevelManager levelManager;
    [SerializeField]
    private GameObject collectible;
    [Header("Collectible Spawns")]
    [SerializeField]
    private Transform[] collectiblePivots;
    private GameObject collectibleMaster;

    [Header("Spawn")]
    private int multipleSpawns = 4;
    [SerializeField]
    private float timeToSpawnMin;
    [SerializeField]
    private float timeToSpawnMax;
    private float selectedTime;
    public List<GameObject> activeSpawns;

    [Header("Starting values")]
    [SerializeField]
    private int startingCollectibles;
    private int[] selectedPivots;

    [Header("Special Collectible")]
    [SerializeField]
    private GameObject specialCollectible;
    [SerializeField]
    private float chanceForSpecialSpawn;
    public bool specialActive;

    [Header("Rotten Collectible")]
    [SerializeField]
    private GameObject rottenCollectible;
    [SerializeField]
    private float chanceForRottenSpawn;
    public bool rottenActive;



    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        //collectibles pivots and master references
        collectibleMaster = GameObject.Find("CollectiblesMaster");
        collectiblePivots = collectibleMaster.GetComponentsInChildren<Transform>().Where(x => x != collectibleMaster.transform).ToArray();

        //random generation of a number starting collectibles
        selectedPivots = GenerateRandom(startingCollectibles, 1, collectiblePivots.Length).ToArray();

        //initialize active spawns list
        activeSpawns = new List<GameObject>(collectiblePivots.Length);

        SpawnAllCollectibles();
        SpawnFirstSpecial();
        SelectStartingPivots();
        selectedTime = RandomizeSpawnTime();
    }

    void SpawnAllCollectibles()
    {
        //spawn one collectible in each pivot position and disable it
        for (int i = 0; i < collectiblePivots.Length; i++)
        {
            var obj = Instantiate(collectible, collectiblePivots[i].transform.position, Quaternion.identity, collectiblePivots[i].transform);
            obj.SetActive(false);
        }
    }

    void SelectStartingPivots()
    {
        //enable collectible only in randomized pivots
        for (int i = 0; i < selectedPivots.Length; i++)
        {
            collectiblePivots[selectedPivots[i]].GetChild(0).gameObject.SetActive(true);
            //add game object to active spawn list
            activeSpawns.Add(collectiblePivots[selectedPivots[i]].gameObject);
        }
    }


    private void Update()
    {
        if (!levelManager.isStarted)
            return;

        selectedTime -= Time.deltaTime;
        if (selectedTime <= 0)
        {
            for (int i = 0; i < multipleSpawns; i++)
            {
                EnableCollectibles();
            }
        }
    }

    void EnableCollectibles()
    {
        Transform x = null;

        for (int i = 0; i < multipleSpawns;)
        {
            x = collectiblePivots[(int)Random.Range(0, collectiblePivots.Length)];
            if (!activeSpawns.Contains(x.gameObject))
                i++;
        }

        //randomize pivot to spawn
        //check if pivot is full
        if (!activeSpawns.Contains(x.gameObject))
        {
            //try to spawn special
            if (chanceForSpecialSpawn > Random.Range(0, 100) && !specialActive)
            {
                SpawnSpecial(x);
            }
            else if (chanceForRottenSpawn > Random.Range(0, 100) && !rottenActive)
            {
                SpawnRotten(x);
            }
            else
            {
                EnableCommonCollectible(x);
            }
        }
        selectedTime = RandomizeSpawnTime();
    }

    void EnableCommonCollectible(Transform pivot)
    {
        pivot.GetChild(0).transform.position = pivot.transform.position;
        pivot.GetChild(0).gameObject.SetActive(true);
        activeSpawns.Add(pivot.gameObject);

    }


    void SpawnFirstSpecial()
    {
        specialActive = true;
        specialCollectible.transform.position = collectiblePivots[0].transform.position;
        specialCollectible.SetActive(true);
        activeSpawns.Add(collectiblePivots[0].gameObject);

    }


    void SpawnSpecial(Transform x)
    {
        specialActive = true;
        specialCollectible.transform.position = x.transform.position;
        specialCollectible.SetActive(true);
        activeSpawns.Add(x.gameObject);
    }

    void SpawnRotten(Transform x)
    {
        rottenActive = true;
        rottenCollectible.transform.position = x.transform.position;
        rottenCollectible.SetActive(true);
        activeSpawns.Add(x.gameObject);
    }

    float RandomizeSpawnTime()
    {
        return Random.Range(timeToSpawnMin, timeToSpawnMax);
    }


    public void CollectibleDespawn(GameObject obj)
    {
        //called from collectible object to remove from active spawn list
        activeSpawns.Remove(obj);
    }



    public List<int> GenerateRandom(int count, int min, int max)
    {
        List<int> result = new List<int>(count);

        for (int i = 0; i < count;)
        {
            var num = (int)Random.Range(min, max);
            if (!result.Contains(num))
            {
                result.Add(num);
                i++;
            }
        }
        return result;
    }


}
