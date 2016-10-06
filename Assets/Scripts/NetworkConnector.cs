namespace UnityEngine.Networking
{
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class NetworkConnector : MonoBehaviour
	{
		public NetworkManager manager;
		public LoginManager loginManager;
		public GameObject playerPrefab;
		public int SlotID;

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
		}

		public void JoinServer(int slotID)
		{
			SlotID = slotID;
			string[] characterData = loginManager.GetCharacterData (slotID).Split(",".ToCharArray());
			string characterID = characterData [0];
			GameObject.Destroy(this.transform.Find ("Particle System").gameObject);
			GameObject.Find ("LoginManager").GetComponentInChildren<Canvas> ().enabled = false;

			if (characterID == "1") 
			{
				manager.StartHost ();
			}
			else 
			{
				manager.networkAddress = ("192.168.0.4");
				manager.StartClient ();
			}
		}
	}
}