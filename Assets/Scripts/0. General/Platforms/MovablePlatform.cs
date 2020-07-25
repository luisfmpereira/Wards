using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{

    [SerializeField]
    private GameObject visual;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;


    void Update()
    {
        visual.transform.position = Vector2.MoveTowards(visual.transform.position,
        waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        if ((visual.transform.position - waypoints[currentWaypointIndex].position).magnitude <= 0.01f)
            ChangeWaypoint();
    }

    void ChangeWaypoint()
    {
        if (currentWaypointIndex >= waypoints.Length - 1)
            currentWaypointIndex = 0;
        else
            currentWaypointIndex++;
    }


}
