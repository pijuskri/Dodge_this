using UnityEngine;
using System.Collections;

public class Gun
{
    public string name;
    public int bullets;
    public double damage;
    public float fireRate;
    public GameObject gunObject;
    public GameObject bullet;
    public Gun() { }
}
public class Shoot : MonoBehaviour
{
    public Gun[] guns = new Gun[3];

    public GameObject[] gunObjects;
    public GameObject[] bulletObjects;

    public AudioClip[] gunSounds;
    public AudioSource audioSource;

   
    private float projectileForce = 2000f;
    private float fireRate = .25f;
    private int gun = 0;
    private float nextFireTime;

    void Start()
    {
        audioSource = gameObject.GetComponentInChildren<AudioSource>();

        guns[0] = new Gun();
        guns[0].name = "pistol";
        guns[0].bullets = 1;
        guns[0].damage = 1;
        guns[0].fireRate = 0.25f;
        guns[0].gunObject = gunObjects[0];
        guns[0].bullet = bulletObjects[0];


        guns[1] = new Gun();
        guns[1].name = "shotgun";
        guns[1].bullets = 7;
        guns[1].damage = 1;
        guns[1].fireRate = 1f;
        guns[1].gunObject = gunObjects[1];
        guns[1].bullet = bulletObjects[1];
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Keypad1)) gun = 0;
        if (Input.GetKeyUp(KeyCode.Keypad2)) gun = 1;
        fireRate = guns[gun].fireRate;

        Transform bulletSpawn = guns[gun].gunObject.transform;
        Rigidbody projectile = guns[gun].bullet.GetComponent<Rigidbody>();

        for (int i = 0; i < guns.Length-1; i++)
        {
            guns[i].gunObject.SetActive(false);
            guns[gun].gunObject.SetActive(true);
        }

        if (Input.GetButtonDown("Fire2") && Time.time > nextFireTime)
        {
            audioSource.PlayOneShot(gunSounds[gun]);
            ParticleSystem muzzleFlash = guns[gun].gunObject.GetComponentInChildren<ParticleSystem>();
            muzzleFlash.Play();
            //Debug.Log(gun);
            if (guns[gun].bullets == 1)
            {
                Rigidbody cloneRb = Instantiate(projectile, bulletSpawn.position, bulletSpawn.rotation) as Rigidbody;
                cloneRb.AddForce(bulletSpawn.transform.forward * projectileForce);
            }
            else
            {
                for (int i = 0; i < guns[gun].bullets; i++)
                {
                    Quaternion rand;
                    rand = Quaternion.Euler( Random.Range(-10,10), Random.Range(-10, 10), Random.Range(-10, 10));
                    //rand = Random.rotation;
                    Rigidbody cloneRb = Instantiate(projectile, bulletSpawn.position, bulletSpawn.rotation * rand) as Rigidbody;
                    cloneRb.AddForce(cloneRb.transform.forward * projectileForce);
                }
            }
            nextFireTime = Time.time + fireRate;
        }
    }
}