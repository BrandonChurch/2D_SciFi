using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	int health;
	// Use this for initialization
	void Start () {
		health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.forward * -.5f);

		if (health <= 0) {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.tag == "bullet") 
		{
			Destroy (other.gameObject);
			health = health - 10;
		}
	}

}
