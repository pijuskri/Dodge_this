using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPhysics : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter(Collision coll)
    {
        //Destroy(coll.gameObject);
        Destroy(gameObject);
    }
}
