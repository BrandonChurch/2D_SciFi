using UnityEngine;
using System.Collections;
using System;
using System.Xml.Serialization;



[XmlRoot]
public class Character 
{
	[XmlElement]
	public int characterID { get; set; }

	[XmlElement]
	public string characterName { get; set; }

	[XmlElement]
	public int colorID { get; set; }

	[XmlElement]
	public int credits { get; set; }

	[XmlElement]
	public int userID { get; set; }
}
