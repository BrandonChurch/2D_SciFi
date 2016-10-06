using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {
	public float speed = .003f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate(new Vector2(0f,speed));
	}
}
