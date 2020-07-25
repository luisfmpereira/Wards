using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{

    [SerializeField]
    private Collider2D spikeCollider;
    [SerializeField]
    private GameObject spikeVisual;
    [SerializeField]
    private float expandSpeed;
    [SerializeField]
    private float contractSpeed;
    [SerializeField]
    private Transform expandedTransform;
    [SerializeField]
    private Transform shortenedTransform;
    [SerializeField]
    private float expandedCooldown;
    [SerializeField]
    private float contractedCooldown;
    private enum States
    {
        expanding,
        contracting
    }
    [SerializeField]
    private States currentState;


    private void Update()
    {
        if (currentState == States.expanding)
        {
            spikeVisual.transform.position = Vector2.MoveTowards(spikeVisual.transform.position, expandedTransform.position, expandSpeed * Time.deltaTime);

            if ((spikeVisual.transform.position - expandedTransform.position).magnitude <= 0.1f)
            {
                StartCoroutine(ChangeToContracting());
            }
        }

        if (currentState == States.contracting)
        {
            spikeVisual.transform.position = Vector2.MoveTowards(spikeVisual.transform.position, shortenedTransform.position, contractSpeed * Time.deltaTime);

            if ((spikeVisual.transform.position - shortenedTransform.position).magnitude <= 0.1f)
            {
                StartCoroutine(ChangeToExpanding());
            }
        }
    }




    IEnumerator ChangeToExpanding()
    {
        yield return new WaitForSeconds(contractedCooldown);
        currentState = States.expanding;

    }

    IEnumerator ChangeToContracting()
    {
        yield return new WaitForSeconds(expandedCooldown);
        currentState = States.contracting;

    }


}
