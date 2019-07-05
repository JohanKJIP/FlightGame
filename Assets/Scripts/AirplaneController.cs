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
    public float yawRate;
    public float rollRate;
    public float turnSpeed;
    public AnimationCurve turnCurve;
    public AnimationCurve pitchCurve;
    public AnimationCurve gravityCurve;
    public Transform startingPoint;
    public float dragFactor;
    Rigidbody rb;

    private float pitchAngle;
    private float rollAngle;
    private float originalDrag;
    private float originalAngularDrag;

    private float rollInput;
    private float pitchInput;
    private float throttleInput;
    private float yawInput;

    private float normalLift = 0.1f;
    private float normalAerodynamicEffect = 0.04f;

    private float acceleration;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        acceleration = 0;
    }

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.drag = 0;
        originalDrag = rb.drag;
        originalAngularDrag = rb.angularDrag;
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

        rollInput = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);
        pitchInput = Mathf.Clamp(Input.GetAxis("Vertical"), -1, 1);
        throttleInput = Mathf.Clamp(Input.GetAxis("Vertical2"), -1, 1);
        yawInput = Mathf.Clamp(Input.GetAxis("Horizontal2"), -1, 1);

        CalculateRollAndPitchAngles();
        CaluclateAerodynamicEffect();
        CalculateDrag();
        CalculateLift();
        CalculateTorque();

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
            ResetAirplane();
        }
    }

    private void CalculateRollAndPitchAngles()
    {
        // Calculate the flat forward direction (with no y component).
        var flatForward = transform.forward;
        flatForward.y = 0;
        // If the flat forward vector is non-zero (which would only happen if the plane was pointing exactly straight upwards)
        if (flatForward.sqrMagnitude > 0)
        {
            flatForward.Normalize();
            // calculate current pitch angle
            var localFlatForward = transform.InverseTransformDirection(flatForward);
            pitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z);
            // calculate current roll angle
            var flatRight = Vector3.Cross(Vector3.up, flatForward);
            var localFlatRight = transform.InverseTransformDirection(flatRight);
            rollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.x);
        }
    }

    private void CalculateDrag()
    {
        // Increase the drag based on speed.
        float extraDrag = rb.velocity.magnitude * dragFactor;
        rb.drag = originalDrag + extraDrag;
        // Reduce torque, harder to turn at higher speeds.
        rb.angularDrag = originalAngularDrag * speed;
    }

    private void CaluclateAerodynamicEffect()
    {
        // "Aerodynamic" calculations. This is a very simple approximation of the effect that a plane
        // will naturally try to align itself in the direction that it's facing when moving at speed.
        // Without this, the plane would behave a bit like the asteroids spaceship!
        if (rb.velocity.magnitude > 0)
        {
            // compare the direction we're pointing with the direction we're moving:
            float m_AeroFactor = Vector3.Dot(transform.forward, rb.velocity.normalized);
            // multipled by itself results in a desirable rolloff curve of the effect
            m_AeroFactor *= m_AeroFactor;
            // Finally we calculate a new velocity by bending the current velocity direction towards
            // the the direction the plane is facing, by an amount based on this aeroFactor
            var newVelocity = Vector3.Lerp(rb.velocity, transform.forward * speed,
                                           m_AeroFactor * speed * 0.02f * Time.deltaTime);
            rb.velocity = newVelocity;

            // also rotate the plane towards the direction of movement - this should be a very small effect, but means the plane ends up
            // pointing downwards in a stall
            rb.rotation = Quaternion.Slerp(rb.rotation,
                                                  Quaternion.LookRotation(rb.velocity, transform.up),
                                                  0.02f * Time.deltaTime);
        }
    }

    void CalculateLift()
    {
        Vector3 liftDirection = Vector3.Cross(rb.velocity, transform.right).normalized;
        float lift = speed * speed * normalLift * normalAerodynamicEffect;
        Debug.Log(lift);
        rb.AddForce(liftDirection * lift);
    }

    void CalculateTorque()
    {
        Vector3 torque = Vector3.zero;
        torque += pitchInput * pitchRate * transform.right;
        // Add torque for the yaw based on the yaw input.
        //torque += Input.GetAxis("Horizontal2") * yawRate * transform.up;
        // Add torque for the roll based on the roll input.
        torque += -rollInput * rollRate * transform.forward;
        // Add torque for banked turning.
        //torque += m_BankedTurnAmount * m_BankedTurnEffect * transform.up;
        // The total torque is multiplied by the forward speed, so the controls have more effect at high speed,
        // and little effect at low speed, or when not moving in the direction of the nose of the plane
        // (i.e. falling while stalled)
        rb.AddTorque(torque * speed/10);
    }

    private void ResetAirplane()
    {
        transform.position = startingPoint.position;
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.rotation = Quaternion.identity;
        transform.rotation = Quaternion.identity;
        speed = 20f;
    }

    private void FixedUpdate()
    {
        // UPDATE POSITION
        if (speed >= 0)
        {
            Vector3 targetPos = transform.position + transform.forward * Time.deltaTime * speed * 8f;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * 6f);
        }
    }

}
