using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : NetworkBehaviour {

	GameObject player;
	Transform playerTransform;
	public float speed;
	public string guiMessage;
	//bool actionAvailable;
	string actionType;
	public string actionName;
	//Animator animator;
	GameObject[] chatTexts;
	GameObject[] gameViewports;
	GameObject[] players;
	public Sprite idleUp;
	public Sprite idleRight;
	public Sprite idleDown;
	public Sprite idleLeft;
	public GameObject textPrefab;
	public GameObject gamePrefab;
	bool chatting;
	int direction;
	public LoginManager loginManager;
	public NetworkConnector networkConnector;
	string[] characterData;

	// Use this for initialization
	void Start () 
	{
		if (isLocalPlayer) 
		{
			loginManager = GameObject.Find ("LoginManager").GetComponent<LoginManager>();
			networkConnector = GameObject.Find ("NetworkConnector").GetComponent<NetworkConnector>();
			characterData = loginManager.GetCharacterData (networkConnector.SlotID).Split(",".ToCharArray());
			//characterID,characterName,colorID,credits,userID
			Debug.Log(characterData[0]);
			//string characterID = characterData [0];


			if (characterData [2] == "1")
				gameObject.GetComponent<SpriteRenderer> ().color = Color.grey;
				
			gameObject.GetComponent<PlayerGUI> ().text = characterData [1];

			this.gameObject.GetComponentInChildren<Camera> ().enabled = true;
			player = this.gameObject;
			playerTransform = player.GetComponent<Transform> ();
			//animator = this.gameObject.GetComponent<Animator> ();
			chatting = false;

			this.transform.Find ("HUD/Credits/Box/Text").GetComponent<Text> ().text = characterData [3];
		}
	}

	public void sendMessage()
	{
		string text = this.transform.Find ("ChatCanvas/InputBackground/ChatInput").gameObject.GetComponent<InputField>().text;
		this.transform.Find ("ChatCanvas/InputBackground/ChatInput").gameObject.GetComponent<InputField>().text = "";
		Debug.Log (text);
		if (!isServer)
		{
			CmdSendMessage (text);
		}
		else
		{
			RpcSendMessage (text);
			Debug.Log("Server Clicked Button");
		}
	}

	//Commands
	#region
	[Command]
	public void CmdSendMessage(string message)
	{
		Debug.Log ("Client Clicked Button");
		RpcSendMessage (message);
	}

	[ClientRpc]
	public void RpcSendMessage(string message)
	{
		foreach (GameObject _chatText in chatTexts)
			_chatText.GetComponent<Text>().text += "\n" + characterData[1] + ": " + message;
	}

	[Command]
	public void CmdSetAnimation(GameObject player, string animation)
	{
		RpcSetAnimation (player, animation);
	}

	[ClientRpc]
	public void RpcSetAnimation(GameObject player, string animation)
	{
		Animator animator = player.GetComponent<Animator> ();
		animator.enabled = true;
		animator.SetTrigger (animation);
	}
		
	public void JoinPublicGame()
	{
		//Open the poker table UI

		if (!isServer)
		{
			CmdJoinPublicGame();
		}

		else
		{
			RpcJoinPublicGame ();
			Debug.Log("Server Clicked Button");

		}
	}

	[Command]
	public void CmdJoinPublicGame()
	{

	}

	[ClientRpc]
	public void RpcJoinPublicGame()
	{
		//Increment the players in the game instance
	}

	public void CreatePublicGame()
	{
		if (!isServer)
		{
			CmdCreatePublicGame();
		}
		else
		{
			RpcCreatePublicGame ();
			Debug.Log("Server Clicked Button");
		}
	}

	[Command]
	public void CmdCreatePublicGame()
	{
		RpcCreatePublicGame ();
	}

	[ClientRpc]
	public void RpcCreatePublicGame()
	{
		foreach (GameObject _gameViewport in gameViewports) 
		{
			GameObject game = GameObject.Instantiate(gamePrefab);
			game.transform.SetParent (_gameViewport.transform);
		}
	}
		
	#endregion

	// Update is called once per frame
	void Update () 
	{
		players = GameObject.FindGameObjectsWithTag ("Playuh");
		chatTexts = GameObject.FindGameObjectsWithTag ("ChatText");
		gameViewports = GameObject.FindGameObjectsWithTag ("GameListViewPort");

		if (Input.GetKeyDown (KeyCode.LeftArrow))
			direction = 3;
		if (Input.GetKeyDown (KeyCode.DownArrow))
			direction = 2;
		if (Input.GetKeyDown (KeyCode.RightArrow))
			direction = 1;
		if (Input.GetKeyDown (KeyCode.UpArrow))
			direction = 0;

		if (isLocalPlayer) 
		{
			if (actionType == "open poker" && Input.GetKeyDown(KeyCode.E)) 
			{
				actionType = "close poker";
				displayTables ("Poker");
			}

			if (Input.GetKeyDown (KeyCode.RightShift)) 
			{
				if (!chatting) {
					
					this.transform.Find ("ChatCanvas").GetComponent<Canvas> ().enabled = true;
					this.chatting = true;
					EventSystem.current.SetSelectedGameObject (this.transform.Find ("ChatCanvas/InputBackground/ChatInput").gameObject);
				} 
				else {
					this.transform.Find ("ChatCanvas").GetComponent<Canvas> ().enabled = false;
					this.chatting = false;
				}
			}

			if (Input.GetKeyDown (KeyCode.Return) && this.chatting) 
			{
				sendMessage ();
			}
		}
	}

	void displayTables(string gameType)
	{
		if(isLocalPlayer)
		this.transform.Find("PokerCanvas").GetComponent<Canvas> ().enabled = true;
	}

	void OnGUI () 
	{
		if (isLocalPlayer) 
		{
			GUI.color = Color.white;
			GUILayout.Label (guiMessage);
		}


	}

	void FixedUpdate()
	{

		if (!isLocalPlayer)
			return;

			if (!Input.anyKey) 
			{
			CmdGetIdleSprite (this.gameObject, this.direction);
			}

			if (Input.GetKey (KeyCode.LeftArrow)) 
			{
				if (direction == 3) 
				{
					CmdSetAnimation (this.gameObject, "Walk_Left");
				this.transform.Translate (new Vector3 (-speed, 0, 0));
				}
			}
				
			if (Input.GetKey (KeyCode.RightArrow)) 
			{
				if (direction == 1) {
					CmdSetAnimation (this.gameObject, "Walk_Right");	
				this.transform.Translate (new Vector3 (speed, 0, 0));
				}
			}
			
			if (Input.GetKey (KeyCode.UpArrow)) 
			{
				if (direction == 0) {
					CmdSetAnimation (this.gameObject, "Walk_Forward");
				this.transform.Translate (new Vector3 (0, speed, 0));
				}
			}

			if (Input.GetKey (KeyCode.DownArrow)) 
			{
				if (direction == 2) {
					CmdSetAnimation (this.gameObject, "Walk_Down");
				this.transform.Translate (new Vector3 (0, -speed, 0));
				} 
			}
		}
		
	[Command]
	void CmdGetIdleSprite(GameObject player, int direction)
	{
		RpcGetIdleSprite (player, direction);
	}

	[ClientRpc]
	void RpcGetIdleSprite(GameObject player, int direction)
	{
		player.GetComponent<Animator> ().enabled = false;

		switch (direction) {
		case 0:
			player.GetComponent<SpriteRenderer> ().sprite = idleUp;
			break;
		case 1:
			player.GetComponent<SpriteRenderer> ().sprite = idleRight;
			break;
		case 2:
			player.GetComponent<SpriteRenderer> ().sprite = idleDown;
			break;
		case 3:
			player.GetComponent<SpriteRenderer> ().sprite = idleLeft;
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (isLocalPlayer) 
		{
			if (other.tag == "door") 
			{
				actionName = other.name.ToString ();
				this.guiMessage = actionName + "\nPress E to enter";
				actionType = "door";
				//actionAvailable = true; 
			}

			if (other.tag == "Poker Table") 
			{
				actionName = other.name.ToString ();
				this.guiMessage = actionName + "\nPress E to play";
				actionType = "open poker";
				//actionAvailable = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (isLocalPlayer) 
		{
			this.guiMessage = "";
			actionType = "";
			//actionAvailable = false; 
			if (other.tag == "Poker Table")
				this.transform.Find ("PokerCanvas").GetComponent<Canvas>().enabled = false;
		}
	}
}