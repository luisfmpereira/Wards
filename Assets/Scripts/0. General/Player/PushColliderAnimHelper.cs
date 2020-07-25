using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushColliderAnimHelper : MonoBehaviour
{
    PlayerController playerController;

    private void Awake() => playerController = transform.parent.GetComponent<PlayerController>();
    void DisableObject() => playerController.DisablePush();
}
