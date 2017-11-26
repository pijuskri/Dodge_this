using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.pijuskri.test {
    public class CollisionPhysics : Photon.MonoBehaviour
    {

        // Use this for initialization
        private Vector3 initialVelocity;

        [SerializeField]
        private float minVelocity = 10f;
        public int dead = 0;
        public float damage = 0;

        private Vector3 lastFrameVelocity;
        private Rigidbody rb;

        private Vector3 position;
        private Quaternion rotation;

        float lerpSmoothing = 5f;
        //public static GameObject gameObject;

        float time = 0;

        private void OnEnable()
        {
            
            rb = GetComponent<Rigidbody>();
            rb.velocity = initialVelocity;
            gameObject.GetComponent<MeshCollider>().enabled = false;
        }

        private void Update()
        {
            time += Time.deltaTime;
            if (time > 0.07f) gameObject.GetComponent<MeshCollider>().enabled = true;
            if (time > 10f) { PhotonNetwork.Destroy(gameObject); Debug.Log(" time"); return; }
            //Debug.Log(time);
            lastFrameVelocity = rb.velocity;
            //if (rb.velocity == new Vector3(0, 0, 0)) { Destroy(gameObject); return; }
        }
        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log("lol");
            //if (collision.gameObject.tag == "bullet") { Destroy(collision.gameObject); Destroy(gameObject); Destroy(collision.gameObject);return;}
            // if (collision.gameObject.tag == "Player") { damagePlayer(damage, collision.gameObject.GetComponent<PhotonView>().owner.NickName); Destroy(gameObject); return; }
            if (collision.gameObject.tag == "Player")
            {
                photonView.RPC("damagePlayer", PhotonTargets.All,
                damage, collision.gameObject.GetComponent<PhotonView>().owner.NickName );
                if(photonView.isMine) PhotonNetwork.Destroy(gameObject);
                return;
            }
            
            Bounce(collision.contacts[0].normal);
        }
        [PunRPC]
        void damagePlayer(float damage, string name)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                Debug.Log(players[i].GetComponent<PhotonView>().owner.NickName);
                if(players[i].GetComponent<PhotonView>().owner.NickName == name )
                {
                    players[i].GetComponent<Player>().health -= damage; 
                }
            }
             Debug.Log("hit player"); return;
        }

        private void Bounce(Vector3 collisionNormal)
        {
            dead++;
            if (dead > 5) { PhotonNetwork.Destroy(gameObject); Debug.Log(" hit too many things"); return; }


            var speed = lastFrameVelocity.magnitude * 0.8f;

            // Debug.Log("Out Direction: " + collisionNormal);

            Vector3 direction = new Vector3();
            //direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

            Vector3 t = new Vector3();
            if (Mathf.Abs(Mathf.Abs(collisionNormal.x) - 1) < 0.2)
            {
                if (collisionNormal.x > 0) t = new Vector3(1, 0, 0);
                if (collisionNormal.x < 0) t = new Vector3(-1, 0, 0);
                direction = Vector3.Reflect(lastFrameVelocity.normalized, t);
                //Debug.Log("Out Direction: " + t);
            }
            if (Mathf.Abs(Mathf.Abs(collisionNormal.y) - 1) < 0.2)
            {
                if (collisionNormal.y > 0) t = new Vector3(0, 1, 0);
                if (collisionNormal.y < 0) t = new Vector3(0, -1, 0);
                direction = Vector3.Reflect(lastFrameVelocity.normalized, t);
                //Debug.Log("Out Direction: " + t);
            }
            if (Mathf.Abs(Mathf.Abs(collisionNormal.z) - 1) < 0.2)
            {
                if (collisionNormal.z > 0) t = new Vector3(0, 0, 1);
                if (collisionNormal.z < 0) t = new Vector3(0, 0, -1);
                direction = Vector3.Reflect(lastFrameVelocity.normalized, t);
                //Debug.Log("Out Direction: " + t);
            }

            double lol = 0;
            //if(gameObject.transform.position.x!=0) lol = Mathf.Atan(gameObject.transform.position.y / gameObject.transform.position.x) / Mathf.PI * 180;

            //gameObject.transform.rotation.
            //rb.velocity = direction * Mathf.Max(speed, minVelocity);
            rb.velocity = direction * speed;
            //gameObject.transform.Rotate(collisionNormal.x * angle, collisionNormal.y * angle, collisionNormal.z * angle);
            //gameObject.transform.Rotate( 180 - direction.y*90 - gameObject.transform.rotation.y * 90 , 0, 0);
            //gameObject.transform.rotation = new Quaternion(1 - direction.y, gameObject.transform.rotation.y, 0, 0);
            Vector3 temp = gameObject.transform.rotation.eulerAngles;
            //Debug.Log("Out Direction: " + collisionNormal + " ;   Angles: " + temp.x);



            if (Mathf.Abs(Mathf.Abs(collisionNormal.y) - 1) < 0.2) gameObject.transform.rotation = Quaternion.Euler(180 - temp.x, temp.y, temp.z);
            if (Mathf.Abs(Mathf.Abs(collisionNormal.x) - 1) < 0.2) gameObject.transform.rotation = Quaternion.Euler(temp.x, 180 - temp.y, temp.z);
            if (Mathf.Abs(Mathf.Abs(collisionNormal.z) - 1) < 0.2) gameObject.transform.rotation = Quaternion.Euler(temp.x, 180 - temp.y, temp.z);

            /*
            if (Mathf.Abs(collisionNormal.y) > 0.2f && Mathf.Abs(collisionNormal.x) > 0.2f) gameObject.transform.rotation = Quaternion.Euler((180 - temp.x)*Mathf.Abs(collisionNormal.x), 180 - temp.y, temp.z);
            else if (Mathf.Abs(collisionNormal.y) > 0.2f && Mathf.Abs(collisionNormal.z) > 0.2f) gameObject.transform.rotation = Quaternion.Euler(temp.x, 180 - temp.y,180 - temp.z);
            else if (Mathf.Abs(collisionNormal.x) > 0.2f && Mathf.Abs(collisionNormal.z) > 0.2f) gameObject.transform.rotation = Quaternion.Euler(180 - temp.x, temp.y, 180 - temp.z);
            else if (Mathf.Abs(collisionNormal.y) > 0.2f) gameObject.transform.rotation = Quaternion.Euler(180 - temp.x, temp.y, temp.z);
            else if (Mathf.Abs(collisionNormal.x) > 0.2f) gameObject.transform.rotation = Quaternion.Euler(temp.x, 180 - temp.y, temp.z);
            else if (Mathf.Abs(collisionNormal.z) > 0.2f) gameObject.transform.rotation = Quaternion.Euler(temp.x, 180 - temp.y,temp.z);
            */
            // Debug.Log(" ;  New Angle: " + gameObject.transform.rotation.eulerAngles);
            // gameObject.transform.rotation = Quaternion.Euler( (1 - direction.x)*90 / gameObject.transform.rotation.w, gameObject.transform.rotation.y * 90 / gameObject.transform.rotation.w, gameObject.transform.rotation.z * 90 / gameObject.transform.rotation.w);
            //gameObject.transform.Rotate( 180 - direction.x * 90, 0,0);
            //gameObject.transform.rotation. = new  (180 - angle, 0 , 0); 

            //rb.rotation. = ;
            //Vector3 kl = new Vector3(0, 0, 0);
            //Debug.Log(Quaternion.Euler(90,0,90));
        }
        /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.position);
            }
            else
            {
                position = (Vector3)stream.ReceiveNext();
                rotation = (Quaternion)stream.ReceiveNext();
            }
        }


        IEnumerator Alive()
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * lerpSmoothing);
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * lerpSmoothing);
            yield return null;
        }
        */

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
}
