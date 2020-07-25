using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{

    [SerializeField]
    private Transform exitPivot;

    [SerializeField]
    private GameObject otherHole;

    private AudioManager AudioManager;

    private void Start()
    {

        AudioManager = AudioManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.PlaySound("Hole");
            other.transform.position = exitPivot.position;

        }
    }


}
