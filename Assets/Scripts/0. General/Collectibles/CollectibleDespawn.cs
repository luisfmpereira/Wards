using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleDespawn : MonoBehaviour
{
    [Header("Score")]
    public int scoreToAdd;
    [SerializeField]
    public bool isSpecial;
    [SerializeField]
    public bool isRotten;

    [Header("Time to Despawn")]
    [SerializeField]
    private float timeToDespawnMin;
    [SerializeField]
    private float timeToDespawnMax;
    [SerializeField]
    private float selectedTime;
    [SerializeField]
    private CollectibleSpawnManager collectibleSpawnManager;
    private CollectiblePowerUpInteractor powerUpInteractor;
    private Animator animator;

    private void Start()
    {
        collectibleSpawnManager = FindObjectOfType<CollectibleSpawnManager>();
        powerUpInteractor = GetComponent<CollectiblePowerUpInteractor>();
        animator = GetComponent<Animator>();
        RandomizeTime();

    }

    private void Update()
    {
        if (this.enabled)
            selectedTime -= Time.deltaTime;

        if (selectedTime <= 0)
            PlayDespawnAnim();
    }

    public void PlayDespawnAnim()
    {
        animator.Play("CollectibleDespawn");
    }

    public void Despawn()
    {
        collectibleSpawnManager.CollectibleDespawn(transform.parent.gameObject);
        RandomizeTime();
        powerUpInteractor.isInteractingWithMagnet = false;

        if (isSpecial)
            collectibleSpawnManager.specialActive = false;
        if (isRotten)
            collectibleSpawnManager.rottenActive = false;
        this.gameObject.SetActive(false);

    }

    void RandomizeTime()
    {
        selectedTime = Random.Range(timeToDespawnMin, timeToDespawnMax);
    }

}
