using UnityEngine;
using System.Collections;

public class Gun
{
    public string name;
    public int bullets;
    public double damage;
    public float fireRate;
    public string fireMode;
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

   
    private float projectileForce = 4000f;
    private float fireRate = .25f;
    private int gun = 0;
    private float nextFireTime;
    private float nextShootTime=0;
    int shotgunShots = 0;

    void Start()
    {
        audioSource = gameObject.GetComponentInChildren<AudioSource>();

        guns[0] = new Gun();
        guns[0].name = "pistol";
        guns[0].bullets = 1;
        guns[0].damage = 15;
        guns[0].fireRate = 0.35f;
        guns[0].fireMode = "semi";
        guns[0].gunObject = gunObjects[0];
        guns[0].bullet = bulletObjects[0];


        guns[1] = new Gun();
        guns[1].name = "shotgun";
        guns[1].bullets = 7;
        guns[1].damage = 15;
        guns[1].fireRate = 1.5f;
        guns[1].fireMode = "semi";
        guns[1].gunObject = gunObjects[1];
        guns[1].bullet = bulletObjects[1];

        guns[2] = new Gun();
        guns[2].name = "smg";
        guns[2].bullets = 1;
        guns[2].damage = 10;
        guns[2].fireRate = 0.2f;
        guns[2].fireMode = "auto";
        guns[2].gunObject = gunObjects[2];
        guns[2].bullet = bulletObjects[0];
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Keypad1)) gun = 0;
        if (Input.GetKeyUp(KeyCode.Keypad2)) gun = 1;
        if (Input.GetKeyUp(KeyCode.Keypad3)) gun = 2;
        fireRate = guns[gun].fireRate;

       

        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].gunObject.SetActive(false);
            guns[gun].gunObject.SetActive(true);
        }

        if (guns[gun].fireMode == "semi")
        {
            if (Input.GetButtonDown("Fire2") && Time.time > nextFireTime)
            {
                shoot();
            }
        }
        if (guns[gun].fireMode == "auto")
        {
            if (Input.GetButton("Fire2") && Time.time > nextFireTime)
            {
                shoot();
            }
        }
        if (shotgunShots > 0 && gun==1) ShootShotgun();
        if (gun != 1) shotgunShots = 0;
    }
    void shoot()
    {
        Transform bulletSpawn = guns[gun].gunObject.transform;
        Rigidbody projectile = guns[gun].bullet.GetComponent<Rigidbody>();

        audioSource.PlayOneShot(gunSounds[gun]);
        ParticleSystem muzzleFlash = guns[gun].gunObject.GetComponentInChildren<ParticleSystem>();
        muzzleFlash.Play();
        //Debug.Log(gun);

        Rigidbody cloneRb= new Rigidbody();
        if (guns[gun].bullets == 1)
        {
            cloneRb = Instantiate(projectile, bulletSpawn.position, bulletSpawn.rotation) as Rigidbody;
            cloneRb.AddForce(bulletSpawn.transform.forward * projectileForce);

            cloneRb.gameObject.GetComponent<CollisionPhysics>().player = gameObject;
            cloneRb.gameObject.GetComponent<CollisionPhysics>().damage = (float)guns[gun].damage;
        }
        else
        {
            shotgunShots = guns[gun].bullets * 2;
        }
        nextFireTime = Time.time + fireRate;
    }
    void ShootShotgun()
    {
        //Debug.Log(shotgunShots);

        Transform bulletSpawn = guns[gun].gunObject.transform;
        Rigidbody projectile = guns[gun].bullet.GetComponent<Rigidbody>();
        Quaternion rand;
        Rigidbody cloneRb = new Rigidbody();
        if (Time.time > nextShootTime)
        {
            for (int i = 3; i > 0 && shotgunShots > 0; i--)
            {
                rand = Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

                cloneRb = Instantiate(projectile, bulletSpawn.position, bulletSpawn.rotation * rand) as Rigidbody;
                cloneRb.AddForce(cloneRb.transform.forward * projectileForce);

                cloneRb.gameObject.GetComponent<CollisionPhysics>().player = gameObject;
                cloneRb.gameObject.GetComponent<CollisionPhysics>().damage = (float)guns[gun].damage;
                
                shotgunShots--;
            }
            nextShootTime = Time.time + 0.01f;
        }
        //Debug.Log(Time.time);
    }
}