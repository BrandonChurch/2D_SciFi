using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Gun : NetworkBehaviour {

	public GameObject bullet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	


		if (Input.GetKeyUp(KeyCode.V)) 
		{
			//Instantiate(bullet, new Vector2(transform.position.x, transform.position.y + .1f),this.GetComponentInParent<Transform>().rotation);

			CmdspawnBullet ();
		}


	}

	[Command]
	void CmdspawnBullet ()
	{
		
		Instantiate(bullet, new Vector2(transform.position.x, transform.position.y + .1f),this.GetComponentInParent<Transform>().rotation);
	}
}
