using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour {
    public Transform networkOwner;
    public Transform followTransform;

    [SerializeField] private float delayTime = .1f;
    [SerializeField] private float distance = .3f;
    [SerializeField] private int moveStep = 10;

    private Vector3 targetPosition;

    private void Update() {
        targetPosition = followTransform.position - followTransform .forward * distance;
        targetPosition += (transform.position - targetPosition) * delayTime ;
        targetPosition.z = 0;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveStep);
    }

}
