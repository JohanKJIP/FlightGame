using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    //float distance = 15f;
    //float height = 2f;

    //float rotationDamping = 3.0f;
    private Vector3 targetOffset = new Vector3(0, 0.5f, -4);
    private float lerpFactor = 14f;

    void FixedUpdate()
    {
        Vector3 localOffset = target.right * targetOffset.x + target.up * targetOffset.y + target.forward * targetOffset.z;

        Vector3 desiredPosition = target.position + localOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.fixedDeltaTime * lerpFactor);

        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.fixedDeltaTime * lerpFactor);
    }

    /*
    void LateUpdate()
    {
        float wantedRotationAngleSide = target.eulerAngles.y;
        float currentRotationAngleSide = transform.eulerAngles.y;

        float wantedRotationAngleUp = target.eulerAngles.x;
        float currentRotationAngleUp = transform.eulerAngles.x;

        currentRotationAngleSide = Mathf.LerpAngle(currentRotationAngleSide, wantedRotationAngleSide, rotationDamping * Time.deltaTime);

        currentRotationAngleUp = Mathf.LerpAngle(currentRotationAngleUp, wantedRotationAngleUp, rotationDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(currentRotationAngleUp, currentRotationAngleSide, 0);

        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        transform.LookAt(target);

        transform.position += transform.up * height;
    }
    */
}
