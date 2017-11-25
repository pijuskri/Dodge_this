using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Com.pijuskri.test
{
    public class Player : Photon.MonoBehaviour
    {

        public float health = 100f;
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        private Vector3 moveDirection = Vector3.zero;
        private float gravity = 20.0f;

        public GameObject viewCamera;

        // Use this for initialization
        private void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.isMine)
            {
                Player.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {

            if (photonView.isMine)
            {
                //gameObject.GetComponent<Player>().enabled = true;
                gameObject.GetComponent<CharacterController>().enabled = true;
                gameObject.GetComponent<Shoot>().enabled = true;
                gameObject.GetComponent<FirstPersonController>().enabled = true;
                viewCamera.SetActive(true);

            }

                CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
            

            if (_cameraWork != null)
            {
                if (photonView.isMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.isMine == false && PhotonNetwork.connected == true)
            {
                return;
            }
           // if (health < 1) Destroy(gameObject);
            

            //gameObject.GetComponent<Shoot>().Cycle();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                Vector3 pos = transform.localPosition;
                stream.Serialize(ref pos);
            }
            else
            {
                Vector3 pos = Vector3.zero;
                stream.Serialize(ref pos);  // pos gets filled-in. must be used somewhere
            }
        }

        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
        }

        
        /*
        CharacterController controller;
        private float speed = 10.0f;
        private float turnSpeed = 200.0f;
        private Vector3 moveDirection = Vector3.zero;
        private float gravity = 1000.0f;

        // Use this for initialization
        void Start () {
            controller = gameObject.GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update () {
            if (controller.isGrounded)
            {
                moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
            }

            float turn = Input.GetAxis("Horizontal");
            transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
            controller.Move(moveDirection * Time.deltaTime);
            moveDirection.y -= gravity * Time.deltaTime;
        }
        */
    }
}
