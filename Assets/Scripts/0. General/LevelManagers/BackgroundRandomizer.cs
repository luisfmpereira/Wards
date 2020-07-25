using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRandomizer : MonoBehaviour
{

    [SerializeField]
    private Sprite[] backgrounds;
    private SpriteRenderer backgroundUIView;

    private void Awake()
    {
        backgroundUIView = GetComponent<SpriteRenderer>();
        backgroundUIView.sprite = backgrounds[Random.Range(0, backgrounds.Length)];
    }

}
