using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPhysics : MonoBehaviour {

    // Use this for initialization
    private Vector3 initialVelocity;

    [SerializeField]
    private float minVelocity = 10f;

    private Vector3 lastFrameVelocity;
    private Rigidbody rb;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = initialVelocity;
    }

    private void Update()
    {
        lastFrameVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bounce(collision.contacts[0].normal);
    }

    private void Bounce(Vector3 collisionNormal)
    {
        var speed = lastFrameVelocity.magnitude * 0.8f;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

        Debug.Log("Out Direction: " + direction);
        rb.velocity = direction * Mathf.Max(speed, minVelocity);
    }

    /*public float maxAngle = 95;

    void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        Vector3 vel = gameObject.GetComponent<Rigidbody>().velocity;
        // measure angle

        Debug.Log(Vector3.Angle(vel, -normal));
        if (Vector3.Angle(vel, -normal) < maxAngle)
        {
            // bullet bounces off the surface
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.Reflect(vel, normal);
        }
        else
        {
            // bullet penetrates the target - apply damage...
            Destroy(gameObject); // and destroy the bullet
        }
    }
    */
}
