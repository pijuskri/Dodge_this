using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{

    public Rigidbody projectile;
    public Transform bulletSpawn;
    private float projectileForce = 100f;
    private float fireRate = .25f;

    private float nextFireTime;

    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        // bulletSpawn = camera;
        //bulletSpawn.Rotate(Vector3.up, 90);

        if (Input.GetButtonDown("Fire2") && Time.time > nextFireTime)
        {
            Rigidbody cloneRb = Instantiate(projectile, bulletSpawn.position, bulletSpawn.rotation) as Rigidbody;
            cloneRb.AddForce(bulletSpawn.transform.forward * projectileForce);
            nextFireTime = Time.time + fireRate;
        }
    }
}