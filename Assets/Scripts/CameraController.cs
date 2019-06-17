using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.position - target.forward * 5f + Vector3.up * 1f;
        float bias = 0.95f;
        transform.position = transform.position * bias +
            targetPos * (1f - bias);
        transform.LookAt(target.position + target.forward * 40f);
    }
}
