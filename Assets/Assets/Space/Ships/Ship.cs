using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class Ship : MonoBehaviour {
	
	public Sprite idle;
	public Sprite boosting;
	public Sprite moving;
	private SpriteRenderer spriteRenderer;
	public GameObject nextShip;
	public GameObject bulletPrefab;
	public float speed;
	GameObject player;
	protected internal Rigidbody2D rigidbody;
	public float maxSpeed = 300f;
	public string guiMessage;
	public float turnSpeed;
	public float bulletSpeed;
	public bool actionAvailable;
	public string actionName;
	public string actionType;
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	//CAMERA STUFF
	public Transform target;
	// Time in seconds for smoothing
	public float smoothTime = 0.5f;
	// Distance from target
	public float distance = 5.0f;
	// Velocity of camera smoothing
	Vector3 _smoothVelocity;



	void OnGUI () 
	{
		GUI.color = Color.white;
		GUILayout.Label(guiMessage);
	}
	
	void Start () 
	{

		rigidbody = this.gameObject.GetComponent<Rigidbody2D> ();
		player = this.gameObject;
		spriteRenderer = this.GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
		if (spriteRenderer.sprite == null) // if the sprite on spriteRenderer is null then
			spriteRenderer.sprite = idle;

		//CAMERA STUFF


		if (target == null) {
			// Point at player by default
			target = this.transform;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{		

		Debug.Log (this.gameObject.GetComponent<Rigidbody2D> ().velocity);


			if (target != null) 
		{
				// Point camera towards target
				Vector3 targetPosition = target.position;
				targetPosition.z -= distance;

				//Camera.main.transform.position = Vector3.SmoothDamp (Camera.main.transform.position, targetPosition, ref _smoothVelocity, smoothTime);


			if (actionAvailable && Input.GetKeyDown (KeyCode.E)) 
			{

				if (actionType == "planet") 
				{

				}
			}
		}
	}

	void FixedUpdate () 
	{    

			if (Input.GetKey (KeyCode.LeftArrow)) {
				if (!Input.GetKey (KeyCode.Space)) {
					transform.Rotate (new Vector3 (0, 0, turnSpeed) * Time.fixedDeltaTime);
					spriteRenderer.sprite = idle;
				} else {
					transform.Rotate (new Vector3 (0, 0, turnSpeed * 2) * Time.fixedDeltaTime);
					spriteRenderer.sprite = boosting;
				}
			}

			if (Input.GetKey (KeyCode.RightArrow)) {
				if (!Input.GetKey (KeyCode.Space)) {
					transform.Rotate (new Vector3 (0, 0, -turnSpeed) * Time.fixedDeltaTime);
					spriteRenderer.sprite = idle;
				} else {
					transform.Rotate (new Vector3 (0, 0, -turnSpeed * 2) * Time.fixedDeltaTime);
					spriteRenderer.sprite = boosting;
				}
			} 

			if (Input.GetKey (KeyCode.UpArrow)) {

				if (!Input.GetKey (KeyCode.Space)) {
					Vector2 dir = (player.transform.position);
					dir.Normalize ();
					GetComponent<Rigidbody2D> ().AddForce (transform.up * 1, ForceMode2D.Force);
					spriteRenderer.sprite = moving;
				} else {
					Vector2 dir = (player.transform.position);
					dir.Normalize ();
					GetComponent<Rigidbody2D> ().AddForce (transform.up * 3, ForceMode2D.Force);
					rigidbody.velocity = Vector2.ClampMagnitude (rigidbody.velocity, maxSpeed);
					spriteRenderer.sprite = boosting;
				}
			}

			if (!Input.GetKey (KeyCode.UpArrow)) {
				spriteRenderer.sprite = idle;
			}


		if (Input.GetKeyUp(KeyCode.V)) 
		{
			//Instantiate(bullet, new Vector2(transform.position.x, transform.position.y + .1f),this.GetComponentInParent<Transform>().rotation);

			spawnBullet ();
		}


			//Prevent infinite acceleration
			if (rigidbody.velocity.magnitude > maxSpeed) {
				rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
			}
	}
		
	//[Command]
	void spawnBullet ()
	{

		GameObject bullet = (GameObject)Instantiate(bulletPrefab, new Vector2(this.transform.position.x, this.transform.position.y + .2f),this.GetComponentInParent<Transform>().rotation);
		//NetworkServer.Spawn (bullet);
		//bullet.transform.Translate(new Vector2(bulletSpeed,0.0f));



	}

	void OnCollisionEnter2D(Collision2D other) 
	{
		//if (!this.gameObject.GetComponent<NetworkIdentity> ().isLocalPlayer)
		//	return;
		
		this.rigidbody.freezeRotation = true;
	}

	void OnCollisionExit2D(Collision2D other) 
	{
		
		Debug.Log ("Exit collision");
		this.rigidbody.freezeRotation = false;
	}
	void OnTriggerEnter2D(Collider2D other) 
	{

		//if (!this.gameObject.GetComponent<NetworkIdentity> ().isLocalPlayer)
		//	return;
		
		if (other.tag == "planet") 
		{
			actionAvailable = true;
			actionType = "planet";
			actionName = other.name.ToString ();
			this.guiMessage = "Planet " + other.name.ToString() + "\nPress E to land";
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				actionAvailable = false;
			}
		}
	}
	void OnTriggerExit2D(Collider2D other) 
	{
		
		if (other.tag == "planet") 
		{
			this.guiMessage = "";
			actionAvailable = false;
		}
	}
}
