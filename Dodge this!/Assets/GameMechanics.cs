﻿using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


namespace Com.pijuskri.test
{
    public class GameMechanics : Photon.PunBehaviour
    {
        #region Public Variables


        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;

        #endregion 

        #region Public Methods

        public AudioClip menuMusic;
        public AudioSource audioSource;

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("Launcher");
        }

        void Start()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else if(SceneManager.GetActiveScene().name != "Launcher")
            {
                if (Player.LocalPlayerInstance == null)
                {
                    Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(85f, 3f, 110f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.Log("Ignoring scene load for " + Application.loadedLevelName);
                }
            }
        }
        void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            // e.g. store this gameobject as this player's charater in PhotonPlayer.TagObject
            //info.sender.NickName = Convert.ToString(UnityEngine.Random.Range(0,100));
            info.sender.NickName = "lol";
        }
        #endregion
        #region Private Methods
     
        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "Launcher")
            {
                audioSource.PlayOneShot(menuMusic);
                if (Cursor.lockState != CursorLockMode.None) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
                
            }
        }

        void LoadArena()
        {
            if (!PhotonNetwork.isMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.Log("PhotonNetwork : Loading Level : " + PhotonNetwork.room.PlayerCount);
            PhotonNetwork.LoadLevel("test");
        }


        #endregion
        #region Photon Messages


        public override void OnPhotonPlayerConnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


                //LoadArena();
            }
        }


        public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
        {
            Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects


            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("OnPhotonPlayerDisonnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected


                //LoadArena();
            }
        }

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            //SceneManager.LoadScene(0);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("Launcher");
        }

        #endregion

    }
}