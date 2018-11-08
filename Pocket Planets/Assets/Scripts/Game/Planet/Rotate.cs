using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30.0f;
    private Transform objectTransform;

    private void Start()
    {
        objectTransform = transform;
    }

    private void Update()
    {
        objectTransform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
