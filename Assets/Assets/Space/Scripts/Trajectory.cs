using UnityEngine;
using System.Collections;

public class Trajectory : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate(new Vector2(0f,.004f));
	}


	void OnCollisionEnter2D(Collision2D col) {
		
		if (col.gameObject.name == "Player") {
			Debug.Log (col);
		}
		
	}


}
