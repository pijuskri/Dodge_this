using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.pijuskri.test
{
    public class PlayerGUI : MonoBehaviour
    {

        [Tooltip("UI Text to display Player's Name")]
        public Text PlayerNameText;


        [Tooltip("UI Slider to display Player's Health")]
        public Slider PlayerHealthSlider;

        [Tooltip("Pixel offset from the player target")]
        public Vector3 ScreenOffset = new Vector3(0f, 30f, 0f);

        float characterControllerHeight = 0f;
        Transform targetTransform;
        Vector3 targetPosition;

        Player target;
        // Use this for initialization
        void Start()
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localPosition = new Vector3(150, -100, 0);
            gameObject.GetComponent<RectTransform>().pivot = new Vector2(1,1);
            //transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        void Awake()
        {
            
            this.GetComponent<Transform>().SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
        }

        // Update is called once per frame
        void Update()
        {
            // Reflect the Player Health
            if (PlayerHealthSlider != null)
            {
                PlayerHealthSlider.value = target.health;
            }
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }
        void LateUpdate()
        {
           /* // #Critical
            // Follow the Target GameObject on screen.
            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + ScreenOffset;
            }
            */
        }
        public void SetTarget(Player targetNew)
        {
            if (targetNew == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = targetNew;
            if (PlayerNameText != null)
            {
                PlayerNameText.text = target.photonView.owner.NickName;
            }
            CharacterController _characterController = target.GetComponent<CharacterController>();
            // Get data from the Player that won't change during the lifetime of this Component
            characterControllerHeight = _characterController.height;
        }
    }
}
