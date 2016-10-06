using System.Net;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using System.Xml.Linq;
using System.Linq;

public class LoginManager : MonoBehaviour {

	List<Character> characters; 

	public void Start()
	{
		characters = new List<Character> ();


		//TEST
		GameObject label = this.transform.Find ("Canvas/pCharacterSelect/HorizontalDiv/Slot1/Label").gameObject;
		label.GetComponent<Text> ().text = "START";
	}

	public void LogIn()
	{
		InputField usernameInput = GameObject.Find ("tUsername").GetComponent<InputField>();
		InputField passwordInput = GameObject.Find ("tPassword").GetComponent<InputField>();
		string username = usernameInput.text;
		string password = passwordInput.text;
		HttpWebRequest getCharactersRequest = (HttpWebRequest)WebRequest.Create("http://narratusgame.com/Rest/getcharacters/" + username + "/" + password);
		WebResponse result = getCharactersRequest.GetResponse ();

			//Login Successful
			Stream stream = result.GetResponseStream();
			XmlReader reader = XmlReader.Create (stream);
			

			while (reader.ReadToFollowing ("Character")) 
			{
				Character character = new Character ();
				reader.ReadToDescendant ("characterID");
				character.characterID = int.Parse (reader.ReadElementContentAsString ());
				if (reader.Name != "characterName")
					reader.ReadToFollowing ("characterName");
				character.characterName = reader.ReadElementContentAsString ();
				if (reader.Name != "colorID")
					reader.ReadToFollowing ("colorID");
				character.colorID = int.Parse (reader.ReadElementContentAsString ());
				if (reader.Name != "credits")
					reader.ReadToFollowing ("credits");
				character.credits = int.Parse (reader.ReadElementContentAsString ());
				if (reader.Name != "userID")
					reader.ReadToFollowing ("userID");
				character.userID = int.Parse (reader.ReadElementContentAsString ());
				characters.Add (character);
			}
			
		this.transform.Find ("Canvas/pCharacterSelect").gameObject.SetActive(true);
		this.transform.Find ("Canvas/pSignIn").gameObject.SetActive(false);
		GameObject[] gCharacters = GameObject.FindGameObjectsWithTag("Character");
		foreach (GameObject gCharacter in gCharacters)
			gCharacter.GetComponent<Image> ().enabled = false;
		int counter = 1;

		foreach(Character _character in characters)
		{
			GameObject slot = this.transform.Find ("Canvas/pCharacterSelect/HorizontalDiv/Slot" + counter.ToString ()).gameObject;
			GameObject slotCharacter = this.transform.Find ("Canvas/pCharacterSelect/HorizontalDiv/Slot" + counter.ToString () + "/Fill/Character").gameObject;
			GameObject label = this.transform.Find ("Canvas/pCharacterSelect/HorizontalDiv/Slot" + counter.ToString () + "/Label").gameObject;
			slotCharacter.GetComponent<Image> ().enabled = true;
			label.GetComponent<Text> ().text = _character.characterName;
			if(_character.colorID == 1)
				slotCharacter.GetComponent<Image> ().color = Color.grey;
			if(_character.colorID == 2)
				slotCharacter.GetComponent<Image> ().color = Color.white;
			
			Debug.Log(_character.characterName);
			counter++;
		}
	}

	public string GetCharacterData(int slotID)
	{
		string characterID = characters[slotID].characterID + ",";
		string characterName = characters[slotID].characterName + ",";
		string colorID = characters[slotID].colorID + ",";
		string credits = characters[slotID].credits + ",";
		string userID = characters[slotID].userID.ToString();
		string characterData = characterID + characterName + colorID + credits + userID;
		return characterData;
	}
}