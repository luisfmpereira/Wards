using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePowerUpInteractor : MonoBehaviour
{
    private float moveSpeed = 0;
    public bool isInteractingWithMagnet;
    private GameObject player;

    private void Update()
    {
        if (isInteractingWithMagnet)
            transform.position = Vector2.MoveTowards(transform.position,
            player.transform.position, moveSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("MagnetCollider"))
        {
            moveSpeed = other.GetComponentInParent<PlayerController>().magnetPowerUpMoveSpeed;
            player = other.gameObject;
            isInteractingWithMagnet = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MagnetCollider"))
        {
            moveSpeed = 0;
            player = null;
            isInteractingWithMagnet = false;
        }

    }

}
