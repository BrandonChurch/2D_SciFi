using UnityEngine;
using System.Collections;

public class Station : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	
	}

	void OnMouseDown()
	{
		Debug.Log ("Stop that, it tickles!");
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.name == "Player") {
			Debug.Log (other);
		}
		
	}
}
