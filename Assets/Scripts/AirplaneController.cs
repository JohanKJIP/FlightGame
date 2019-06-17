using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    public float speed = 45f;
    public float maxSpeed = 60f;
    public float maxTurnSpeed;
    public float turnSpeed;
    public float lerpSpeed;
    public AnimationCurve turnCurve;
    Rigidbody rb;

    private float rPitch;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rPitch = 0;
    }

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.drag = 0;
    }

    void Update()
    {
        if (speed >= 0)
        {
            Vector3 targetPos = transform.position + transform.forward * Time.deltaTime * speed;
            transform.position = targetPos;
        }
        speed -= transform.forward.y * Time.deltaTime * 30f;
        //transform.Rotate(Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal") * turnSpeed);
        // this is fucked...
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.position, transform.right, Time.deltaTime * 40f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.RotateAround(transform.position, -transform.right, Time.deltaTime * 40f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 150f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(transform.position, -transform.forward, Time.deltaTime * 150f);
            //if (rPitch < 1)
            //{
            //    rPitch += 0.05f;
            //}
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(transform.position, -transform.up, Time.deltaTime * 12f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * 12f);
        }
        //else
        //{
        //    if (rPitch > 0)
        //    {
        //        rPitch -= 0.02f;
        //    }
        //}
        //transform.RotateAround(transform.position, -transform.forward, Time.deltaTime * turnCurve.Evaluate(rPitch) * 180f);

        if (Input.GetKey(KeyCode.W))
        {
            speed += 0.5f;
        }
        if (Input.GetKey(KeyCode.S) && speed > 0)
        {
            speed -= 0.5f;
        }

        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }

    }

    private void FixedUpdate()
    {
        rb.useGravity = !(speed > 15f);
        if (!rb.useGravity)
        {
            rb.velocity *= 0.98f;
        }
    }

}
