using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    private Player player;
    private Vector3 closedPosition;

    [SerializeField]
    private float slideSpeed = 1.0f;

    [SerializeField]
    private Transform openedTransform;

    [SerializeField]
    private SlidingDoorTrigger trigger;

    void Start()
    {
        closedPosition = transform.position;

        trigger.onPlayerEnter.AddListener(() => StartMove(openedTransform.position));
        trigger.onPlayerExit.AddListener(() => StartMove(closedPosition));
    }

    private void StartMove(Vector3 destination)
    {
        StopAllCoroutines();
        StartCoroutine(Move(destination));
    }

    private IEnumerator Move(Vector3 destination)
    {
        for (
            float distance = Vector3.Distance(destination, transform.position);
            distance > 0f;
            distance = Vector3.Distance(destination, transform.position)
            )
        {
            Vector3 moveDirection = (destination - transform.position).normalized;
            transform.position += Vector3.ClampMagnitude(moveDirection * slideSpeed * Time.deltaTime, distance);
            yield return null;
        }
    }
}