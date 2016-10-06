using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	public float speed;
	GameObject player;
	bool nearPlayer;
	int health;

	void Start ()
	{
		health = 100;
		nearPlayer = false;
		player = GameObject.Find ("Player");
	}
	
	void Update () 
	{
		if (!nearPlayer) {
			transform.position = Vector2.MoveTowards (new Vector2 (transform.position.x, transform.position.y), new Vector2 (player.transform.position.x, player.transform.position.y), speed * Time.deltaTime);
			var dir = player.transform.position - transform.position;
			var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg + 90;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		} 
		else 
		{
			var dir = player.transform.position - transform.position;
			var angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg + 90;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

		}

		if (health <= 0) {
			Destroy(this.gameObject);
		}
	}
	

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.name == "PlayerClose") 
		{
			nearPlayer = true;
		}

		if (other.name == "PlayerFar") 
		{
		}

		if (other.tag == "bullet") {
			Destroy(other.gameObject);
			health = health - 10;
		}
	}
	void OnTriggerExit2D(Collider2D other) 
	{
		if (other.name == "PlayerClose") 
		{
			nearPlayer = false;
		}
	}

}
