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
    public float turnSpeed;
    public AnimationCurve turnCurve;
    public AnimationCurve pitchCurve;
    public AnimationCurve gravityCurve;
    public Transform startingPoint;
    Rigidbody rb;

    private float acceleration;
    private float roll;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        acceleration = 0;
        roll = 0;
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

        speed += acceleration * Time.deltaTime;

        // Increaseing altitude decreases speed, SHOULD BE DELTA?
        speed -= transform.forward.y * Time.deltaTime * 15f;

        // PITCH
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.position, transform.right, Time.deltaTime * pitchCurve.Evaluate(speed/maxSpeed) * pitchRate);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.RotateAround(transform.position, -transform.right, Time.deltaTime * pitchCurve.Evaluate(speed/maxSpeed) * pitchRate);
        }

        // ROLL
        bool rolled = false;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (roll >= 0)
            {
                roll += 0.05f;
            } else
            {
                roll += 0.1f;
            }
            
            if (roll > 1) roll = 1;
            rolled = true;
            //transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 150f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (roll <= 0)
            {
                roll -= 0.05f;
            } else
            {
                roll -= 0.1f;
            }
            if (roll < -1) roll = -1;
            rolled = true;
            //transform.RotateAround(transform.position, -transform.forward, Time.deltaTime * 150f);
        }
        //Debug.Log(roll);
        if (Mathf.Abs(roll) < 0.0001f) roll = 0;
        //float angle = Mathf.LerpAngle(minAngle, maxAngle, Time.deltaTime);
        if (roll > 0)
        {
            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * turnCurve.Evaluate(Mathf.Abs(roll)) * turnSpeed);
        } else if (roll < 0)
        {
            transform.RotateAround(transform.position, -transform.forward, Time.deltaTime * turnCurve.Evaluate(Mathf.Abs(roll)) * turnSpeed);
        }

        if (!rolled) roll *= 0.7f;

        // YAW
        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(transform.position, -transform.up, Time.deltaTime * 12f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * 12f);
        }

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

        // RESET
        if (Input.GetKey(KeyCode.R))
        {
            resetAirplane();
        }
    }

    private void resetAirplane()
    {
        transform.position = startingPoint.position;
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.rotation = Quaternion.identity;
        transform.rotation = Quaternion.identity;
        speed = 20f;
    }

    private void FixedUpdate()
    {
        Physics.gravity = new Vector3(0f, gravityCurve.Evaluate(speed/maxSpeed) * -9.82f, 0f);
        if (speed > stallSpeed) rb.velocity = rb.velocity * 0.99f;

        // UPDATE POSITION
        if (speed >= 0)
        {
            Vector3 targetPos = transform.position + transform.forward * Time.deltaTime * speed * 8f;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * 6f);
        }
    }

}
