using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    public float speed = 45f;
    public float maxSpeed = 60f;
    public float minSpeed;
    public float stockAcceleration = 5f;
    public float stockDeacceleration = -2.5f;
    public float pitchRate;
    public float stallSpeed;
    public float maxTurnSpeed;
    public float turnSpeed;
    public AnimationCurve turnCurve;
    public AnimationCurve pitchCurve;
    public AnimationCurve gravityCurve;
    Rigidbody rb;

    private float acceleration;
    private float wPitch;
    private float sPitch;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        acceleration = 0;
        wPitch = 0;
        sPitch = 0;
    }

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.drag = 0;
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        // MOVEMENT
        speed += acceleration * Time.deltaTime;
        if (speed >= 0)
        {
            Vector3 targetPos = transform.position + transform.forward * Time.deltaTime * speed;
            transform.position = targetPos;
        }
        // Increaseing altitude decreases speed, SHOULD BE DELTA?
        speed -= transform.forward.y * Time.deltaTime * 15f;

        // PITCH AND YAW
        Debug.Log(pitchCurve.Evaluate(speed / maxSpeed) * 40f);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.position, transform.right, Time.deltaTime * pitchCurve.Evaluate(speed/maxSpeed) * pitchRate);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.RotateAround(transform.position, -transform.right, Time.deltaTime * pitchCurve.Evaluate(speed/maxSpeed) * pitchRate);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 150f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(transform.position, -transform.forward, Time.deltaTime * 150f);
        }

        // ROLL
        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(transform.position, -transform.up, Time.deltaTime * 12f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * 12f);
        }
        //transform.RotateAround(transform.position, -transform.forward, Time.deltaTime * turnCurve.Evaluate(rPitch) * 180f);

        // ACCELERATION AND SPEED
        if (Input.GetKey(KeyCode.W))
        {
            acceleration = stockAcceleration;
        } else if (Input.GetKey(KeyCode.S) && speed > 0 && (speed > minSpeed))
        {
            acceleration = stockDeacceleration;
        } else
        {
            acceleration = 0;
        }

        if (speed > maxSpeed) speed = maxSpeed;
    }

    private void FixedUpdate()
    {
        Physics.gravity = new Vector3(0f, gravityCurve.Evaluate(speed/maxSpeed) * -9.82f, 0f);
        if (speed > stallSpeed) rb.velocity = rb.velocity * 0.99f;
    }

}
