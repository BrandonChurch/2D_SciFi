using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
	public float speed = .01f;
	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 3f);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate(new Vector2(0.0f,speed));

	}
}
