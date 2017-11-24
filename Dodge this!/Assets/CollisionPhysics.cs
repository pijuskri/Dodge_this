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
        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.red, 2.0f);
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red, 2.0f);
        }
        Bounce(collision.contacts[0].normal);
    }

    private void Bounce(Vector3 collisionNormal)
    {

        var speed = lastFrameVelocity.magnitude * 0.8f;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);
        var angle = Vector3.Angle(lastFrameVelocity, collisionNormal);
        var anglex = (collisionNormal.x - lastFrameVelocity.x) * 45;
        var angley = 90 + lastFrameVelocity.y * 90;
        var anglez = (collisionNormal.z - lastFrameVelocity.z) * 45;

        double lol=0;
        //if(gameObject.transform.position.x!=0) lol = Mathf.Atan(gameObject.transform.position.y / gameObject.transform.position.x) / Mathf.PI * 180;
        
        //gameObject.transform.rotation.
        //rb.velocity = direction * Mathf.Max(speed, minVelocity);
        rb.velocity = direction * speed;
        //gameObject.transform.Rotate(collisionNormal.x * angle, collisionNormal.y * angle, collisionNormal.z * angle);
        //gameObject.transform.Rotate( 180 - direction.y*90 - gameObject.transform.rotation.y * 90 , 0, 0);
        //gameObject.transform.rotation = new Quaternion(1 - direction.y, gameObject.transform.rotation.y, 0, 0);
        Vector3 temp = gameObject.transform.rotation.eulerAngles;
        Debug.Log("Out Direction: " + collisionNormal + " ;   Angles: " + temp.x + " ;  New Angle: " + gameObject.transform.rotation);
        if (collisionNormal.y == -1 || collisionNormal.y == 1) gameObject.transform.rotation = Quaternion.Euler(180-temp.x, temp.y, temp.z);
        if (collisionNormal.x == -1 || collisionNormal.x == 1) gameObject.transform.rotation = Quaternion.Euler(temp.x, temp.y, 180 - temp.z);
        if (collisionNormal.z == -1 || collisionNormal.z == 1) gameObject.transform.rotation = Quaternion.Euler(temp.x,180 - temp.y, temp.z);
        // gameObject.transform.rotation = Quaternion.Euler( (1 - direction.x)*90 / gameObject.transform.rotation.w, gameObject.transform.rotation.y * 90 / gameObject.transform.rotation.w, gameObject.transform.rotation.z * 90 / gameObject.transform.rotation.w);
        //gameObject.transform.Rotate( 180 - direction.x * 90, 0,0);
        //gameObject.transform.rotation. = new  (180 - angle, 0 , 0); 
       
        //rb.rotation. = ;
        //Vector3 kl = new Vector3(0, 0, 0);
        //Debug.Log(Quaternion.Euler(90,0,90));
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
