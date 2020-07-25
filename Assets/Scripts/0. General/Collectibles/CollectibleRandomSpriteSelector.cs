using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleRandomSpriteSelector : MonoBehaviour
{
    [SerializeField]
    private Sprite[] collectibles;



    private void OnEnable()
    {
        this.GetComponent<SpriteRenderer>().sprite = collectibles[Random.Range(0, collectibles.Length)];

    }

}
